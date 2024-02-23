using Andavies.MonoGame.Network.Extensions;
using Andavies.MonoGame.Utilities;
using LiteNetLib.Utils;

namespace Andavies.SpellboundSettlement.GameWorld;

public class ChunkData : ChangeTracker, INetSerializable
{
	private WorldTile[,,] _worldTiles = new WorldTile[0, 0, 0];
	
	public Vector2Int ChunkPosition { get; set; }
	public Vector3Int TileCount { get; set; }

	public WorldTile[,,] WorldTiles
	{
		get => _worldTiles;
		set => SetAndFlagChange(value, ref _worldTiles);
	}

	public void SetWorldTile(Vector3Int tileChunkPosition, WorldTile worldTile)
	{
		SetAndFlagChange(worldTile, WorldTiles, tileChunkPosition);
	}

	public void UpdateWorldTileId(Vector3Int tileChunkPosition, string id)
	{
		WorldTile worldTile = WorldTiles[tileChunkPosition.X, tileChunkPosition.Y, tileChunkPosition.Z];
		if (worldTile.TileId != id)
		{
			worldTile.TileId = id;
			IsChanged = true;
		}
	}
	
	// INetSerializable Implementation
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
					writer.Put(WorldTiles[x, y, z]);
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