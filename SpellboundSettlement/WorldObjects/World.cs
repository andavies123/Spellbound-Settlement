using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpellboundSettlement.WorldObjects;

public class World
{
	private const int ChunkTilesXAndZ = 10;
	private const int ChunkTilesY = 20;
	
	private readonly ConcurrentDictionary<Vector2, Chunk> _chunks = new();

	public IReadOnlyDictionary<Vector2, Chunk> Chunks => _chunks;

	public Chunk GetChunk(Vector2 chunkPosition)
	{
		return _chunks.GetOrAdd(chunkPosition, GenerateChunk);
	}

	private Chunk GenerateChunk(Vector2 chunkPosition)
	{
		Chunk chunk = new(chunkPosition, chunkPosition * ChunkTilesXAndZ, (ChunkTilesXAndZ, ChunkTilesY, ChunkTilesXAndZ));
		chunk.SetAllTiles(0);
		return chunk;
	}
}