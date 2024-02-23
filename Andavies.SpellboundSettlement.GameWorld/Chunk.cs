using Andavies.MonoGame.Utilities;
using Andavies.SpellboundSettlement.GameWorld.Tiles;

namespace Andavies.SpellboundSettlement.GameWorld;

public class Chunk
{
	private const int XDimension = 0;
	private const int YDimension = 1;
	private const int ZDimension = 2;

	public event Action<Chunk>? Updated;

	public WorldTile this[int x, int y, int z] => ChunkData.WorldTiles[x, y, z];
	public WorldTile this[Vector3Int index] => ChunkData.WorldTiles[index.X, index.Y, index.Z];

	public ChunkData ChunkData { get; } = new();

	public void Update()
	{
		if (!ChunkData.IsChanged) 
			return;
		
		ChunkData.AcceptChanges();
		Updated?.Invoke(this);
	}

	public void UpdateWorldTile(Vector3Int tilePosition, string newTileId)
	{
		ChunkData.UpdateWorldTileId(tilePosition, newTileId);
	}

	/// <summary>
	/// Returns the world height at a given (x, z) position
	/// </summary>
	/// <param name="position">(x, z) tile position in the chunk</param>
	/// <returns>The highest terrain at that column</returns>
	public int GetHeightAtPosition(Vector2Int position)
	{
		if (position.X < 0 || position.X >= ChunkData.WorldTiles.GetLength(XDimension) || position.Y < 0 || position.Y >= ChunkData.WorldTiles.GetLength(ZDimension))
			throw new ArgumentOutOfRangeException($"Unable to get height at position. {position}");
		
		for (int y = ChunkData.WorldTiles.GetLength(YDimension) - 1; y >= 0; y--)
		{
			if (ChunkData.WorldTiles[position.X, y, position.Y].TileId == nameof(GroundTile))
			{
				return y;
			}
		}

		return 0;
	}
}