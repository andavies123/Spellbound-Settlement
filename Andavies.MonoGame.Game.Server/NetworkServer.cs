using Andavies.MonoGame.Game.Server.Interfaces;
using Andavies.SpellboundSettlement.NetworkMessages.Messages.General;
using Andavies.SpellboundSettlement.NetworkMessages.Messages.World;
using Andavies.SpellboundSettlement.World;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Andavies.MonoGame.Game.Server;

public class NetworkServer : INetworkServer
{
	private readonly NetManager _server;
	private readonly EventBasedNetListener _listener = new();
	private readonly NetPacketProcessor _packetProcessor = new();
	private readonly NetDataWriter _dataWriter = new();
	private readonly World _world = new((0, 0), 5);
	private int _maxUsersAllowed;
	private bool _isRunning;

	public NetworkServer()
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
		_packetProcessor.RegisterNestedType(() => new WorldChunkResponsePacket());
		
		_packetProcessor.SubscribeNetSerializable<WorldChunkRequestPacket, NetPeer>(OnWorldChunkRequestPacketReceived);

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

	public void SendMessage<T>(NetPeer client, T packet) where T : INetSerializable
	{
		_dataWriter.Reset();
		_packetProcessor.WriteNetSerializable(_dataWriter, ref packet);
		client.Send(_dataWriter, DeliveryMethod.ReliableOrdered);
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
		
		SendMessage(client, new WelcomePacket {WelcomeMessage = "Welcome to this Spellbound Settlement server"});
	}

	private void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
	{
		Console.WriteLine($"Server: Peer disconnected\n" +
		                  $"\tIP: {peer.EndPoint}\n" +
		                  $"\tReason: {disconnectInfo.Reason}");
	}

	private void OnNetworkReceived(NetPeer client, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
	{
		_packetProcessor.ReadAllPackets(reader, client);
	}

	private void OnWorldChunkRequestPacketReceived(WorldChunkRequestPacket packet, NetPeer client)
	{
		Console.WriteLine($"Server: Received Packet - \n" +
		                  $"\tType: {nameof(WorldChunkRequestPacket)}\n" +
		                  $"\tFrom: {client.EndPoint}\n" +
		                  $"\tData: {packet}");
		
		SendMessage(client, new WorldChunkResponsePacket
		{
			Chunk = _world.GetChunk(packet.ChunkPosition)
		});
	}
}