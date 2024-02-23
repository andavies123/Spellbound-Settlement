using Andavies.MonoGame.Utilities;
using Andavies.SpellboundSettlement.GameWorld.Tiles;

namespace Andavies.SpellboundSettlement.GameWorld;

public static class ChunkHelper
{
	public const int XDimension = 0;
	public const int YDimension = 1;
	public const int ZDimension = 2;
	
	/// <summary>
	/// Returns the world height at a given (x, z) tile position
	/// </summary>
	/// <param name="chunk">The extended chunk</param>
	/// <param name="positionNoHeight">(x, z) tile position in the chunk</param>
	/// <returns>The highest terrain in the chunk column</returns>
	/// <exception cref="IndexOutOfRangeException">Thrown if <paramref name="positionNoHeight"/> has invalid coordinates</exception>
	public static int GetHeightAtPosition(this Chunk chunk, Vector2Int positionNoHeight)
	{
		if (positionNoHeight.X < 0 || positionNoHeight.X >= chunk.ChunkData.WorldTiles.GetLength(XDimension) || 
		    positionNoHeight.Y < 0 || positionNoHeight.Y >= chunk.ChunkData.WorldTiles.GetLength(ZDimension))
			throw new IndexOutOfRangeException($"Unable to get height at position. {positionNoHeight}");
		
		for (int y = chunk.ChunkData.WorldTiles.GetLength(YDimension) - 1; y >= 0; y--)
		{
			if (chunk.ChunkData.WorldTiles[positionNoHeight.X, y, positionNoHeight.Y].TileId == nameof(GroundTile))
			{
				return y;
			}
		}

		return 0;
	}
}