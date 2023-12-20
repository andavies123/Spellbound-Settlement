using System;
using System.Collections.Generic;
using System.Linq;
using Andavies.MonoGame.Inputs;
using Andavies.MonoGame.Meshes;
using Andavies.MonoGame.Network.Client;
using Andavies.MonoGame.Utilities.Extensions;
using Andavies.SpellboundSettlement.CameraObjects;
using Andavies.SpellboundSettlement.Inputs;
using Andavies.SpellboundSettlement.Meshes;
using Andavies.SpellboundSettlement.NetworkMessages.Messages.World;
using Andavies.SpellboundSettlement.UIStates.Gameplay;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Serilog;

namespace Andavies.SpellboundSettlement.GameStates;

public class GameplayGameState : GameState
{
	private readonly ILogger _logger;
	private readonly INetworkClient _networkClient;
	private readonly Camera _camera;
	private readonly WorldMesh _worldMesh = new();

	private readonly GameplayUIState _gameplayGameplayUIState;

	// Mouse hover objects
	private ChunkMesh _hoveredChunk = null;
	private (int x, int y, int z)? _hoveredTile = null;
	
	public GameplayGameState(
		ILogger logger,
		INetworkClient networkClient,
		GameplayUIState gameplayUIState, 
		GameplayInputState inputState,
		Camera camera)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_networkClient = networkClient ?? throw new ArgumentNullException(nameof(networkClient));
		_gameplayGameplayUIState = gameplayUIState ?? throw new ArgumentNullException(nameof(gameplayUIState));
		InputState = inputState ?? throw new ArgumentNullException(nameof(inputState));
		_camera = camera ?? throw new ArgumentNullException(nameof(camera));
		
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
		const int chunkRadius = 5;
		for (int x = 0; x < chunkRadius; x++)
		{
			for (int y = 0; y < chunkRadius; y++)
			{
				chunkPositions.Add(new Vector2(x, y));
			}
		}
		_networkClient.SendMessage(new WorldChunkRequestPacket {ChunkPositions = chunkPositions });

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
		Input.MouseMoved -= OnMouseMoved;
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

	private void HandleMouseHover()
	{
		// Clear at beginning so it doesn't get redrawn
		ClearHoveredTile();
		
		// Distance doesn't matter as we want it to go forever
		Ray ray = _camera.GetRayFromCamera(GameManager.Viewport, Input.CurrentMousePosition.ToVector2(), 1);

		// Find all chunks that intersect with ray cast
		List<(ChunkMesh, float)> chunkMeshesByDistance = new();
		foreach (ChunkMesh chunkMesh in _worldMesh.ChunkMeshes.Values)
		{
			float? chunkDistance = ray.Intersects(chunkMesh.Collider);
			if (!chunkDistance.HasValue)
				continue;

			chunkMeshesByDistance.Add((chunkMesh, chunkDistance.Value));
		}

		// If we aren't hovered any chunks we don't need to continue
		if (chunkMeshesByDistance.IsEmpty())
			return;
		
		// Sort list by distance so we eventually don't have to go through all chunk tiles.
		// Only until we hit a chunk where we hit a tile that is visible
		chunkMeshesByDistance = chunkMeshesByDistance.OrderBy(tuple => tuple.Item2).ToList();
		
		// Find the closest tile
		ChunkMesh closestChunk = null;
		(int x, int y, int z)? closestTile = null;
		float smallestDistance = float.MaxValue;
		foreach ((ChunkMesh chunkMesh, float _) in chunkMeshesByDistance)
		{
			for (int x = 0; x < chunkMesh.TileColliders.GetLength(0); x++)
			{
				for (int y = 0; y < chunkMesh.TileColliders.GetLength(1); y++)
				{
					for (int z = 0; z < chunkMesh.TileColliders.GetLength(2); z++)
					{
						if (!chunkMesh.TileMeshes[x, y, z].IsVisible) // Don't check tiles that can't be seen
							continue;

						BoundingBox tileCollider = chunkMesh.TileColliders[x, y, z];
						float? tileDistance = ray.Intersects(tileCollider);
						if (!tileDistance.HasValue || tileDistance >= smallestDistance)
							continue;
						
						closestTile = (x, y, z);
						smallestDistance = tileDistance.Value;
					}
				}
			}

			// If we found in a chunk then we don't have to look at the rest of the chunks since the chunk list is sorted by closest
			if (closestTile != null)
			{
				closestChunk = chunkMesh;
				break;
			}
		}

		// If closestTile comes back null we don't need to continue
		if (closestTile == null)
			return;
		
		SetHoveredTile(closestChunk, closestTile.Value);
	}

	private void SetHoveredTile(ChunkMesh chunkMesh, (int x, int y, int z) tilePosition)
	{
		if (chunkMesh == null)
			return;

		_hoveredChunk = chunkMesh;
		_hoveredTile = tilePosition;
		
		const float colorIncrease = .2f;
		Vector3 tileColorValues = WorldMeshConstants.HeightColors[_hoveredTile.Value.y].ToVector3();
		Color hoveredColor = new(tileColorValues - new Vector3(colorIncrease));
		chunkMesh.SetTileColor(_hoveredTile.Value.x, _hoveredTile.Value.y, _hoveredTile.Value.z, hoveredColor);
	}

	private void ClearHoveredTile()
	{
		if (_hoveredChunk != null && _hoveredTile != null)
			_hoveredChunk.SetTileColor(_hoveredTile.Value.x, _hoveredTile.Value.y, _hoveredTile.Value.z, WorldMeshConstants.HeightColors[_hoveredTile.Value.y]);

		_hoveredChunk = null;
		_hoveredTile = null;
	}

	private void OnWorldChunkResponsePacketReceived(INetSerializable packet)
	{
		if (packet is not WorldChunkResponsePacket response)
			return;
		
		_worldMesh.SetChunk(response.Chunk);
	}

	private void OnMouseMoved()
	{
		HandleMouseHover();
	}
}