using Andavies.MonoGame.Utilities;
using Andavies.SpellboundSettlement.GameWorld.ChunkSubGenerators;
using Andavies.SpellboundSettlement.GameWorld.Repositories;
using Serilog;

namespace Andavies.SpellboundSettlement.GameWorld;

public class WorldBuilder : IWorldBuilder
{
	private readonly ILogger _logger;

	private readonly IChunkSubGenerator _terrainGenerator;
	private readonly IChunkSubGenerator _rockGenerator;
	private readonly IChunkSubGenerator _grassGenerator;
	private readonly IChunkSubGenerator _bushGenerator;
	
	public WorldBuilder(ILogger logger, ITileRegistry tileRegistry, IChunkNoiseGenerator chunkNoiseGenerator)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		if (tileRegistry is null) throw new ArgumentNullException(nameof(tileRegistry));
		if (chunkNoiseGenerator is null) throw new ArgumentNullException(nameof(chunkNoiseGenerator));

		_terrainGenerator = new TerrainGenerator(_logger, tileRegistry, chunkNoiseGenerator);
		_rockGenerator = new RockGenerator(_logger, tileRegistry, chunkNoiseGenerator);
		_grassGenerator = new GrassGenerator(_logger, tileRegistry, chunkNoiseGenerator);
		_bushGenerator = new BushGenerator(_logger, tileRegistry, chunkNoiseGenerator);
	}
	
	public World BuildNewWorld(Vector2Int centerChunkPosition, int initialGenerationRadius)
	{
		int radius = initialGenerationRadius - 1;
		(int centerChunkX, int centerChunkZ) = centerChunkPosition;

		World world = new();
		
		for (int x = centerChunkX - radius; x <= centerChunkX + radius; x++)
		{
			for (int z = centerChunkZ - radius; z <= centerChunkZ + radius; z++)
			{
				Chunk chunk = GenerateChunk(new Vector2Int(x, z));

				if (!world.TryAddChunk(chunk))
					_logger.Warning("Unable to add chunk {chunkPos} to World", chunk.ChunkData.ChunkPosition);
			}
		}

		return world;
	}
    
	public Chunk GenerateChunk(Vector2Int chunkPosition)
	{
		_logger.Debug("Generating chunk {position}", chunkPosition);
		Chunk chunk = new()
		{
			ChunkData =
			{
				ChunkPosition = chunkPosition,
				TileCount = WorldHelper.ChunkSize,
				WorldTiles = new WorldTile[WorldHelper.ChunkSize.X, WorldHelper.ChunkSize.Y, WorldHelper.ChunkSize.Z]
			}
		};

		// Todo: Change this to be variable and not constant
		const int seed = 100;

		_terrainGenerator.Generate(chunk, seed);
		_rockGenerator.Generate(chunk, seed);
		_grassGenerator.Generate(chunk, seed);
		_bushGenerator.Generate(chunk, seed);
		
		return chunk;
	}
}

public interface IWorldBuilder
{
	Chunk GenerateChunk(Vector2Int chunkPosition);

	World BuildNewWorld(Vector2Int centerChunkPosition, int initialGenerationRadius);
}