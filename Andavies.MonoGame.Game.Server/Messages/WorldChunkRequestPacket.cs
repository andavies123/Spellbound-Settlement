using LiteNetLib.Utils;

namespace Andavies.MonoGame.Game.Server.Messages;

/// <summary>
/// A packet from the client to server to request world chunk information
/// </summary>
public class WorldChunkRequestPacket : INetSerializable
{
	public void Serialize(NetDataWriter writer)
	{
		
	}

	public void Deserialize(NetDataReader reader)
	{
		
	}
}