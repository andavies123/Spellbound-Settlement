using Andavies.MonoGame.Utilities;
using Serilog;

namespace Andavies.SpellboundSettlement.GameWorld.ChunkSubGenerators;

public class TerrainGenerator : ChunkSubGenerator
{
	private readonly IChunkNoiseGenerator _chunkNoiseGenerator;
	
	public TerrainGenerator(ILogger logger, IChunkNoiseGenerator chunkNoiseGenerator)
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