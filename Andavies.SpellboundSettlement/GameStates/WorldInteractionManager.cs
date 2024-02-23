using System;
using System.Collections.Generic;
using System.Linq;
using Andavies.MonoGame.Inputs;
using Andavies.MonoGame.Network.Client;
using Andavies.MonoGame.Utilities;
using Andavies.MonoGame.Utilities.Extensions;
using Andavies.SpellboundSettlement.CameraObjects;
using Andavies.SpellboundSettlement.GameWorld;
using Andavies.SpellboundSettlement.GameWorld.Tiles;
using Andavies.SpellboundSettlement.Meshes;
using Andavies.SpellboundSettlement.NetworkMessages.Messages.World;
using Microsoft.Xna.Framework;

namespace Andavies.SpellboundSettlement.GameStates;

public class WorldInteractionManager : IWorldInteractionManager
{
	private readonly INetworkClient _networkClient;
	private readonly IInputManager _inputManager;
	private readonly ITileHoverHandler _tileHoverHandler;
	private readonly WorldMesh _worldMesh;
	private readonly Camera _camera;
    
	public WorldInteractionManager(
		INetworkClient networkClient,
		IInputManager inputManager,
		ITileHoverHandler tileHoverHandler,
		WorldMesh worldMesh,
		Camera camera)
	{
		_networkClient = networkClient ?? throw new ArgumentNullException(nameof(networkClient));
		_inputManager = inputManager ?? throw new ArgumentNullException(nameof(inputManager));
		_tileHoverHandler = tileHoverHandler ?? throw new ArgumentNullException(nameof(tileHoverHandler));
		_worldMesh = worldMesh ?? throw new ArgumentNullException(nameof(worldMesh));
		_camera = camera ?? throw new ArgumentNullException(nameof(camera));
	}

	public void UpdateTileHover()
	{
		_tileHoverHandler.UpdateHover(_worldMesh);
	}

	public void CheckPrimaryInteraction()
	{
		if (!TryGetWorldTileUnderMouse(out ChunkMesh closestChunkMesh, out Vector3Int? closestTilePosition) || closestTilePosition == null)
			return;
		
		_networkClient.SendMessage(new UpdateTileRequestPacket
		{
			TileId = nameof(AirTile),
			WorldTilePosition = WorldHelper.ChunkAndTilePositionToWorldPosition(closestChunkMesh.Chunk.ChunkPosition, closestTilePosition.Value)
		});
	}

	public void CheckSecondaryInteraction()
	{
		if (!TryGetAdjacentWorldTileUnderMouse(out ChunkMesh closestChunkMesh, out Vector3Int? closestTilePosition) || closestTilePosition == null)
			return;
		
		_networkClient.SendMessage(new UpdateTileRequestPacket
		{
			TileId = nameof(GroundTile),
			WorldTilePosition = WorldHelper.ChunkAndTilePositionToWorldPosition(closestChunkMesh.Chunk.ChunkPosition, closestTilePosition.Value)
		});
	}

	private bool TryGetWorldTileUnderMouse(out ChunkMesh closestChunkMesh, out Vector3Int? closestTilePosition)
	{
		closestChunkMesh = null;
		closestTilePosition = default;
        
		// Distance doesn't matter as we want it to go forever
		Ray ray = _camera.GetRayFromCamera(GameManager.Viewport, _inputManager.CurrentMousePosition.ToVector2(), 1);
		
		// Find all chunks that intersect with ray cast
		List<(ChunkMesh, float)> chunkMeshesByDistance = new();
		foreach (ChunkMesh chunkMesh in _worldMesh.ChunkMeshes)
		{
			float? chunkDistance = ray.Intersects(chunkMesh.ChunkMeshCollider.ChunkCollider.Collider);
			if (!chunkDistance.HasValue)
				continue;
		
			chunkMeshesByDistance.Add((chunkMesh, chunkDistance.Value));
		}
		
		// If we aren't hovered any chunks we don't need to continue
		if (chunkMeshesByDistance.IsEmpty())
			return false;
		
		// Sort list by distance so we eventually don't have to go through all chunk tiles.
		// Only until we hit a chunk where we hit a tile that is visible
		chunkMeshesByDistance = chunkMeshesByDistance.OrderBy(tuple => tuple.Item2).ToList();
		
		// Find the closest tile
		float smallestDistance = float.MaxValue;
		foreach ((ChunkMesh chunkMesh, float _) in chunkMeshesByDistance)
		{
			for (int x = 0; x < chunkMesh.ChunkMeshCollider.TileColliders.GetLength(0); x++)
			{
				for (int y = 0; y < chunkMesh.ChunkMeshCollider.TileColliders.GetLength(1); y++)
				{
					for (int z = 0; z < chunkMesh.ChunkMeshCollider.TileColliders.GetLength(2); z++)
					{
						CubeMesh cubeMesh = chunkMesh.TerrainMesh.GetCubeMesh(new Vector3Int(x, y, z));
						if (cubeMesh == null || !cubeMesh.IsVisible) // Don't check tiles that can't be seen
							continue;
		
						BoundingBox tileCollider = chunkMesh.ChunkMeshCollider.TileColliders[x, y, z].Collider;
						float? tileDistance = ray.Intersects(tileCollider);
						if (!tileDistance.HasValue || tileDistance >= smallestDistance)
							continue;
						
						closestTilePosition = new Vector3Int(x, y, z);
						smallestDistance = tileDistance.Value;
					}
				}
			}
		
			// If we found in a chunk then we don't have to look at the rest of the chunks since the chunk list is sorted by closest
			if (closestTilePosition != null)
			{
				closestChunkMesh = chunkMesh;
				break;
			}
		}
		
		return closestTilePosition != null;
	}
	
