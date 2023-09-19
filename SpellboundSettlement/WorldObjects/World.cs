using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SpellboundSettlement.Global;

namespace SpellboundSettlement.WorldObjects;

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
				float noise = GlobalRandom.GetPerlinNoise(100, 25, (
					chunkPosition.X + ((float)x / ChunkTileCount) + float.Epsilon,
					chunkPosition.Y + ((float)z / ChunkTileCount) + float.Epsilon));
				Console.WriteLine(noise);
				int height = GetHeightFromNoise(noise, 0, ChunkTileCount);
				
				for (int y = 0; y < chunk.TileCount.y; y++)
				{
					chunk.Tiles[x, y, z] = y <= height ? 1 : 0;
				}
			}
		}
		
		return chunk;
	}
	
	private static int GetHeightFromNoise(float noise, int minHeight, int maxHeight) =>
		(int)((maxHeight - minHeight) * ((noise + 1)/2)) + minHeight;
}