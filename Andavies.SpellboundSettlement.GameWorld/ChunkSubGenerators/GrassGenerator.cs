using Andavies.MonoGame.Utilities;

namespace Andavies.SpellboundSettlement.GameWorld;

public class GrassGenerator : IChunkSubGenerator
{
	public void Generate(Chunk chunk, int seed)
	{
		
	}
}

public class TerrainGenerator : IChunkSubGenerator
{
	private readonly IChunkNoiseGenerator _chunkNoiseGenerator;
	
	public TerrainGenerator(IChunkNoiseGenerator chunkNoiseGenerator)
	{
		_chunkNoiseGenerator = chunkNoiseGenerator ?? throw new ArgumentNullException(nameof(chunkNoiseGenerator));
	}
	
	public void Generate(Chunk chunk, int seed)
	{
		for (int tileX = 0; tileX < chunk.ChunkData.TileCount.X; tileX++)
		{
			for (int tileZ = 0; tileZ < chunk.ChunkData.TileCount.Z; tileZ++)
			{
				float noise = _chunkNoiseGenerator.GenerateNoise(
					chunk.ChunkData.ChunkPosition, 
					new Vector2Int(tileX, tileZ), 
					seed, 
					.5f);
				
				int height = GetHeightFromNoise(noise, 0, WorldHelper.ChunkSize.Y);

				for (int tileY = 0; tileY < chunk.ChunkData.TileCount.Y; tileY++)
				{
					if (tileY <= height)
					{
						// Add ground
					}
					else
					{
						// Add Air
					}
				}
			}
		}
	}
	
	private static int GetHeightFromNoise(float noise, int minHeight, int maxHeight) =>
		(int)((maxHeight - minHeight) * ((noise + 1)/2)) + minHeight;
}

public interface IChunkSubGenerator
{
	void Generate(Chunk chunk, int seed);
}

public interface IChunkNoiseGenerator
{
	float GenerateNoise(Vector2Int chunkPosition, Vector3Int tileChunkPosition, int seed, float scale);
	float GenerateNoise(Vector2Int chunkPosition, Vector2Int tileChunkPositionNoHeight, int seed, float scale);
}