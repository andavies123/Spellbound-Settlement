﻿using System.Collections.Concurrent;
using Andavies.MonoGame.Utilities;
using Microsoft.Xna.Framework;

namespace Andavies.SpellboundSettlement.GameWorld;

public class World
{
	private const int ChunkTileCount = 10;
	
	private readonly ConcurrentDictionary<Vector2, Chunk> _chunks = new();

	public IReadOnlyDictionary<Vector2, Chunk> Chunks => _chunks;

	public World((int x, int z) centerChunkPosition, int initialGenerationRadius)
	{
		int radius = initialGenerationRadius - 1;
		for (int x = centerChunkPosition.x - radius; x <= centerChunkPosition.x + radius; x++)
		{
			for (int z = centerChunkPosition.z - radius; z <= centerChunkPosition.z + radius; z++)
			{
				Chunk chunk = GenerateChunk(new Vector2(x, z));
				_chunks[chunk.ChunkPosition] = chunk;
			}
		}
	}

	public Chunk GetChunk(Vector2 chunkPosition)
	{
		return _chunks.GetOrAdd(chunkPosition, GenerateChunk);
	}

	private Chunk GenerateChunk(Vector2 chunkPosition)
	{
		Chunk chunk = new(chunkPosition, chunkPosition * ChunkTileCount, (ChunkTileCount, ChunkTileCount, ChunkTileCount));
		chunk.SetAllTiles(0);

		for (int x = 0; x < chunk.TileCount.x; x++)
		{
			for (int z = 0; z < chunk.TileCount.z; z++)
			{
				const int seed = 100;
				float noise = RandomUtility.GetPerlinNoise(seed, .5f, (
					chunkPosition.X + ((float)x / ChunkTileCount) + float.Epsilon,
					chunkPosition.Y + ((float)z / ChunkTileCount) + float.Epsilon));
				int height = GetHeightFromNoise(noise, 0, ChunkTileCount);

				float rockNoise = RandomUtility.GetPerlinNoise(seed, 1f, (
					chunkPosition.X + ((float)x / ChunkTileCount) + float.Epsilon,
					chunkPosition.Y + ((float)z / ChunkTileCount) + float.Epsilon));
				bool addRock = (int)((rockNoise + 1) * 1000) % 97 == 0; // 97 is an arbitrary prime number. Completely random
				
				for (int y = 0; y < chunk.TileCount.y; y++)
				{
					if (y <= height)
						chunk.Tiles[x, y, z] = 1;
					else if (y == height + 1 && addRock)
						chunk.Tiles[x, y, z] = 2;
					else
						chunk.Tiles[x, y, z] = 0;
				}
			}
		}
		
		return chunk;
	}
	
	private static int GetHeightFromNoise(float noise, int minHeight, int maxHeight) =>
		(int)((maxHeight - minHeight) * ((noise + 1)/2)) + minHeight;
}