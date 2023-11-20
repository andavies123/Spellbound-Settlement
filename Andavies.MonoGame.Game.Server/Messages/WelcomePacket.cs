using LiteNetLib.Utils;

namespace Andavies.MonoGame.Game.Server.Messages;

/// <summary>
/// Message that is forwarded to a client when they first connect to the server
/// </summary>
public class WelcomePacket : INetSerializable
{
	public string WelcomeMessage { get; init; } = "Welcome!";
	
	public void Serialize(NetDataWriter writer)
	{
		writer.Put(WelcomeMessage);
	}

	public void Deserialize(NetDataReader reader)
	{
		reader.GetString();
	}

	public override string ToString()
	{
		return WelcomeMessage;
	}
}