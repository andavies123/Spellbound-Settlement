using Andavies.MonoGame.Utilities;

namespace Andavies.SpellboundSettlement.GameWorld.ChunkSubGenerators;

public interface IChunkNoiseGenerator
{
	float GenerateNoise(Vector2Int chunkPosition, Vector3Int tileChunkPosition, int seed, float scale);
	float GenerateNoise(Vector2Int chunkPosition, Vector2Int tileChunkPositionNoHeight, int seed, float scale);
}