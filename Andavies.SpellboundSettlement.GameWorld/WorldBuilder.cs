using Andavies.MonoGame.Utilities;
using Andavies.MonoGame.Utilities.Extensions;
using Andavies.SpellboundSettlement.GameWorld.Repositories;
using Andavies.SpellboundSettlement.GameWorld.Tiles;
using Microsoft.Xna.Framework;
using Serilog;

namespace Andavies.SpellboundSettlement.GameWorld;

public class WorldBuilder : IWorldBuilder
{
	private readonly ILogger _logger;
	private readonly ITileRegistry _tileRegistry;
	
	public WorldBuilder(ILogger logger, ITileRegistry tileRegistry)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_tileRegistry = tileRegistry ?? throw new ArgumentNullException(nameof(tileRegistry));
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

		for (int x = 0; x < chunk.ChunkData.TileCount.X; x++)
		{
			for (int z = 0; z < chunk.ChunkData.TileCount.Z; z++)
			{
				const int seed = 100;
				float noise = RandomUtility.GetPerlinNoise(seed, .5f, (
					chunkPosition.X + ((float)x / WorldHelper.ChunkSize.X) + float.Epsilon,
					chunkPosition.Y + ((float)z / WorldHelper.ChunkSize.Z) + float.Epsilon));
				int height = GetHeightFromNoise(noise, 0, WorldHelper.ChunkSize.Y);

				float rockNoise = RandomUtility.GetPerlinNoise(seed, 1f, (
					chunkPosition.X + ((float)x / WorldHelper.ChunkSize.X) + float.Epsilon,
					chunkPosition.Y + ((float)z / WorldHelper.ChunkSize.Z) + float.Epsilon));
				bool addRock = (int) ((rockNoise + 1) * 1000) % 97 == 0; // 97 is an arbitrary prime number. Completely random
				bool addGrass = noise.GetThousandthsPlace() < 2;
				bool addBush = noise.GetThousandthsPlace() == 3;
				
				for (int y = 0; y < chunk.ChunkData.TileCount.X; y++)
				{
					// Todo: Find way to not use hardcoded tileIds here
					Vector3Int tileChunkPosition = new(x, y, z);
					if (y <= height)
					{
						if (!_tileRegistry.TryGetTile(nameof(GroundTile), out Tile? tile) || tile == null)
							continue;
						
						chunk.ChunkData.SetWorldTile(tileChunkPosition, new WorldTile
						{
							TileId = tile.TileId,
							ParentChunkPosition = chunkPosition,
							TilePosition = tileChunkPosition
						});
					}
					else if (y == height + 1 && addRock)
					{
						if (!_tileRegistry.TryGetTile(nameof(SmallRockTile), out Tile? tile) || tile is not ModelTile modelTile)
							continue;
						
						chunk.ChunkData.SetWorldTile(tileChunkPosition, new WorldTile
						{
							TileId = tile.TileId,
							ParentChunkPosition = chunkPosition,
							TilePosition = tileChunkPosition,
							Rotation = GetRotationFromNoise(rockNoise),
							Scale = GetScaleFromNoise(rockNoise, modelTile.MinGenerationScale, modelTile.MaxGenerationScale)
						});
					}
					else if (y == height + 1 && addGrass)
					{
						if (!_tileRegistry.TryGetTile(nameof(GrassTile), out Tile? tile) || tile is not ModelTile modelTile)
							continue;
                        
						chunk.ChunkData.SetWorldTile(tileChunkPosition, new WorldTile
						{
							TileId = tile.TileId,
							ParentChunkPosition = chunkPosition,
							TilePosition = tileChunkPosition,
							Rotation = GetRotationFromNoise(noise),
							Scale = GetScaleFromNoise(noise, modelTile.MinGenerationScale, modelTile.MaxGenerationScale)
						});
					}
					else if (y == height + 1 && addBush)
					{
						if (!_tileRegistry.TryGetTile(nameof(BushTile), out Tile? tile) || tile is not ModelTile modelTile)
							continue;
						
						chunk.ChunkData.SetWorldTile(tileChunkPosition, new WorldTile
						{
							TileId = tile.TileId,
							ParentChunkPosition = chunkPosition,
							TilePosition = tileChunkPosition,
							Rotation = GetRotationFromNoise(noise),
							Scale = GetScaleFromNoise(noise, modelTile.MinGenerationScale, modelTile.MaxGenerationScale)
						});
					}
					else
					{
						if (!_tileRegistry.TryGetTile(nameof(AirTile), out Tile? tile) || tile == null)
							continue;
						
						chunk.ChunkData.SetWorldTile(tileChunkPosition, new WorldTile
						{
							TileId = tile.TileId,
							ParentChunkPosition = chunkPosition,
							TilePosition = tileChunkPosition,
						});
					}
				}
			}
		}
		
		return chunk;
	}
	
	private static int GetHeightFromNoise(float noise, int minHeight, int maxHeight) =>
		(int)((maxHeight - minHeight) * ((noise + 1)/2)) + minHeight;

	private static float GetRotationFromNoise(float noise) => (int) ((noise + 1) * 1000) % 4 * MathHelper.PiOver2;

	private static float GetScaleFromNoise(float noise, float minScale, float maxScale)
	{
		float scaleDifference = maxScale - minScale;
		return minScale + scaleDifference * noise.GetHundredthsPlace() / 10f;
	}
}

public interface IWorldBuilder
{
	Chunk GenerateChunk(Vector2Int chunkPosition);

	World BuildNewWorld(Vector2Int centerChunkPosition, int initialGenerationRadius);
}