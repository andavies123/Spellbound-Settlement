using LiteNetLib.Utils;

namespace Andavies.SpellboundSettlement.NetworkMessages.Messages.World;

/// <summary>
/// A packet from the server to client to supply tile details
/// </summary>
public class TileDetailsResponsePacket : INetSerializable
{
	/// <summary>
	/// Json that contains the details for all the tiles
	/// </summary>
	public string TileDetailsJson { get; set; } = string.Empty;
	
	public void Serialize(NetDataWriter writer)
	{
		writer.Put(TileDetailsJson);
	}

	public void Deserialize(NetDataReader reader)
	{
		TileDetailsJson = reader.GetString();
	}
}