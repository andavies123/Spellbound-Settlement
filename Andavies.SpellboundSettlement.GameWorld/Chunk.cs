using Andavies.MonoGame.Utilities;
using Andavies.SpellboundSettlement.GameWorld.Tiles;

namespace Andavies.SpellboundSettlement.GameWorld;

public class Chunk
{
	private const int XDimension = 0;
	private const int YDimension = 1;
	private const int ZDimension = 2;
    
	public Chunk(Vector2Int chunkPosition, Vector3Int tileCount)
	{
		ChunkPosition = chunkPosition;
		TileCount = tileCount;
		WorldTiles = new WorldTile[TileCount.X, TileCount.Y, TileCount.Z];
	}

	public Chunk(Vector2Int chunkPosition, WorldTile[,,] worldTiles)
	{
		ChunkPosition = chunkPosition;
		TileCount = new Vector3Int(worldTiles.GetLength(XDimension), worldTiles.GetLength(YDimension), worldTiles.GetLength(ZDimension));
		WorldTiles = worldTiles;
	}
	
	public Vector2Int ChunkPosition { get; }
	public Vector3Int TileCount { get; }
	public WorldTile[,,] WorldTiles { get; }

	/// <summary>
	/// Returns the world height at a given (x, z) position
	/// </summary>
	/// <param name="position">(x, z) tile position in the chunk</param>
	/// <returns>The highest terrain at that column</returns>
	public int GetHeightAtPosition(Vector2Int position)
	{
		if (position.X < 0 || position.X >= WorldTiles.GetLength(XDimension) || position.Y < 0 || position.Y >= WorldTiles.GetLength(ZDimension))
			throw new ArgumentOutOfRangeException($"Unable to get height at position. {position}");
		
		for (int y = WorldTiles.GetLength(YDimension) - 1; y >= 0; y--)
		{
			if (WorldTiles[position.X, y, position.Y].TileId == nameof(GroundTile))
			{
				return y;
			}
		}

		return 0;
	}
}