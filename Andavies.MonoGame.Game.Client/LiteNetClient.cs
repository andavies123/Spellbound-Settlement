using LiteNetLib;

namespace Andavies.MonoGame.Game.Client;

public class LiteNetClient
{
	private readonly EventBasedNetListener _listener = new();
	private readonly NetManager _client;
	private NetPeer _server;
	private bool _isConnected = false;
	
	public LiteNetClient()
	{
		_client = new NetManager(_listener);
	}

	public void Start()
	{
		_listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod, channel) =>
		{
			Console.WriteLine($"We got: {dataReader.GetString(100)}");
			dataReader.Recycle();
		};
        
		_client.Start();
		_isConnected = TryConnect();
	}

	public void Stop()
	{
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
}