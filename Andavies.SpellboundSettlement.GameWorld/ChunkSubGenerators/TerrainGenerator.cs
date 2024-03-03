using Andavies.MonoGame.Utilities;
using Andavies.SpellboundSettlement.GameWorld.Repositories;
using Andavies.SpellboundSettlement.GameWorld.Tiles;
using Serilog;

namespace Andavies.SpellboundSettlement.GameWorld.ChunkSubGenerators;

public class TerrainGenerator : ChunkSubGenerator
{
	public TerrainGenerator(
		ILogger logger,
		ITileRegistry tileRegistry,
		IChunkNoiseGenerator chunkNoiseGenerator)
		: base(logger, tileRegistry, chunkNoiseGenerator) { }
	
	public override void Generate(Chunk chunk, int seed)
	{
		for (int tileX = 0; tileX < chunk.ChunkData.TileCount.X; tileX++)
		{
			for (int tileZ = 0; tileZ < chunk.ChunkData.TileCount.Z; tileZ++)
			{
				Vector2Int tileChunkPositionNoHeight = new(tileX, tileZ);
				
				float noise = ChunkNoiseGenerator.GenerateNoise(chunk.ChunkData, tileChunkPositionNoHeight, seed, .5f);
				int height = GetHeightFromNoise(noise, 0, chunk.ChunkData.TileCount.Y);

				for (int tileY = 0; tileY < chunk.ChunkData.TileCount.Y; tileY++)
				{
					Vector3Int tileChunkPosition = new(tileX, tileY, tileZ);
					if (tileY <= height)
					{
						if (TileRegistry.TryGetTile(nameof(GroundTile), out Tile? tile))
							SetTile(chunk, tileChunkPosition, tile);
						else
							Logger.Warning("Unable to add tile. {tileName} does not exist.", nameof(GroundTile));
					}
					else
					{
						if (TileRegistry.TryGetTile(nameof(AirTile), out Tile? tile))
							SetTile(chunk, tileChunkPosition, tile);
						else
							Logger.Warning("Unable to add tile. {tileName} does not exist.", nameof(AirTile));
					}
				}
			}
		}
	}
	
	private static int GetHeightFromNoise(float noise, int minHeight, int maxHeight) =>
		(int)((maxHeight - minHeight) * ((noise + 1)/2)) + minHeight;
}