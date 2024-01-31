using System;
using System.Collections.Generic;
using System.Linq;
using Andavies.MonoGame.Inputs;
using Andavies.MonoGame.Utilities;
using Andavies.MonoGame.Utilities.Extensions;
using Andavies.SpellboundSettlement.CameraObjects;
using Andavies.SpellboundSettlement.Meshes;
using Microsoft.Xna.Framework;

namespace Andavies.SpellboundSettlement.GameStates;

public class TileMouseHoverHandler : ITileHoverHandler
{
	private readonly IInputManager _inputManager;
	private readonly Camera _camera;
	
	private ChunkMesh _hoveredChunk = null;
	private Vector3Int? _hoveredTile = null;

	public TileMouseHoverHandler(IInputManager inputManager, Camera camera)
	{
		_inputManager = inputManager ?? throw new ArgumentNullException(nameof(inputManager));
		_camera = camera ?? throw new ArgumentNullException(nameof(camera));
	}

	public void UpdateHover(WorldMesh worldMesh)
	{
		// Clear at beginning so it doesn't get redrawn
		ClearHoveredTile();
		
		// Distance doesn't matter as we want it to go forever
		Ray ray = _camera.GetRayFromCamera(GameManager.Viewport, _inputManager.CurrentMousePosition.ToVector2(), 1);
		
		// Find all chunks that intersect with ray cast
		List<(ChunkMesh, float)> chunkMeshesByDistance = new();
		foreach (ChunkMesh chunkMesh in worldMesh.ChunkMeshes)
		{
			float? chunkDistance = ray.Intersects(chunkMesh.ChunkMeshCollider.ChunkCollider.Collider);
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
		Vector3Int? closestTile = null;
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
						
						closestTile = new Vector3Int(x, y, z);
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

	private void SetHoveredTile(ChunkMesh chunkMesh, Vector3Int tilePosition)
	{
		if (chunkMesh == null)
			return;

		_hoveredChunk = chunkMesh;
		_hoveredTile = tilePosition;
		
		const float colorIncrease = .2f;
		Vector3 tileColorValues = WorldMeshConstants.HeightColors[_hoveredTile.Value.Y].ToVector3();
		Color hoveredColor = new(tileColorValues - new Vector3(colorIncrease));
		chunkMesh.TerrainMesh.SetTileColor(_hoveredTile.Value, hoveredColor);
	}
	
	private void ClearHoveredTile()
	{
		if (_hoveredChunk != null && _hoveredTile != null)
			_hoveredChunk.TerrainMesh.SetTileColor(_hoveredTile.Value, WorldMeshConstants.HeightColors[_hoveredTile.Value.Y]);

		_hoveredChunk = null;
		_hoveredTile = null;
	}
}