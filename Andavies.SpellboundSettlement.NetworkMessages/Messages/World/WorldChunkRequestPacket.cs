using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
using Andavies.MonoGame.NetworkUtilities.Extensions;

namespace Andavies.SpellboundSettlement.NetworkMessages.Messages.World;

/// <summary>
/// A packet from the client to server to request world chunk information
/// </summary>
public class WorldChunkRequestPacket : INetSerializable
{
	/// <summary>
	/// The position of the chunk data that is being requested
	/// </summary>
	public Vector2 ChunkPosition { get; set; }
	
	public void Serialize(NetDataWriter writer)
	{
		writer.Put(ChunkPosition);
	}

	public void Deserialize(NetDataReader reader)
	{
		ChunkPosition = reader.GetVector2();
	}

	public override string ToString()
	{
		return $"{nameof(ChunkPosition)}: {ChunkPosition}";
	}
}