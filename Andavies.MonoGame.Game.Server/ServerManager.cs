using Andavies.MonoGame.Game.Server.Interfaces;
using Andavies.MonoGame.Game.Server.Messages;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Andavies.MonoGame.Game.Server;

public class ServerManager : IServerManager
{
	private readonly NetManager _server;
	private readonly EventBasedNetListener _listener = new();
	private readonly NetPacketProcessor _packetProcessor = new();
	private int _maxUsersAllowed;
	private bool _isRunning;

	public ServerManager()
	{
		_server = new NetManager(_listener);
	}
	
	public void Start(int port, int maxUsersAllowed)
	{
		Console.WriteLine($"Server: Starting Server...\n" +
		                  $"\tPort: {port}");
		_maxUsersAllowed = maxUsersAllowed;
		_isRunning = true;
		_server.Start(port);
		
		_packetProcessor.RegisterNestedType(() => new WelcomePacket());

		_listener.ConnectionRequestEvent += OnConnectionRequest;
		_listener.PeerConnectedEvent += OnPeerConnected;
		_listener.PeerDisconnectedEvent += OnPeerDisconnected;
		_listener.NetworkReceiveEvent += OnNetworkReceived;

		Thread loopThread = new(GameLoop);
		loopThread.Start();
	}

	public void Stop()
	{
		_isRunning = false;
	}

	private void GameLoop()
	{
		while (_isRunning)
		{
			_server.PollEvents();
			UpdateGame();
			SendUpdatesToPeers();
		}
		
		EndServer();
	}

	private void UpdateGame()
	{
		
	}

	private void SendUpdatesToPeers()
	{
		foreach (NetPeer peer in _server.ConnectedPeerList)
		{
			// Send game updates
		}
	}

	private void EndServer()
	{
		Console.WriteLine("Server: Stopping server...");
		
		_listener.ConnectionRequestEvent -= OnConnectionRequest;
		_listener.PeerConnectedEvent -= OnPeerConnected;
		_listener.PeerDisconnectedEvent -= OnPeerDisconnected;
		_listener.NetworkReceiveEvent -= OnNetworkReceived;
		
		_server.Stop();
	}

	private void OnConnectionRequest(ConnectionRequest connectionRequest)
	{
		Console.WriteLine($"Server: Connection request received: {connectionRequest.RemoteEndPoint}");
		if (_server.ConnectedPeersCount < _maxUsersAllowed)
			connectionRequest.Accept();
		else
			connectionRequest.Reject();
	}

	private void OnPeerConnected(NetPeer client)
	{
		Console.WriteLine($"Server: New client connected: {client.EndPoint}");
		
		NetDataWriter writer = new();
		_packetProcessor.Write(writer, new WelcomePacket
		{
			WelcomeMessage = "Welcome to this Spellbound Settlement server"
		});
		
		client.Send(writer, DeliveryMethod.ReliableOrdered);
	}

	private void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
	{
		Console.WriteLine($"Server: Peer disconnected\n" +
		                  $"\tIP: {peer.EndPoint}\n" +
		                  $"\tReason: {disconnectInfo.Reason}");
	}

	private void OnNetworkReceived(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
	{
		Console.WriteLine($"Server: Received message from {peer.EndPoint}");
	}
}