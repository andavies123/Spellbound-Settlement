﻿using System;
using System.Collections.Generic;
using Andavies.MonoGame.Inputs;
using Andavies.MonoGame.Network.Client;
using Andavies.MonoGame.Utilities;
using Andavies.SpellboundSettlement.GameWorld;
using Andavies.SpellboundSettlement.GameWorld.Repositories;
using Andavies.SpellboundSettlement.GameWorld.Tiles;
using Andavies.SpellboundSettlement.Globals;
using Andavies.SpellboundSettlement.Inputs;
using Andavies.SpellboundSettlement.Meshes;
using Andavies.SpellboundSettlement.NetworkMessages.Messages.World;
using Andavies.SpellboundSettlement.UIStates.Gameplay;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.SpellboundSettlement.GameStates;

public class GameplayGameState : GameState
{
	private readonly INetworkClient _networkClient;
	private readonly ITileRegistry _tileRegistry;
	private readonly IChunkMeshBuilder _chunkMeshBuilder;
	private readonly IChunkDrawManager _chunkDrawManager;
	private readonly ITileHoverHandler _tileHoverHandler;
	private readonly WorldMesh _worldMesh = new();

	private readonly Dictionary<Vector2Int, Chunk> _chunks = new();

	private readonly GameplayUIState _gameplayGameplayUIState;
	
	public GameplayGameState(
		INetworkClient networkClient,
		ITileRegistry tileRegistry,
		IChunkMeshBuilder chunkMeshBuilder,
		IChunkDrawManager chunkDrawManager,
		ITileHoverHandler tileHoverHandler,
		GameplayUIState gameplayUIState,
		GameplayInputState inputState)
	{
		_networkClient = networkClient ?? throw new ArgumentNullException(nameof(networkClient));
		_tileRegistry = tileRegistry ?? throw new ArgumentNullException(nameof(tileRegistry));
		_chunkMeshBuilder = chunkMeshBuilder ?? throw new ArgumentNullException(nameof(chunkMeshBuilder));
		_chunkDrawManager = chunkDrawManager ?? throw new ArgumentNullException(nameof(chunkDrawManager));
		_tileHoverHandler = tileHoverHandler ?? throw new ArgumentNullException(nameof(tileHoverHandler));
		_gameplayGameplayUIState = gameplayUIState ?? throw new ArgumentNullException(nameof(gameplayUIState));
		InputState = inputState ?? throw new ArgumentNullException(nameof(inputState));
		
		UIStates.Add(gameplayUIState);
	}
	
	public event Action PauseGameRequested;

	public override GameplayInputState InputState { get; }

	public override void Start()
	{
		base.Start();
		
		RegisterTiles();
		
		UIStateMachine.ChangeUIState(_gameplayGameplayUIState);

		_networkClient.AddSubscription<WorldChunkResponsePacket>(OnWorldChunkResponsePacketReceived);

		List<Vector2Int> chunkPositions = new();
		const int chunkRadius = 5;
		for (int x = 0; x < chunkRadius; x++)
		{
			for (int y = 0; y < chunkRadius; y++)
			{
				chunkPositions.Add(new Vector2Int(x, y));
			}
		}

		_networkClient.SendMessage(new WorldChunkRequestPacket {ChunkPositions = chunkPositions});

		_gameplayGameplayUIState.PauseButtonClicked += OnPauseGameClicked;
		InputState.PauseGame.OnKeyUp += OnPauseGameKeyReleased;
		Input.MouseMoved += OnMouseMoved;
	}
	
	public override void Update(float deltaTimeSeconds)
	{
		base.Update(deltaTimeSeconds);
		_networkClient.Update();
	}

	public override void Draw3D(GraphicsDevice graphicsDevice)
	{
		base.Draw3D(graphicsDevice);
		
		// Draw world using the IChunkDrawManager
		foreach (ChunkMesh chunkMesh in _worldMesh.ChunkMeshes)
		{
			_chunkDrawManager.DrawChunk(chunkMesh);
		}
	}
	
	public override void End()
	{
		base.End();
		
		_networkClient.RemoveSubscription<WorldChunkResponsePacket>(OnWorldChunkResponsePacketReceived);
		
		_gameplayGameplayUIState.PauseButtonClicked -= OnPauseGameClicked;
		InputState.PauseGame.OnKeyUp -= OnPauseGameKeyReleased;
		Input.MouseMoved -= OnMouseMoved;
		
		UnloadTileModels();
	}
	
	private void OnPauseGameKeyReleased() => PauseGameRequested?.Invoke();
	private void OnPauseGameClicked() => PauseGameRequested?.Invoke();

	private void RegisterTiles()
	{
		GrassTile grassTile = new();
		grassTile.Model = Global.GameManager.Content.Load<Model>(grassTile.ContentModelPath);
		SmallRockTile smallRockTile = new();
		smallRockTile.Model = Global.GameManager.Content.Load<Model>(smallRockTile.ContentModelPath);
        
		_tileRegistry.RegisterTile(new AirTile());
		_tileRegistry.RegisterTile(new GroundTile());
		_tileRegistry.RegisterTile(grassTile);
		_tileRegistry.RegisterTile(smallRockTile);
	}

	private void OnWorldChunkResponsePacketReceived(INetSerializable packet)
	{
		if (packet is not WorldChunkResponsePacket response)
			return;

		if (response.Chunk == null)
			return;
		
		_chunks.Add(response.Chunk.ChunkPosition, response.Chunk);
		
		ChunkMesh chunkMesh = _chunkMeshBuilder.BuildChunkMesh(response.Chunk);
		_worldMesh.SetChunkMesh(chunkMesh, response.Chunk.ChunkPosition);
	}

	private void UnloadTileModels()
	{
		foreach (ModelTile modelTile in _tileRegistry.GetAllTilesOfType<ModelTile>())
		{
			Global.GameManager.Content.UnloadAsset(modelTile.ContentModelPath);
		}
	}

	private void OnMouseMoved()
	{
		_tileHoverHandler.UpdateHover(_worldMesh);
	}
}