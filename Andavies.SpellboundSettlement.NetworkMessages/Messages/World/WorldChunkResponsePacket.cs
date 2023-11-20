using LiteNetLib.Utils;
using Microsoft.Xna.Framework;

namespace Andavies.SpellboundSettlement.NetworkMessages.Messages.World;

/// <summary>
/// A packet from the server to client to supply world chunk information
/// </summary>
public class WorldChunkResponsePacket : INetSerializable
{
	/// <summary>
	/// The position of the chunk this data belongs to
	/// </summary>
	public Vector2 ChunkPosition { get; set; }
	
	public void Serialize(NetDataWriter writer)
	{
		writer.Put(ChunkPosition.X);
		writer.Put(ChunkPosition.Y);
	}

	public void Deserialize(NetDataReader reader)
	{
		float x = reader.GetFloat();
		float y = reader.GetFloat();
		ChunkPosition = new Vector2(x, y);
	}

	public override string ToString()
	{
		return $"{nameof(ChunkPosition)}: {ChunkPosition}";
	}
}