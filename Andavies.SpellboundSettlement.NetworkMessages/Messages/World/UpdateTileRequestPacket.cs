using Andavies.MonoGame.Network.Extensions;
using Andavies.MonoGame.Utilities;
using LiteNetLib.Utils;

namespace Andavies.SpellboundSettlement.NetworkMessages.Messages.World;

public class UpdateTileRequestPacket : INetSerializable
{
	public Vector3Int WorldTilePosition { get; set; }
	public string TileId = string.Empty;
	
	public void Serialize(NetDataWriter writer)
	{
		writer.Put(WorldTilePosition);
		writer.Put(TileId);
	}

	public void Deserialize(NetDataReader reader)
	{
		WorldTilePosition = reader.GetVector3Int();
		TileId = reader.GetString();
	}
}