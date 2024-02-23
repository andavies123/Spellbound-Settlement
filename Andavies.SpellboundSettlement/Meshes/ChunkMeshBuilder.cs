using System;
using Andavies.MonoGame.Utilities;
using Andavies.SpellboundSettlement.GameWorld;
using Andavies.SpellboundSettlement.GameWorld.Repositories;
using Andavies.SpellboundSettlement.GameWorld.Tiles;
using Microsoft.Xna.Framework;
using Serilog;

namespace Andavies.SpellboundSettlement.Meshes;

public class ChunkMeshBuilder : IChunkMeshBuilder
{
	private readonly ILogger _logger;
	private readonly ITileRegistry _tileRegistry;

	public ChunkMeshBuilder(ILogger logger, ITileRegistry tileRegistry)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_tileRegistry = tileRegistry ?? throw new ArgumentNullException(nameof(tileRegistry));
	}

	public ChunkMesh BuildChunkMesh(ChunkData chunkData)
	{
		ChunkMesh chunkMesh = new(chunkData);

		for (int x = 0; x < chunkData.TileCount.X; x++)
		{
			for (int y = 0; y < chunkData.TileCount.Y; y++)
			{
				for (int z = 0; z < chunkData.TileCount.Z; z++)
				{
					AddWorldTileToChunkMesh(chunkData.WorldTiles[x, y, z], chunkMesh);
				}
			}
		}

		chunkMesh.TerrainMesh.RecalculateMesh();
		return chunkMesh;
	}

	private void AddWorldTileToChunkMesh(WorldTile worldTile, ChunkMesh chunkMesh)
	{
		if (!_tileRegistry.TryGetTile(worldTile.TileId, out Tile tile) || tile == null)
		{
			_logger.Warning("Tile ID does not exist in tile repository. ID: {id}", worldTile.TileId);
			return;
		}

		switch (tile)
		{
			case AirTile:
				// Currently we don't do anything for non visible tiles
				break;
			case TerrainTile terrainTile:
				HandleTerrainTileDetails(chunkMesh, worldTile, terrainTile);
				break;
			case ModelTile modelTile:
				HandleModelTileDetails(chunkMesh, worldTile, modelTile);
				break;
			default:
				_logger.Warning("Unable to draw tile: {tile}", tile.DisplayName);
				break;
		}
	}

	private static void HandleTerrainTileDetails(ChunkMesh chunkMesh, WorldTile worldTile, TerrainTile terrainTile)
	{
		Vector3Int tilePosition = worldTile.ParentChunkPosition.ToVector3IntNoY() * 10 + worldTile.TilePosition;
		CubeMesh cubeMesh = new((Vector3)tilePosition, WorldMeshConstants.HeightColors[worldTile.TilePosition.Y]);
			
		chunkMesh.SetTileMesh(worldTile.TilePosition, cubeMesh);
	}

	private static void HandleModelTileDetails(ChunkMesh chunkMesh, WorldTile worldTile, ModelTile modelTile)
	{
		chunkMesh.SetTileModel(worldTile.TilePosition, modelTile, worldTile);
	}
}