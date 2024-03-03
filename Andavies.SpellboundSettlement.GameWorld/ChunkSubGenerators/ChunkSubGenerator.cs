using Andavies.MonoGame.Utilities;
using Andavies.MonoGame.Utilities.Extensions;
using Andavies.SpellboundSettlement.GameWorld.Repositories;
using Andavies.SpellboundSettlement.GameWorld.Tiles;
using Microsoft.Xna.Framework;
using Serilog;

namespace Andavies.SpellboundSettlement.GameWorld.ChunkSubGenerators;

public abstract class ChunkSubGenerator : IChunkSubGenerator
{
	protected readonly ILogger Logger;
	protected readonly ITileRegistry TileRegistry;
	protected readonly IChunkNoiseGenerator ChunkNoiseGenerator;
	
	protected ChunkSubGenerator(ILogger logger, ITileRegistry tileRegistry, IChunkNoiseGenerator chunkNoiseGenerator)
	{
		Logger = logger ?? throw new ArgumentNullException(nameof(logger));
		TileRegistry = tileRegistry ?? throw new ArgumentNullException(nameof(tileRegistry));
		ChunkNoiseGenerator = chunkNoiseGenerator ?? throw new ArgumentNullException(nameof(chunkNoiseGenerator));
	}

	public abstract void Generate(Chunk chunk, int seed);
	
	protected void SetModelTile(Chunk chunk, Vector3Int tileChunkPosition, ModelTile? modelTile, float noise)
	{
		if (modelTile == null)
		{
			Logger.Warning($"Unable to set model tile {chunk.ChunkData.ChunkPosition} / {tileChunkPosition}. Model Tile is null");
			return;
		}
		
		float rotation = GetRotationFromNoise(noise);
		float scale = GetScaleFromNoise(noise, modelTile.MinGenerationScale, modelTile.MaxGenerationScale);
		SetTile(chunk, tileChunkPosition, modelTile, rotation: rotation, scale: scale);
	}

	protected void SetTile(Chunk chunk, Vector3Int tileChunkPosition, Tile? tile, float rotation = 0f, float scale = 1f)
	{
		if (tile == null)
		{
			Logger.Warning($"Unable to set tile {chunk.ChunkData.ChunkPosition} / {tileChunkPosition}. Tile is null");
			return;
		}
		
		chunk.ChunkData.SetWorldTile(tileChunkPosition, new WorldTile
		{
			TileId = tile.TileId,
			ParentChunkPosition = chunk.ChunkData.ChunkPosition,
			TilePosition = tileChunkPosition,
			Rotation = rotation,
			Scale = scale
		});
	}

	private static float GetRotationFromNoise(float noise) => (int) ((noise + 1) * 1000) % 4 * MathHelper.PiOver2;

	private static float GetScaleFromNoise(float noise, float minScale, float maxScale)
	{
		float scaleDifference = maxScale - minScale;
		return minScale + scaleDifference * noise.GetHundredthsPlace() / 10f;
	}
}