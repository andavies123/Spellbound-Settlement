using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Andavies.MonoGame.Utilities;

namespace Andavies.SpellboundSettlement.Meshes;

public class WorldMesh
{
	private readonly ConcurrentDictionary<Vector2Int, ChunkMesh> _chunkMeshes = new();

	public IReadOnlyList<ChunkMesh> ChunkMeshes => _chunkMeshes.Values.ToList();
	
	public void SetChunkMesh(ChunkMesh chunkMesh, Vector2Int chunkPosition)
	{
		_chunkMeshes[chunkPosition] = chunkMesh;
	}

	public bool TryGetChunkMesh(Vector2Int chunkPosition, out ChunkMesh chunkMesh)
	{
		return _chunkMeshes.TryGetValue(chunkPosition, out chunkMesh);
	}
}