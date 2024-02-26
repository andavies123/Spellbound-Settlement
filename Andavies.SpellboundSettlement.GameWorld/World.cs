using System.Collections.Concurrent;
using Andavies.MonoGame.Utilities;

namespace Andavies.SpellboundSettlement.GameWorld;

public class World
{
	private readonly ConcurrentDictionary<Vector2Int, Chunk> _chunks = new();

	public event Action<Chunk>? ChunkUpdated;

	public void Update()
	{
		foreach (Chunk chunk in _chunks.Values)
		{
			chunk.Update();
		}
	}

	public bool TryGetChunk(Vector2Int chunkPosition, out Chunk? chunk)
	{
		return _chunks.TryGetValue(chunkPosition, out chunk);
	}

	public bool TryAddChunk(Chunk chunk)
	{
		if (!_chunks.TryAdd(chunk.ChunkData.ChunkPosition, chunk))
			return false;

		chunk.Updated += OnChunkUpdated;
		return true;
	}

	public bool TryRemoveChunk(Vector2Int chunkPosition)
	{
		if (!_chunks.TryRemove(chunkPosition, out Chunk? chunk))
			return false;

		chunk.Updated -= OnChunkUpdated;
		return true;
	}

	private void OnChunkUpdated(Chunk chunk) => ChunkUpdated?.Invoke(chunk);
}