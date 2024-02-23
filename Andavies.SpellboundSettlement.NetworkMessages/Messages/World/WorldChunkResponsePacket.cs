using Andavies.SpellboundSettlement.GameWorld;
using LiteNetLib.Utils;

namespace Andavies.SpellboundSettlement.NetworkMessages.Messages.World;

/// <summary>
/// A packet from the server to client to supply world chunk information
/// </summary>
public class WorldChunkResponsePacket : INetSerializable
{
	/// <summary>
	/// ChunkData data at the requested position
	/// </summary>
	public ChunkData? ChunkData { get; set; }
	
	public void Serialize(NetDataWriter writer)
	{
		ChunkData?.Serialize(writer);
	}

	public void Deserialize(NetDataReader reader)
	{
		ChunkData = new ChunkData();
		ChunkData.Deserialize(reader);
	}
}