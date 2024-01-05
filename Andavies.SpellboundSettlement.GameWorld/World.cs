using System.Collections.Concurrent;
using Andavies.MonoGame.Utilities;

namespace Andavies.SpellboundSettlement.GameWorld;

public class World
{
	private const int ChunkTileCount = 10;
	
	private readonly ConcurrentDictionary<Vector2Int, Chunk> _chunks = new();

	public IReadOnlyDictionary<Vector2Int, Chunk> Chunks => _chunks;

	public World((int x, int z) centerChunkPosition, int initialGenerationRadius)
	{
		int radius = initialGenerationRadius - 1;
		for (int x = centerChunkPosition.x - radius; x <= centerChunkPosition.x + radius; x++)
		{
			for (int z = centerChunkPosition.z - radius; z <= centerChunkPosition.z + radius; z++)
			{
				Chunk chunk = GenerateChunk(new Vector2Int(x, z));
				_chunks[chunk.ChunkPosition] = chunk;
			}
		}
	}

	public Chunk GetChunk(Vector2Int chunkPosition)
	{
		return _chunks.GetOrAdd(chunkPosition, GenerateChunk);
	}

	private Chunk GenerateChunk(Vector2Int chunkPosition)
	{
		Chunk chunk = new(chunkPosition, new Vector3Int(ChunkTileCount));

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
				
				for (int y = 0; y < chunk.TileCount.X; y++)
				{
					Vector3Int tilePosition = new(x, y, z);
					if (y <= height)
						chunk.WorldTiles[x, y, z] = new WorldTile(1, chunkPosition, tilePosition);
					else if (y == height + 1 && addRock)
						chunk.WorldTiles[x, y, z] = new WorldTile(2, chunkPosition, tilePosition)
						{
							Rotation = GetRotationFromNoise(rockNoise)
						};
					else
						chunk.WorldTiles[x, y, z] = new WorldTile(0, chunkPosition, tilePosition);
				}
			}
		}
		
		return chunk;
	}
	
	private static int GetHeightFromNoise(float noise, int minHeight, int maxHeight) =>
		(int)((maxHeight - minHeight) * ((noise + 1)/2)) + minHeight;

	private static Rotation GetRotationFromNoise(float noise) =>
		(Rotation) ((int) ((noise + 1) * 1000) % 4);
}