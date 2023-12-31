using LiteNetLib.Utils;
using Andavies.MonoGame.Network.Extensions;
using Andavies.MonoGame.Utilities;

namespace Andavies.SpellboundSettlement.NetworkMessages.Messages.World;

/// <summary>
/// A packet from the client to server to request world chunk information
/// </summary>
public class WorldChunkRequestPacket : INetSerializable
{
	/// <summary>
	/// Collection of chunk positions that the client is requesting data for
	/// </summary>
	public List<Vector2Int> ChunkPositions { get; set; } = new();
	
	public void Serialize(NetDataWriter writer)
	{
		writer.Put(ChunkPositions);
	}

	public void Deserialize(NetDataReader reader)
	{
		ChunkPositions = reader.GetVector2IntList();
	}
}