	private bool TryGetAdjacentWorldTileUnderMouse(out ChunkMesh closestChunkMesh, out Vector3Int? adjacentTilePosition)
	{
		closestChunkMesh = null;
		adjacentTilePosition = default;
        
		// Distance doesn't matter as we want it to go forever
		Ray ray = _camera.GetRayFromCamera(GameManager.Viewport, _inputManager.CurrentMousePosition.ToVector2(), 1);
		
		// Find all chunks that intersect with ray cast
		List<(ChunkMesh, float)> chunkMeshesByDistance = new();
		foreach (ChunkMesh chunkMesh in _worldMesh.ChunkMeshes)
		{
			float? chunkDistance = ray.Intersects(chunkMesh.ChunkMeshCollider.ChunkCollider.Collider);
			if (!chunkDistance.HasValue)
				continue;
		
			chunkMeshesByDistance.Add((chunkMesh, chunkDistance.Value));
		}
		
		// If we aren't hovered any chunks we don't need to continue
		if (chunkMeshesByDistance.IsEmpty())
			return false;
		
		// Sort list by distance so we eventually don't have to go through all chunk tiles.
		// Only until we hit a chunk where we hit a tile that is visible
		chunkMeshesByDistance = chunkMeshesByDistance.OrderBy(tuple => tuple.Item2).ToList();
		
		// Find the closest tile
		float smallestDistance = float.MaxValue;
		Vector3Int? closestTilePosition = null;
		foreach ((ChunkMesh chunkMesh, float _) in chunkMeshesByDistance)
		{
			for (int x = 0; x < chunkMesh.ChunkMeshCollider.TileColliders.GetLength(0); x++)
			{
				for (int y = 0; y < chunkMesh.ChunkMeshCollider.TileColliders.GetLength(1); y++)
				{
					for (int z = 0; z < chunkMesh.ChunkMeshCollider.TileColliders.GetLength(2); z++)
					{
						CubeMesh cubeMesh = chunkMesh.TerrainMesh.GetCubeMesh(new Vector3Int(x, y, z));
						if (cubeMesh == null || !cubeMesh.IsVisible) // Don't check tiles that can't be seen
							continue;
		
						BoundingBox tileCollider = chunkMesh.ChunkMeshCollider.TileColliders[x, y, z].Collider;
						float? tileDistance = ray.Intersects(tileCollider);
						if (!tileDistance.HasValue || tileDistance >= smallestDistance)
							continue;
						
						closestTilePosition = new Vector3Int(x, y, z);
						smallestDistance = tileDistance.Value;
					}
				}
			}
		
			// If we found in a chunk then we don't have to look at the rest of the chunks since the chunk list is sorted by closest
			if (closestTilePosition != null)
			{
				closestChunkMesh = chunkMesh;
				break;
			}
		}

		Vector3 hitPoint = ray.Position + ray.Direction * smallestDistance;
		hitPoint = WorldHelper.WorldPositionToTilePosition(hitPoint);

		if (closestTilePosition == null)
			return false;
		
		if (Math.Abs(hitPoint.X - closestTilePosition.Value.X) < .0001f)
		{
			adjacentTilePosition = closestTilePosition + Vector3Int.West;
		}
		else if (Math.Abs(hitPoint.X - closestTilePosition.Value.X - 1) < .0001f)
		{
			adjacentTilePosition = closestTilePosition + Vector3Int.East;
		}
		else if (Math.Abs(hitPoint.Y - closestTilePosition.Value.Y) < .0001f)
		{
			adjacentTilePosition = closestTilePosition + Vector3Int.Down;
		}
		else if (Math.Abs(hitPoint.Y - closestTilePosition.Value.Y - 1) < .0001f)
		{
			adjacentTilePosition = closestTilePosition + Vector3Int.Up;
		}
		else if (Math.Abs(hitPoint.Z - closestTilePosition.Value.Z) < .0001f)
		{
			adjacentTilePosition = closestTilePosition + Vector3Int.North;
		}
		else if (Math.Abs(hitPoint.Z - closestTilePosition.Value.Z - 1) < .0001f)
		{
			adjacentTilePosition = closestTilePosition + Vector3Int.South;
		}
		
		return adjacentTilePosition != null;
	}
}

public interface IWorldInteractionManager
{
	void UpdateTileHover();
	void CheckPrimaryInteraction();
	void CheckSecondaryInteraction();
}