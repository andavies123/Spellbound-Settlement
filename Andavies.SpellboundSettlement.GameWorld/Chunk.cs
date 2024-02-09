using Andavies.MonoGame.Network.Extensions;
using Andavies.MonoGame.Utilities;
using Andavies.SpellboundSettlement.GameWorld.Tiles;
using LiteNetLib.Utils;

namespace Andavies.SpellboundSettlement.GameWorld;

public class Chunk : INetSerializable
{
	private const int XDimension = 0;
	private const int YDimension = 1;
	private const int ZDimension = 2;

	public event Action<Chunk>? Updated;
	
	public Vector2Int ChunkPosition { get; set; }
	public Vector3Int TileCount { get; set; }
	public WorldTile[,,] WorldTiles { get; set; } = new WorldTile[0, 0, 0];

	public WorldTile GetWorldTile(Vector3Int tilePosition)
	{
		return WorldTiles[tilePosition.X, tilePosition.Y, tilePosition.Z];
	}

	public void UpdateWorldTile(Vector3Int tilePosition, string newTileId)
	{
		WorldTile worldTile = GetWorldTile(tilePosition);
		if (worldTile.TileId != newTileId)
		{
			worldTile.TileId = newTileId;
			Updated?.Invoke(this);
		}
	}

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

	public void Serialize(NetDataWriter writer)
	{
		writer.Put(ChunkPosition);
		writer.Put(TileCount);

		for (int x = 0; x < WorldTiles.GetLength(0); x++)
		{
			for (int y = 0; y < WorldTiles.GetLength(1); y++)
			{
				for (int z = 0; z < WorldTiles.GetLength(2); z++)
				{
					WorldTiles[x, y, z].Serialize(writer);
				}
			}
		}
	}

	public void Deserialize(NetDataReader reader)
	{
		ChunkPosition = reader.GetVector2Int();
		TileCount = reader.GetVector3Int();

		WorldTiles = new WorldTile[TileCount.X, TileCount.Y, TileCount.Z];
		
		for (int x = 0; x < WorldTiles.GetLength(0); x++)
		{
			for (int y = 0; y < WorldTiles.GetLength(1); y++)
			{
				for (int z = 0; z < WorldTiles.GetLength(2); z++)
				{
					WorldTiles[x, y, z] = new WorldTile();
					WorldTiles[x, y, z].Deserialize(reader);
				}
			}
		}
	}
}