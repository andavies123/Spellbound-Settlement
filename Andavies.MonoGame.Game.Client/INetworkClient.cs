using LiteNetLib.Utils;

namespace Andavies.MonoGame.Game.Client;

public interface INetworkClient
{
	bool IsConnected { get; }
	
	void Start();
	void Update();
	void Stop();
	void TryConnect();
	void SendMessage<T>(T packet) where T : INetSerializable;
}