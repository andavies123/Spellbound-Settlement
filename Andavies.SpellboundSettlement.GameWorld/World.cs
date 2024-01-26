﻿using System.Collections.Concurrent;
using Andavies.MonoGame.Utilities;
using Andavies.SpellboundSettlement.GameWorld.Repositories;
using Andavies.SpellboundSettlement.GameWorld.Tiles;
using Microsoft.Xna.Framework;
using Serilog;

namespace Andavies.SpellboundSettlement.GameWorld;

public class World
{
	private const int ChunkTileCount = 10;

	private readonly ILogger _logger;
	private readonly ITileRegistry _tileRegistry;
	
	private readonly ConcurrentDictionary<Vector2Int, Chunk> _chunks = new();

	public IReadOnlyDictionary<Vector2Int, Chunk> Chunks => _chunks;

	public World(ILogger logger, ITileRegistry tileRegistry)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_tileRegistry = tileRegistry ?? throw new ArgumentNullException(nameof(tileRegistry));
	}

	public void CreateNewWorld(Vector2Int centerChunkPosition, int initialGenerationRadius)
	{
		int radius = initialGenerationRadius - 1;
		for (int x = centerChunkPosition.X - radius; x <= centerChunkPosition.X + radius; x++)
		{
			for (int z = centerChunkPosition.Y - radius; z <= centerChunkPosition.Y + radius; z++)
			{
				Chunk chunk = GenerateChunk(new Vector2Int(x, z));
				_chunks[chunk.ChunkPosition] = chunk;
			}
		}
	}

	public void LoadWorld()
	{
		throw new NotImplementedException();
	}

	public Chunk GetChunk(Vector2Int chunkPosition)
	{
		return _chunks.GetOrAdd(chunkPosition, GenerateChunk);
	}

	public int GetHeightAtPosition(Vector3Int worldPosition)
	{
		(Vector2Int chunkPosition, Vector3Int tilePosition) = GetChunkPositionFromWorldPosition(worldPosition);

		Chunk chunk = _chunks.GetOrAdd(chunkPosition, GenerateChunk);
		return chunk.GetHeightAtPosition(new Vector2Int(tilePosition.X, tilePosition.Z));
	}

	private (Vector2Int chunkPosition, Vector3Int tilePosition) GetChunkPositionFromWorldPosition(Vector3Int worldPosition)
	{
		Vector2Int chunkPosition = new(worldPosition.X / 10, worldPosition.Z / 10);
		Vector3Int tilePosition = new(worldPosition.X % 10, worldPosition.Y % 10, worldPosition.Z % 10);

		return (chunkPosition, tilePosition);
	}

	private Chunk GenerateChunk(Vector2Int chunkPosition)
	{
		_logger.Debug("Generating chunk {position}", chunkPosition);
		Chunk chunk = new()
		{
			ChunkPosition = chunkPosition,
			TileCount = new Vector3Int(ChunkTileCount),
			WorldTiles = new WorldTile[ChunkTileCount, ChunkTileCount, ChunkTileCount]
		};

		for (int x = 0; x < chunk.TileCount.X; x++)
		{
			for (int z = 0; z < chunk.TileCount.Z; z++)
			{
				const int seed = 100;
				float noise = RandomUtility.GetPerlinNoise(seed, .5f, (
					chunkPosition.X + ((float)x / ChunkTileCount) + float.Epsilon,
					chunkPosition.Y + ((float)z / ChunkTileCount) + float.Epsilon));
				int height = GetHeightFromNoise(noise, 0, ChunkTileCount);

				float rockNoise = RandomUtility.GetPerlinNoise(seed, 1f, (
					chunkPosition.X + ((float)x / ChunkTileCount) + float.Epsilon,
					chunkPosition.Y + ((float)z / ChunkTileCount) + float.Epsilon));
				bool addRock = (int) ((rockNoise + 1) * 1000) % 97 == 0; // 97 is an arbitrary prime number. Completely random
				bool addGrass = GetThousandthsPlace(noise) < 2;
				bool addBush = GetThousandthsPlace(noise) == 3;
				
				for (int y = 0; y < chunk.TileCount.X; y++)
				{
					// Todo: Find way to not use hardcoded tileIds here
					Vector3Int tilePosition = new(x, y, z);
					if (y <= height)
					{
						if (!_tileRegistry.TryGetTile(nameof(GroundTile), out Tile? tile) || tile == null)
							continue;
						
						chunk.WorldTiles[x, y, z] = new WorldTile
						{
							TileId = tile.TileId,
							ParentChunkPosition = chunkPosition,
							TilePosition = tilePosition
						};
					}
					else if (y == height + 1 && addRock)
					{
						if (!_tileRegistry.TryGetTile(nameof(SmallRockTile), out Tile? tile) || tile is not ModelTile modelTile)
							continue;
						
						chunk.WorldTiles[x, y, z] = new WorldTile
						{
							TileId = tile.TileId,
							ParentChunkPosition = chunkPosition,
							TilePosition = tilePosition,
							Rotation = GetRotationFromNoise(rockNoise),
							Scale = GetScaleFromNoise(rockNoise, modelTile.MinGenerationScale, modelTile.MaxGenerationScale)
						};
					}
					else if (y == height + 1 && addGrass)
					{
						if (!_tileRegistry.TryGetTile(nameof(GrassTile), out Tile? tile) || tile is not ModelTile modelTile)
							continue;
                        
						chunk.WorldTiles[x, y, z] = new WorldTile
						{
							TileId = tile.TileId,
							ParentChunkPosition = chunkPosition,
							TilePosition = tilePosition,
							Rotation = GetRotationFromNoise(noise),
							Scale = GetScaleFromNoise(noise, modelTile.MinGenerationScale, modelTile.MaxGenerationScale)
						};
					}
					else if (y == height + 1 && addBush)
					{
						if (!_tileRegistry.TryGetTile(nameof(BushTile), out Tile? tile) || tile is not ModelTile modelTile)
							continue;
						
						chunk.WorldTiles[x, y, z] = new WorldTile
						{
							TileId = tile.TileId,
							ParentChunkPosition = chunkPosition,
							TilePosition = tilePosition,
							Rotation = GetRotationFromNoise(noise),
							Scale = GetScaleFromNoise(noise, modelTile.MinGenerationScale, modelTile.MaxGenerationScale)
						};
					}
					else
					{
						if (!_tileRegistry.TryGetTile(nameof(AirTile), out Tile? tile) || tile == null)
							continue;
						
						chunk.WorldTiles[x, y, z] = new WorldTile
						{
							TileId = tile.TileId,
							ParentChunkPosition = chunkPosition,
							TilePosition = tilePosition,
						};
					}
				}
			}
		}
		
		return chunk;
	}
	
	private static int GetHeightFromNoise(float noise, int minHeight, int maxHeight) =>
		(int)((maxHeight - minHeight) * ((noise + 1)/2)) + minHeight;

	private static float GetRotationFromNoise(float noise) => (int) ((noise + 1) * 1000) % 4 * MathHelper.PiOver2;

	private static float GetScaleFromNoise(float noise, float minScale, float maxScale)
	{
		float scaleDifference = maxScale - minScale;
		return minScale + scaleDifference * GetHundredthsPlace(noise) / 10f;
	}

	private static int GetHundredthsPlace(float value) => (int) Math.Abs(value * 100 % 10);
	private static int GetThousandthsPlace(float value) => (int) Math.Abs(value * 1000 % 10);
}