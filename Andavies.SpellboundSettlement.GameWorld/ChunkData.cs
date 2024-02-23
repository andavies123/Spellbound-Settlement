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
		set => SetAndFlagChanged(value, ref _worldTiles);
	}

	public void SetWorldTile(Vector3Int tileChunkPosition, WorldTile worldTile)
	{
		SetAndFlagChanged(worldTile, WorldTiles, tileChunkPosition);
	}
	
	// INetSerializable Implementation
	public void Serialize(NetDataWriter writer)
	{
		throw new NotImplementedException();
	}

	public void Deserialize(NetDataReader reader)
	{
		throw new NotImplementedException();
	}
}