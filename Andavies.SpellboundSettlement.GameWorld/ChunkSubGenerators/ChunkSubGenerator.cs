using Andavies.MonoGame.Utilities;
using Andavies.SpellboundSettlement.GameWorld.Tiles;
using Serilog;

namespace Andavies.SpellboundSettlement.GameWorld;

public abstract class ChunkSubGenerator
{
	protected readonly ILogger Logger;
	
	protected ChunkSubGenerator(ILogger logger)
	{
		Logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}
	
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
}