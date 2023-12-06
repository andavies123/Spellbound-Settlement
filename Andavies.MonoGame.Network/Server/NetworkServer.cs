using System.Collections.Concurrent;
using System.Net;
using Andavies.MonoGame.Network.Extensions;
using Andavies.MonoGame.Network.Utilities;
using LiteNetLib;
using LiteNetLib.Utils;
using Serilog;

namespace Andavies.MonoGame.Network.Server;

public class NetworkServer : INetworkServer
{
	private readonly ILogger _logger;
	private readonly IPacketBatchSender _packetBatchSender;
	private readonly NetManager _server;
	private readonly EventBasedNetListener _listener = new();
	private readonly NetPacketProcessor _packetProcessor = new();
	private readonly ConcurrentDictionary<Type, List<Action<INetSerializable, NetPeer>>> _subscriptions = new();
	private bool _isRunning = false;
	private int _maxUsersAllowed = 10;

	public NetworkServer(ILogger logger, IPacketBatchSender packetBatchSender)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_packetBatchSender = packetBatchSender;
		_server = new NetManager(_listener);
	}

	public event Action? ServerStarted;
	public event Action<NetPeer>? ClientConnected;
	public event Action<NetPeer>? ClientDisconnected;
	
	public void Start(IPAddress ipAddress, int port, int maxAllowedUsers)
	{
		if (_isRunning)
		{
			_logger.Warning("Unable to start server. Server is already running");
			return;
		}
        
		_logger.Information("Starting server on IP:Port {ipAddress}:{port}", ipAddress, port);
		_maxUsersAllowed = maxAllowedUsers;
		_isRunning = true;
		_server.Start(5678);

		_listener.ConnectionRequestEvent += OnConnectionRequest;
		_listener.PeerConnectedEvent += OnClientConnected;
		_listener.PeerDisconnectedEvent += OnClientDisconnected;
		_listener.NetworkReceiveEvent += OnNetworkReceived;
		
		ServerStarted?.Invoke();
	}

	public void Update()
	{
		_server.PollEvents();
	}

	public void Stop()
	{
		if (!_isRunning)
		{
			_logger.Warning("Unable to stop server. Server is not running.");
			return;
		}
		
		_logger.Information("Stopping server");
		_isRunning = false;
	}
	
	public void SendPacket<T>(NetPeer client, T packet) where T : INetSerializable
	{
        _packetBatchSender.AddPacket(client, packet);
	}

	public void AddSubscription<T>(Action<INetSerializable, NetPeer> onReceivedCallback) where T : INetSerializable, new()
	{
		if (!_subscriptions.TryGetValue(typeof(T), out List<Action<INetSerializable, NetPeer>>? actions))
		{
			actions = new List<Action<INetSerializable, NetPeer>>();
			_subscriptions.TryAdd(typeof(T), actions);
			_packetProcessor.SubscribeNetSerializable<T, NetPeer>(OnMessageReceived);
		}
		
		actions.Add(onReceivedCallback);
	}

	public void RemoveSubscription<T>(Action<INetSerializable, NetPeer> onReceivedCallback) where T : INetSerializable, new()
	{
		if (!_subscriptions.TryGetValue(typeof(T), out List<Action<INetSerializable, NetPeer>>? actions)) 
			return;
		
		actions.Remove(onReceivedCallback);

		if (actions.Count != 0) 
			return;
		
		_subscriptions.TryRemove(typeof(T), out _);
		_packetProcessor.RemoveSubscription<T>();
	}

	private void OnMessageReceived<T>(T packet, NetPeer client) where T : INetSerializable
	{
		_logger.LogPacketReceived(packet, "Client");
		
		if (!_subscriptions.TryGetValue(typeof(T), out List<Action<INetSerializable, NetPeer>>? actions))
			return;
		
		foreach (Action<INetSerializable, NetPeer> action in actions)
		{
			action.Invoke(packet, client);
		}
	}

	private void OnConnectionRequest(ConnectionRequest connectionRequest)
	{
		string userName = connectionRequest.Data.GetString();
		_logger.Information("Connection request received from {userName} at {endpoint}", userName, connectionRequest.RemoteEndPoint);
		if (_server.ConnectedPeersCount < _maxUsersAllowed)
			connectionRequest.Accept();
		else
			connectionRequest.Reject();
	}

	private void OnClientConnected(NetPeer client)
	{
		_logger.Information("New client connected from {endpoint}", client.EndPoint);
		ClientConnected?.Invoke(client);
	}

	private void OnClientDisconnected(NetPeer client, DisconnectInfo disconnectInfo)
	{
		_logger.Information("Client disconnected from {endpoint}. Reason: {reason}", client.EndPoint, disconnectInfo.Reason);
		ClientDisconnected?.Invoke(client);
	}

	private void OnNetworkReceived(NetPeer client, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
	{
		_packetProcessor.ReadAllPackets(reader, client);
	}
}