using Andavies.MonoGame.Utilities;

namespace Andavies.SpellboundSettlement.GameWorld;

public class Chunk
{
	public Chunk(Vector2Int chunkPosition, Vector3Int tileCount)
	{
		ChunkPosition = chunkPosition;
		TileCount = tileCount;
		WorldTiles = new WorldTile[TileCount.X, TileCount.Y, TileCount.Z];
	}

	public Chunk(Vector2Int chunkPosition, WorldTile[,,] worldTiles)
	{
		ChunkPosition = chunkPosition;
		TileCount = new Vector3Int(worldTiles.GetLength(0), worldTiles.GetLength(1), worldTiles.GetLength(2));
		WorldTiles = worldTiles;
	}
	
	public Vector2Int ChunkPosition { get; }
	public Vector3Int TileCount { get; }
	public WorldTile[,,] WorldTiles { get; }
}