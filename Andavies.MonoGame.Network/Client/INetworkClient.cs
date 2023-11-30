using LiteNetLib.Utils;

namespace Andavies.MonoGame.Network.Client;

public interface INetworkClient
{
	bool IsConnected { get; }
	
	void Start();
	void Update();
	void Stop();
	void TryConnect(string ipAddress, int port);
	void SendMessage<T>(T packet) where T : INetSerializable;
	void AddSubscription<T>(Action<INetSerializable> onReceivedCallback) where T : INetSerializable, new();
	void RemoveSubscription<T>(Action<INetSerializable> onReceivedCallback) where T : INetSerializable, new();
}