using System;
using System.Threading;
using Andavies.MonoGame.Game.Client;
using Andavies.MonoGame.Game.Server;
using LiteNetLib;
using SpellboundSettlement.Inputs;

namespace SpellboundSettlement.GameStates;

public class LoadGameState : GameState
{
	private const int MaxConnectionAttempts = 10;
	private const int TimeBetweenConnectionAttempts = 5000;
	private readonly IServerStarter _serverStarter;
	private readonly INetworkClient _networkClient;
	
	private NetPeer _server;
	private bool _isConnected = false;
	private int _connectionAttempts = 0;

	public LoadGameState(IServerStarter serverStarter, INetworkClient networkClient)
	{
		_serverStarter = serverStarter;
		_networkClient = networkClient;
	}

	public event Action GameLoaded;
	public event Action UnableToLoadGame;

	public override IInputManager InputState { get; } = null;

	public override void Start()
	{
		base.Start();
		
		// Initialize server
		_serverStarter.StartServer("localhost");

		_networkClient.Start();
	}

	public override void Update(float deltaTimeSeconds)
	{
		base.Update(deltaTimeSeconds);

		while (!_isConnected && _connectionAttempts < MaxConnectionAttempts)
		{
			_connectionAttempts++;
			_isConnected = TryConnect();
			if (!_isConnected)
				Thread.Sleep(TimeBetweenConnectionAttempts);
		}

		if (_isConnected)
			GameLoaded?.Invoke();
		else
			UnableToLoadGame?.Invoke();
	}

	private bool TryConnect()
	{
		_server = _client.Connect("localhost", 9580, "test key");

		if (_server == null)
		{
			Console.WriteLine("Could not connect to the server");
			return false;
		}
		
		Console.WriteLine($"Connected to server - ID: {_server.Id}");
		return true;
	}

	private void OnNetworkReceived(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
	{
		Console.WriteLine($"Received: {reader.GetString(100)}");
		reader.Recycle();
	}
}