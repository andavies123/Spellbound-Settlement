using Andavies.SpellboundSettlement.GameWorld;
using LiteNetLib.Utils;

namespace Andavies.SpellboundSettlement.NetworkMessages.Messages.World;

/// <summary>
/// A packet from the server to client to supply world chunk information
/// </summary>
public class WorldChunkResponsePacket : INetSerializable
{
	/// <summary>
	/// Chunk data at the requested position
	/// </summary>
	public Chunk? Chunk { get; set; }
	
	public void Serialize(NetDataWriter writer)
	{
		Chunk?.Serialize(writer);
	}

	public void Deserialize(NetDataReader reader)
	{
		Chunk = new Chunk();
		Chunk.Deserialize(reader);
	}
}