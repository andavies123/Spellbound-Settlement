using System;
using Andavies.MonoGame.Utilities;
using Andavies.SpellboundSettlement.GameWorld;
using Andavies.SpellboundSettlement.GameWorld.Repositories;
using Andavies.SpellboundSettlement.Repositories;
using Microsoft.Xna.Framework;
using Serilog;

namespace Andavies.SpellboundSettlement.Meshes;

public class ChunkMeshBuilder : IChunkMeshBuilder
{
	private readonly ILogger _logger;
	private readonly ITileRepository _tileRepository;

	public ChunkMeshBuilder(ILogger logger, ITileRepository tileRepository)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_tileRepository = tileRepository ?? throw new ArgumentNullException(nameof(tileRepository));
	}

	public ChunkMesh BuildChunkMesh(Chunk chunk)
	{
		ChunkMesh chunkMesh = new(chunk);

		for (int x = 0; x < chunk.TileCount.X; x++)
		{
			for (int y = 0; y < chunk.TileCount.Y; y++)
			{
				for (int z = 0; z < chunk.TileCount.Z; z++)
				{
					AddWorldTileToChunkMesh(chunk.WorldTiles[x, y, z], chunkMesh);
				}
			}
		}

		chunkMesh.TerrainMesh.RecalculateMesh();
		return chunkMesh;
	}

	private void AddWorldTileToChunkMesh(WorldTile worldTile, ChunkMesh chunkMesh)
	{
		if (!_tileRepository.TryGetTileDetails(worldTile.TileId, out ITileDetails tileDetails) || tileDetails == null)
		{
			_logger.Warning("Tile ID does not exist in tile repository. ID: {id}", worldTile.TileId);
			return;
		}

		switch (tileDetails)
		{
			case NonVisibleTileDetails nonVisibleTileDetails:
				// Currently we don't do anything for non visible tiles
				break;
			case TerrainTileDetails terrainTileDetails:
				HandleTerrainTileDetails(chunkMesh, worldTile, terrainTileDetails);
				break;
			case ModelTileDetails modelTileDetails:
				HandleModelTileDetails(chunkMesh, worldTile, modelTileDetails);
				break;
			default:
				_logger.Warning("Unable to draw tile with id: {tileId}", tileDetails.TileId);
				break;
		}
	}

	private static void HandleTerrainTileDetails(ChunkMesh chunkMesh, WorldTile worldTile, TerrainTileDetails terrainTileDetails)
	{
		Vector3Int tilePosition = worldTile.ParentChunkPosition.ToVector3IntNoY() * 10 + worldTile.TilePosition;
		CubeMesh cubeMesh = new((Vector3)tilePosition, WorldMeshConstants.HeightColors[worldTile.TilePosition.Y]);
			
		chunkMesh.SetTileMesh(worldTile.TilePosition, cubeMesh);
	}

	private static void HandleModelTileDetails(ChunkMesh chunkMesh, WorldTile worldTile, ModelTileDetails modelTileDetails)
	{
		chunkMesh.SetTileModel(worldTile.TilePosition, modelTileDetails, worldTile);
	}
}