using System.Collections.Concurrent;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Andavies.MonoGame.Game.Server;

public class LiteNetServer
{
	private readonly EventBasedNetListener _listener = new();
	private readonly NetSerializer _netSerializer = new();
	private readonly NetManager _server;

	private readonly int _maxAllowedConnections;
	private readonly ConcurrentDictionary<string, int> _connectedClients = new();
	
	public LiteNetServer(int maxAllowedConnections)
	{
		_maxAllowedConnections = maxAllowedConnections;
		_server = new NetManager(_listener);
	}

	public void Start(int port)
	{
		Console.WriteLine("Starting server");
		_server.Start(port);

		_listener.ConnectionRequestEvent += OnConnectionRequest;
		_listener.PeerConnectedEvent += OnPeerConnected;

		while (!Console.KeyAvailable)
		{
			_server.PollEvents(); // Unsure what this does
			ProcessClientMessages();
			UpdateGame();
			SendUpdatesToClients();
			Thread.Sleep(15);
		}
		
		Stop();
	}

	public void Stop()
	{
		Console.WriteLine("Stopping server");
		_server.Stop();
	}

	private void OnConnectionRequest(ConnectionRequest request)
	{
		if (_server.ConnectedPeersCount < _maxAllowedConnections)
			request.AcceptIfKey("Sample Key");
		else
			request.Reject();
	}

	private void OnPeerConnected(NetPeer peer)
	{
		// Print the IP of the connected peer
		Console.WriteLine($"Server received connection: {peer.EndPoint}");

		string key = peer.EndPoint.ToString();
		if (!_connectedClients.ContainsKey(key)) // Connect the client if it is not already connected
		{
			
		}
		else // Client must have disconnected from the client
		{
			
		}
		
		NetDataWriter writer = new();
		writer.Put("Hello Client");
		
		// Send with reliability
		peer.Send(writer, DeliveryMethod.ReliableOrdered);
	}

	private void ProcessClientMessages()
	{
		
	}

	private void UpdateGame()
	{
		
	}
	
	private void SendUpdatesToClients()
	{
		foreach (NetPeer peer in _server.ConnectedPeerList)
		{
			// Send updates to the peer
		}
	}
}