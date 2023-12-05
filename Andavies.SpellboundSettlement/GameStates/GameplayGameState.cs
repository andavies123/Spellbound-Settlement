using System;
using System.Collections.Generic;
using Andavies.MonoGame.Meshes;
using Andavies.MonoGame.Network.Client;
using Andavies.SpellboundSettlement.CameraObjects;
using Andavies.SpellboundSettlement.Inputs;
using Andavies.SpellboundSettlement.Meshes;
using Andavies.SpellboundSettlement.NetworkMessages.Messages.World;
using Andavies.SpellboundSettlement.UIStates.Gameplay;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.SpellboundSettlement.GameStates;

public class GameplayGameState : GameState
{
	private readonly INetworkClient _networkClient;
	private readonly Camera _camera;
	private readonly WorldMesh _worldMesh = new();

	private readonly GameplayUIState _gameplayGameplayUIState;
	
	public GameplayGameState(
		INetworkClient networkClient,
		GameplayUIState gameplayUIState, 
		GameplayInputState inputState,
		Camera camera)
	{
		_networkClient = networkClient;
		_gameplayGameplayUIState = gameplayUIState;
		InputState = inputState;
		_camera = camera;
		
		UIStates.Add(gameplayUIState);
	}
	
	public event Action PauseGameRequested;

	public override GameplayInputState InputState { get; }

	public override void Start()
	{
		base.Start();
		
		UIStateMachine.ChangeUIState(_gameplayGameplayUIState);
		
		_networkClient.AddSubscription<WorldChunkResponsePacket>(OnWorldChunkResponsePacketReceived);

		List<Vector2> chunkPositions = new();
		for (int x = 0; x < 5; x++)
		{
			for (int y = 0; y < 5; y++)
			{
				chunkPositions.Add(new Vector2(x, y));
			}
		}
		_networkClient.SendMessage(new WorldChunkRequestPacket {ChunkPositions = chunkPositions });

		_gameplayGameplayUIState.PauseButtonClicked += OnPauseGameClicked;
		InputState.PauseGame.OnKeyUp += OnPauseGameKeyReleased;
	}
	
	public override void Update(float deltaTimeSeconds)
	{
		base.Update(deltaTimeSeconds);
		_networkClient.Update();
	}

	public override void Draw3D(GraphicsDevice graphicsDevice)
	{
		base.Draw3D(graphicsDevice);
		
		// Draw World
		foreach (ChunkMesh chunkMesh in _worldMesh.ChunkMeshes.Values)
			DrawMesh(graphicsDevice, chunkMesh);
	}
	
	public override void End()
	{
		base.End();
		
		_networkClient.RemoveSubscription<WorldChunkResponsePacket>(OnWorldChunkResponsePacketReceived);
		
		_gameplayGameplayUIState.PauseButtonClicked -= OnPauseGameClicked;
		InputState.PauseGame.OnKeyUp -= OnPauseGameKeyReleased;
	}
	
	private void OnPauseGameKeyReleased() => PauseGameRequested?.Invoke();
	private void OnPauseGameClicked() => PauseGameRequested?.Invoke();
	
	private void DrawMesh(GraphicsDevice graphicsDevice, IMesh mesh)
	{
		if (mesh.Vertices.Length == 0 || mesh.Indices.Length == 0)
			return;
		
		VertexPositionColor[] vertices = mesh.Vertices;
		int[] indices = mesh.Indices;
		
		VertexBuffer vertexBuffer = new(graphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.None);
		vertexBuffer.SetData(vertices);

		IndexBuffer indexBuffer = new(graphicsDevice, IndexElementSize.ThirtyTwoBits, indices.Length, BufferUsage.None);
		indexBuffer.SetData(indices);
		
		graphicsDevice.SetVertexBuffer(vertexBuffer);
		graphicsDevice.Indices = indexBuffer;
		
		GameManager.Effect.Parameters["WorldMatrix"].SetValue(_camera.WorldMatrix);
		GameManager.Effect.Parameters["ViewMatrix"].SetValue(_camera.ViewMatrix);
		GameManager.Effect.Parameters["ProjectionMatrix"].SetValue(_camera.ProjectionMatrix);
		GameManager.Effect.CurrentTechnique.Passes[0].Apply();
		
		graphicsDevice.DrawIndexedPrimitives(
			PrimitiveType.TriangleList,
			0,
			0,
			indexBuffer.IndexCount / 3);
	}

	private void OnWorldChunkResponsePacketReceived(INetSerializable packet)
	{
		if (packet is not WorldChunkResponsePacket response)
			return;
		
		_worldMesh.SetChunk(response.Chunk);
	}
}