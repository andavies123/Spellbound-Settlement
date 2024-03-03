using Andavies.MonoGame.Utilities;

namespace Andavies.SpellboundSettlement.GameWorld.ChunkSubGenerators;

public interface IChunkNoiseGenerator
{
	float GenerateNoise(ChunkData chunkData, Vector3Int tileChunkPosition, int seed, float scale);
	float GenerateNoise(ChunkData chunkData, Vector2Int tileChunkPositionNoHeight, int seed, float scale);
}

public class ChunkNoiseGenerator : IChunkNoiseGenerator
{
	public float GenerateNoise(ChunkData chunkData, Vector3Int tileChunkPosition, int seed, float scale)
	{
		return GenerateNoise(chunkData, new Vector2Int(tileChunkPosition.X, tileChunkPosition.Z), seed, scale);
	}

	public float GenerateNoise(ChunkData chunkData, Vector2Int tileChunkPositionNoHeight, int seed, float scale)
	{
		return RandomUtility.GetPerlinNoise(seed, scale, (
			chunkData.ChunkPosition.X + (float)tileChunkPositionNoHeight.X / chunkData.TileCount.X + float.Epsilon,
			chunkData.ChunkPosition.Y + (float)tileChunkPositionNoHeight.Y / chunkData.TileCount.Z + float.Epsilon));
	}
}