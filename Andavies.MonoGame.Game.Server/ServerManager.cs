using Andavies.MonoGame.Game.Server.Interfaces;
using LiteNetLib;

namespace Andavies.MonoGame.Game.Server;

public class ServerManager : IServerManager
{
	private readonly NetManager _server;
	private readonly EventBasedNetListener _listener = new();
	private int _maxUsersAllowed;
	private bool _isRunning = false;

	public ServerManager()
	{
		_server = new NetManager(_listener);
	}
	
	public void Start(int port, int maxUsersAllowed)
	{
		Console.WriteLine($"Starting Server on port {port}");
		_maxUsersAllowed = maxUsersAllowed;
		_isRunning = true;
		_server.Start(port);

		_listener.ConnectionRequestEvent += OnConnectionRequest;
		_listener.PeerConnectedEvent += OnPeerConnected;
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
		Console.WriteLine($"Stopping server");
		
		_listener.ConnectionRequestEvent -= OnConnectionRequest;
		_listener.PeerConnectedEvent -= OnPeerConnected;
		_listener.NetworkReceiveEvent -= OnNetworkReceived;
		
		_server.Stop();
	}

	private void OnConnectionRequest(ConnectionRequest connectionRequest)
	{
		Console.WriteLine($"Connection request received: {connectionRequest.RemoteEndPoint}");
		if (_server.ConnectedPeersCount < _maxUsersAllowed)
			connectionRequest.Accept();
		else
			connectionRequest.Reject();
	}

	private void OnPeerConnected(NetPeer peer)
	{
		Console.WriteLine($"New peer connected: {peer.EndPoint}");
	}

	private void OnNetworkReceived(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
	{
		Console.WriteLine($"Received message from {peer.EndPoint}");
	}
}