using Andavies.MonoGame.Network.Server;
using Andavies.MonoGame.NetworkUtilities.Extensions;
using Andavies.SpellboundSettlement.NetworkMessages.Messages.General;
using Andavies.SpellboundSettlement.NetworkMessages.Messages.World;
using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
using Serilog;

namespace Andavies.SpellboundSettlement.Server;

public class NetworkServer : INetworkServer
{
	private readonly ILogger _logger;
	private readonly NetManager _server;
	private readonly EventBasedNetListener _listener = new();
	private readonly NetPacketProcessor _packetProcessor = new();
	private readonly NetDataWriter _dataWriter = new();
	private readonly World.World _world = new((0, 0), 5);
	private int _maxUsersAllowed;
	private bool _isRunning;

	public NetworkServer(ILogger logger)
	{
		_logger = logger;
		_server = new NetManager(_listener);
	}
	
	public void Start(int port, int maxUsersAllowed)
	{
		_logger.Information("Starting server on port {port}", port);
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
		_logger.Information("Stopping server...");
		
		_listener.ConnectionRequestEvent -= OnConnectionRequest;
		_listener.PeerConnectedEvent -= OnPeerConnected;
		_listener.PeerDisconnectedEvent -= OnPeerDisconnected;
		_listener.NetworkReceiveEvent -= OnNetworkReceived;
		
		_server.Stop();
	}

	private void OnConnectionRequest(ConnectionRequest connectionRequest)
	{
		_logger.Information("Connection request received from {endpoint}", connectionRequest.RemoteEndPoint);
		if (_server.ConnectedPeersCount < _maxUsersAllowed)
			connectionRequest.Accept();
		else
			connectionRequest.Reject();
	}

	private void OnPeerConnected(NetPeer client)
	{
		_logger.Information("New client connected from {endpoint}", client.EndPoint);
		
		SendMessage(client, new WelcomePacket {WelcomeMessage = "Welcome to this Spellbound Settlement server"});
	}

	private void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
	{
		_logger.Information("Client disconnected from {endpoint}. Reason: {reason}", peer.EndPoint, disconnectInfo.Reason);
	}

	private void OnNetworkReceived(NetPeer client, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
	{
		_packetProcessor.ReadAllPackets(reader, client);
	}

	private void OnWorldChunkRequestPacketReceived(WorldChunkRequestPacket packet, NetPeer client)
	{
		_logger.LogPacketReceived(packet, client.EndPoint.ToString());

		foreach (Vector2 chunkPosition in packet.ChunkPositions)
		{
			SendMessage(client, new WorldChunkResponsePacket
			{
				Chunk = _world.GetChunk(chunkPosition)
			});	
		}
	}
}