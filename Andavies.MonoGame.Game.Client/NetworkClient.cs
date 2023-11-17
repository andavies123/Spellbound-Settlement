using Andavies.MonoGame.Game.Server.Messages;
using LiteNetLib;

namespace Andavies.MonoGame.Game.Client;

public class NetworkClient : INetworkClient
{
	private const int ConnectTimeoutMSec = 5000;
	
	private readonly EventBasedNetListener _listener = new();
	private readonly NetManager _client;
	private NetPeer? _server;
	
	public NetworkClient()
	{
		_client = new NetManager(_listener);
	}

	public bool IsConnected => _server?.ConnectionState == ConnectionState.Connected;
	
	public void Start()
	{
		_listener.NetworkReceiveEvent += OnNetworkReceived;
		
		_client.Start();
	}

	public void Update()
	{
		_client.PollEvents();
	}

	public void Stop()
	{
		_listener.NetworkReceiveEvent -= OnNetworkReceived;
		
		_server?.Disconnect();
		_client.Stop();
	}

	public void TryConnect()
	{
		Console.WriteLine("Client: Attempting to connect to server");
		_server = _client.Connect("localhost", 9580, "test key");

		DateTime start = DateTime.Now;
		DateTime now = DateTime.Now;
		
		while (!IsConnected || now.Subtract(start).Milliseconds >= ConnectTimeoutMSec)
		{
			Thread.Sleep(100);
			now = DateTime.Now;
		}
        
		if (!IsConnected)
		{
			Console.WriteLine("Client: Could not connect to the server");
			return;
		}

		Console.WriteLine($"Client: Connected to server: {_server.EndPoint}");
	}
	
	private void OnNetworkReceived(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
	{
		var message = new WelcomePacket();
		message.Deserialize(reader);
		Console.WriteLine($"Client: Received message\n" +
		                  $"\t{message.WelcomeMessage}");
		reader.Recycle();
	}
}