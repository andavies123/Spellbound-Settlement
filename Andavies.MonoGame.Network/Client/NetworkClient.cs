using System.Collections.Concurrent;
using Andavies.MonoGame.NetworkUtilities.Extensions;
using LiteNetLib;
using LiteNetLib.Utils;
using Serilog;

namespace Andavies.MonoGame.Network.Client;

public class NetworkClient : INetworkClient
{
	private const int ConnectTimeoutMSec = 5000;

	private readonly ILogger _logger;
	private readonly EventBasedNetListener _listener = new();
	private readonly NetPacketProcessor _packetProcessor = new();
	private readonly NetDataWriter _dataWriter = new();
	private readonly NetManager _client;
	private readonly ConcurrentDictionary<Type, List<Action<INetSerializable>>> _subscriptions = new();
	
	private NetPeer? _server;
	
	public NetworkClient(ILogger logger)
	{
		_logger = logger;
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

	public void TryConnect(string ipAddress, int port)
	{
		const string userName = "Andavies123";
		_logger.Information("Attempting to connect {username} to the server...", userName);
		
		_dataWriter.Reset();
		_dataWriter.Put(userName);
		_server = _client.Connect(ipAddress, port, _dataWriter);
		_dataWriter.Reset();

		DateTime start = DateTime.Now;
		DateTime now = DateTime.Now;
		
		while (!IsConnected || now.Subtract(start).Milliseconds >= ConnectTimeoutMSec)
		{
			Thread.Sleep(100);
			now = DateTime.Now;
		}
        
		if (!IsConnected)
		{
			_logger.Information("Could not connect to the server");
			return;
		}

		_logger.Information("Connected to server {server}", _server.EndPoint);
	}

	public void AddSubscription<T>(Action<INetSerializable> onReceivedCallback) where T : INetSerializable, new()
	{
		if (!_subscriptions.TryGetValue(typeof(T), out List<Action<INetSerializable>>? actions))
		{
			actions = new List<Action<INetSerializable>>();
			_subscriptions.TryAdd(typeof(T), actions);
			_packetProcessor.SubscribeNetSerializable<T>(OnMessageReceived);
		}
		
		actions.Add(onReceivedCallback);
	}

	public void RemoveSubscription<T>(Action<INetSerializable> onReceivedCallback) where T : INetSerializable, new()
	{
		if (!_subscriptions.TryGetValue(typeof(T), out List<Action<INetSerializable>>? actions)) 
			return;
		
		actions.Remove(onReceivedCallback);

		if (actions.Count != 0) 
			return;
		
		_subscriptions.TryRemove(typeof(T), out _);
		_packetProcessor.RemoveSubscription<T>();
	}

	private void OnMessageReceived<T>(T packet) where T : INetSerializable
	{
		_logger.LogPacketReceived(packet, "Server");
		
		if (!_subscriptions.TryGetValue(typeof(T), out List<Action<INetSerializable>>? actions))
			return;
		
		foreach (Action<INetSerializable> action in actions)
		{
			action.Invoke(packet);
		}
	}
	
	private void OnNetworkReceived(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
	{
		// When receiving a packet that we are not currently subscribed to
		// a ParseException error is thrown, so here we are handling that
		// in the case a packet is received that we are not subscribed to 
		try
		{
			_packetProcessor.ReadAllPackets(reader, peer);
		}
		catch (ParseException parseException)
		{
			_logger.Warning("ParseException - {exception}", parseException.Message);
		}
	}

	public void SendMessage<T>(T packet) where T : INetSerializable
	{
		_dataWriter.Reset();
		_packetProcessor.WriteNetSerializable(_dataWriter, ref packet);
		_server?.Send(_dataWriter, DeliveryMethod.ReliableOrdered);
		_logger.LogPacketSent(packet, "Server");
	}
}