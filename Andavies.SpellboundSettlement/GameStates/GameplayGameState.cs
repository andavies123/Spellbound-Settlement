using System;
using System.Collections.Concurrent;
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
using Andavies.SpellboundSettlement.Wizards;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Serilog;

namespace Andavies.SpellboundSettlement.GameStates;

public class GameplayGameState : GameState
{
	private readonly ILogger _logger;
	private readonly INetworkClient _networkClient;
	private readonly ITileRegistry _tileRegistry;
	private readonly IChunkMeshBuilder _chunkMeshBuilder;
	private readonly IChunkDrawManager _chunkDrawManager;
	private readonly ITileHoverHandler _tileHoverHandler;
	private readonly IModelDrawManager _modelDrawManager;
	private readonly WorldMesh _worldMesh = new();

	private readonly ConcurrentDictionary<Vector2Int, Chunk> _chunks = new();
	private readonly ConcurrentDictionary<Guid, Wizard> _wizards = new();
	private readonly ConcurrentDictionary<Type, WizardDrawDetails> _wizardDrawDetails = new();

	private readonly GameplayUIState _gameplayGameplayUIState;
	
	public GameplayGameState(
		ILogger logger,
		INetworkClient networkClient,
		ITileRegistry tileRegistry,
		IChunkMeshBuilder chunkMeshBuilder,
		IChunkDrawManager chunkDrawManager,
		ITileHoverHandler tileHoverHandler,
		IModelDrawManager modelDrawManager,
		GameplayUIState gameplayUIState,
		GameplayInputState inputState)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_networkClient = networkClient ?? throw new ArgumentNullException(nameof(networkClient));
		_tileRegistry = tileRegistry ?? throw new ArgumentNullException(nameof(tileRegistry));
		_chunkMeshBuilder = chunkMeshBuilder ?? throw new ArgumentNullException(nameof(chunkMeshBuilder));
		_chunkDrawManager = chunkDrawManager ?? throw new ArgumentNullException(nameof(chunkDrawManager));
		_tileHoverHandler = tileHoverHandler ?? throw new ArgumentNullException(nameof(tileHoverHandler));
		_modelDrawManager = modelDrawManager ?? throw new ArgumentNullException(nameof(modelDrawManager));
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
		RegisterWizardDrawDetails();
		
		UIStateMachine.ChangeUIState(_gameplayGameplayUIState);

		_networkClient.AddSubscription<WorldChunkResponsePacket>(OnWorldChunkResponsePacketReceived);
		_networkClient.AddSubscription<WizardAddedPacket>(OnWizardAddedPacketReceived);
		_networkClient.AddSubscription<WizardRemovedPacket>(OnWizardRemovedPacketReceived);
		_networkClient.AddSubscription<WizardUpdatedPacket>(OnWizardUpdatedPacketReceived);

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

		foreach (Wizard wizard in _wizards.Values)
		{
			DrawWizard(wizard);
		}
	}
	
	public override void End()
	{
		base.End();
		
		_networkClient.RemoveSubscription<WorldChunkResponsePacket>(OnWorldChunkResponsePacketReceived);
		_networkClient.RemoveSubscription<WizardAddedPacket>(OnWizardAddedPacketReceived);
		_networkClient.RemoveSubscription<WizardRemovedPacket>(OnWizardRemovedPacketReceived);
		_networkClient.RemoveSubscription<WizardUpdatedPacket>(OnWizardUpdatedPacketReceived);
		
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
		grassTile.Model = Global.GameManager.Content.Load<Model>(grassTile.ModelDetails.ContentModelPath);
		SmallRockTile smallRockTile = new();
		smallRockTile.Model = Global.GameManager.Content.Load<Model>(smallRockTile.ModelDetails.ContentModelPath);
		BushTile bushTile = new();
		bushTile.Model = Global.GameManager.Content.Load<Model>(bushTile.ModelDetails.ContentModelPath);
        
		_tileRegistry.RegisterTile(new AirTile());
		_tileRegistry.RegisterTile(new GroundTile());
		_tileRegistry.RegisterTile(grassTile);
		_tileRegistry.RegisterTile(smallRockTile);
		_tileRegistry.RegisterTile(bushTile);
	}

	private void RegisterWizardDrawDetails()
	{
		BasicWizardDrawDetails basicWizardDrawDetails = new();
		basicWizardDrawDetails.Model = Global.GameManager.Content.Load<Model>(basicWizardDrawDetails.ModelDetails.ContentModelPath);
		
		_wizardDrawDetails.TryAdd(basicWizardDrawDetails.WizardType, basicWizardDrawDetails);
	}

	private void DrawWizard(Wizard wizard)
	{
		if (!_wizardDrawDetails.TryGetValue(wizard.GetType(), out WizardDrawDetails wizardDrawDetails))
		{
			_logger.Warning("Wizard draw details does not exist: {type}", wizard.GetType());
			return;
		}
		
		_modelDrawManager.DrawModel(wizardDrawDetails.Model, wizardDrawDetails.ModelDetails, (Vector3)wizard.WorldPosition, 1f, 0);
	}

	private void OnWorldChunkResponsePacketReceived(INetSerializable packet)
	{
		if (packet is not WorldChunkResponsePacket worldChunkResponsePacket)
			return;

		if (worldChunkResponsePacket.Chunk == null)
			return;
		
		_chunks.TryAdd(worldChunkResponsePacket.Chunk.ChunkPosition, worldChunkResponsePacket.Chunk);
		
		ChunkMesh chunkMesh = _chunkMeshBuilder.BuildChunkMesh(worldChunkResponsePacket.Chunk);
		_worldMesh.SetChunkMesh(chunkMesh, worldChunkResponsePacket.Chunk.ChunkPosition);
	}

	private void OnWizardAddedPacketReceived(INetSerializable packet)
	{
		if (packet is not WizardAddedPacket wizardAddedPacket || wizardAddedPacket.Wizard == null)
			return;
		
		_wizards.TryAdd(wizardAddedPacket.Wizard.Id, wizardAddedPacket.Wizard);
		_logger.Debug("Wizard added!");
	}

	private void OnWizardRemovedPacketReceived(INetSerializable packet)
	{
		if (packet is not WizardAddedPacket wizardAddedPacket || wizardAddedPacket.Wizard == null)
			return;
		
		_wizards.TryRemove(wizardAddedPacket.Wizard.Id, out Wizard _);
	}

	private void OnWizardUpdatedPacketReceived(INetSerializable packet)
	{
		if (packet is not WizardAddedPacket wizardAddedPacket || wizardAddedPacket.Wizard == null)
			return;

		_wizards[wizardAddedPacket.Wizard.Id] = wizardAddedPacket.Wizard;
	}

	private void UnloadTileModels()
	{
		foreach (ModelTile modelTile in _tileRegistry.GetAllTilesOfType<ModelTile>())
		{
			Global.GameManager.Content.UnloadAsset(modelTile.ModelDetails.ContentModelPath);
		}
	}

	private void OnMouseMoved()
	{
		_tileHoverHandler.UpdateHover(_worldMesh);
	}
}