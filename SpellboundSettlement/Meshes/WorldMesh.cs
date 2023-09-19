using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SpellboundSettlement.WorldObjects;

namespace SpellboundSettlement.Meshes;

public class WorldMesh
{
	private readonly World _world;
	private readonly ConcurrentDictionary<Vector2, ChunkMesh> _chunkMeshes = new();
	
	public WorldMesh(World world)
	{
		_world = world;

		foreach (Chunk chunk in _world.Chunks.Values)
		{
			_chunkMeshes.TryAdd(chunk.ChunkPosition, GenerateChunkMesh(chunk));
		}
	}

	public IReadOnlyDictionary<Vector2, ChunkMesh> ChunkMeshes => _chunkMeshes;

	private ChunkMesh GenerateChunkMesh(Chunk chunk)
	{
		ChunkMesh chunkMesh = new(
			chunk.TileCount,
			new Vector3(chunk.WorldOffset.X, 0, chunk.WorldOffset.Y));
		return chunkMesh;
	}
}