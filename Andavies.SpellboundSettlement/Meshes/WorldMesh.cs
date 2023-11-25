using System.Collections.Concurrent;
using System.Collections.Generic;
using Andavies.SpellboundSettlement.World;
using Microsoft.Xna.Framework;

namespace Andavies.SpellboundSettlement.Meshes;

public class WorldMesh
{
	private readonly ConcurrentDictionary<Vector2, ChunkMesh> _chunkMeshes = new();
	
	public IReadOnlyDictionary<Vector2, ChunkMesh> ChunkMeshes => _chunkMeshes;

	public void SetChunk(Chunk chunk)
	{
		_chunkMeshes.TryAdd(chunk.ChunkPosition, GenerateChunkMesh(chunk));
	}

	private ChunkMesh GenerateChunkMesh(Chunk chunk)
	{
		ChunkMesh chunkMesh = new(
			chunk,
			new Vector3(chunk.WorldOffset.X, 0, chunk.WorldOffset.Y));
		return chunkMesh;
	}
}