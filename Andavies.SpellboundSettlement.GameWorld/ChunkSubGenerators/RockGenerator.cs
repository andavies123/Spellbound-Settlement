using Andavies.MonoGame.Utilities;
using Andavies.MonoGame.Utilities.Extensions;
using Andavies.SpellboundSettlement.GameWorld.Repositories;
using Andavies.SpellboundSettlement.GameWorld.Tiles;
using Serilog;

namespace Andavies.SpellboundSettlement.GameWorld.ChunkSubGenerators;

public class RockGenerator : ChunkSubGenerator
{
	public RockGenerator(
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
				Vector2Int tilePositionNoHeight = new(tileX, tileZ);
				int height = chunk.GetHeightAtPosition(tilePositionNoHeight);
				Vector3Int tilePosition = new(tileX, height + 1, tileZ);

				if (tilePosition.Y < chunk.ChunkData.TileCount.Y - 1 && chunk[tilePosition].TileId != nameof(AirTile))
					continue;

				float noise = ChunkNoiseGenerator.GenerateNoise(chunk.ChunkData, tilePositionNoHeight, seed, .5f);

				//Warning: Since I am currently using the GetThousandthsPlace method, the scale does not matter of the noise,
				bool addRock = noise.GetHundredthsPlace() == 5 && noise.GetThousandthsPlace() == 5;

				if (addRock)
				{
					if (TileRegistry.TryGetTile(nameof(SmallRockTile), out Tile? tile) && tile is ModelTile modelTile)
						SetModelTile(chunk, tilePosition, modelTile, noise);
				}
			}
		}
	}
}