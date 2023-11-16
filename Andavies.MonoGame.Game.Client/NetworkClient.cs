using LiteNetLib;

namespace Andavies.MonoGame.Game.Client;

public interface INetworkClient
{
	void Start();
	void Stop();
}

public class NetworkClient
{
	private readonly EventBasedNetListener _listener = new();
	private readonly NetManager _client;
	private NetPeer? _server;
	private bool _isConnected = false;
	
	public NetworkClient()
	{
		_client = new NetManager(_listener);
	}
	
	public void Start()
	{
		_listener.NetworkReceiveEvent += OnNetworkReceived;
		
		_client.Start();
		_isConnected = TryConnect();
	}

	public void Stop()
	{
		_listener.NetworkReceiveEvent -= OnNetworkReceived;
		
		_server.Disconnect();
		_client.Stop();
		_isConnected = false;
	}

	private bool TryConnect()
	{
		_server = _client.Connect("localhost", 9050, "Sample Key");

		if (_server == null)
			return false;
		
		Console.WriteLine($"Connected to server - ID: {_server.Id}");
		return true;
	}
	
	private void OnNetworkReceived(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
	{
		Console.WriteLine($"Received: {reader.GetString(100)}");
		reader.Recycle();
	}
}