using System.Diagnostics;
using System.Net;
using Andavies.MonoGame.Network.Server;
using Andavies.MonoGame.Network.Utilities;
using Andavies.MonoGame.Utilities.GameEvents;
using Andavies.SpellboundSettlement.GameWorld;
using Andavies.SpellboundSettlement.GameWorld.Wizards;
using Andavies.SpellboundSettlement.NetworkMessages.Messages.General;
using Andavies.SpellboundSettlement.NetworkMessages.Messages.World;
using LiteNetLib;
using Serilog;

namespace Andavies.SpellboundSettlement.Server;

public class GameServer
{
	private readonly ILogger _logger;
	private readonly INetworkServer _networkServer;
	private readonly IPacketBatchSender _packetBatchSender;
	private readonly IServerAccessManager _serverAccessManager;
	private readonly IGameEventSystem _gameEventSystem;
	private readonly IWorldManager _worldManager;
	private readonly IWizardManager _wizardManager;
	private readonly ITileRegister _tileRegister;
	private readonly IGameEventListener _gameEventListener;
	private readonly INetworkEventListener _networkEventListener;
	
	private bool _runGameLoop = false;
	private int _tickRate = 50;
    
	public GameServer(ILogger logger,
		INetworkServer networkServer,
		IPacketBatchSender packetBatchSender,
		IServerAccessManager serverAccessManager,
		IGameEventSystem gameEventSystem,
		IWorldManager worldManager,
		IWizardManager wizardManager,
		ITileRegister tileRegister,
		IGameEventListener gameEventListener,
		INetworkEventListener networkEventListener)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_networkServer = networkServer ?? throw new ArgumentNullException(nameof(networkServer));
		_packetBatchSender = packetBatchSender ?? throw new ArgumentNullException(nameof(packetBatchSender));
		_serverAccessManager = serverAccessManager ?? throw new ArgumentNullException(nameof(serverAccessManager));
		_gameEventSystem = gameEventSystem ?? throw new ArgumentNullException(nameof(gameEventSystem));
		_worldManager = worldManager ?? throw new ArgumentNullException(nameof(worldManager));
		_wizardManager = wizardManager ?? throw new ArgumentNullException(nameof(wizardManager));
		_tileRegister = tileRegister ?? throw new ArgumentNullException(nameof(tileRegister));
		_gameEventListener = gameEventListener ?? throw new ArgumentNullException(nameof(gameEventListener));
		_networkEventListener = networkEventListener ?? throw new ArgumentNullException(nameof(networkEventListener));
	}

	private float TickTimeMilliseconds => 1000f / _tickRate;
	
	public void Start(ServerSettings serverSettings, int maxAllowedUsers, int tickRate)
	{
		if (!IPAddress.TryParse(serverSettings.IpAddress, out IPAddress? parsedIpAddress))
			parsedIpAddress = IPAddress.Any;
		if (!int.TryParse(serverSettings.Port, out int parsedPort))
			parsedPort = 5555;

		InitializeServerAccessManager(serverSettings);
		_tileRegister.RegisterTiles();
		
		_worldManager.CreateNewWorld();
		
		_gameEventSystem.Publish(new WorldCreatedGameEvent());
		
		_networkEventListener.SubscribeToPackets();
		_gameEventListener.SubscribeToEvents();
		
		_networkServer.ServerStarted += OnServerStarted;
		_networkServer.ClientConnected += OnClientConnected;

		_tickRate = tickRate;
		_networkServer.Start(parsedIpAddress, parsedPort, maxAllowedUsers);
	}

	public void Stop()
	{
		_runGameLoop = false;
	}

	private void OnServerStarted()
	{
		_runGameLoop = true;
		Thread loopThread = new(GameLoop);
		loopThread.Start();
	}

	private void OnClientConnected(NetPeer client)
	{
		_packetBatchSender.AddPacket(client, new WelcomePacket {WelcomeMessage = "Welcome to this Spellbound Settlement server"});
		
		foreach (Wizard wizard in _wizardManager.AllWizards.Values)
		{
			_packetBatchSender.AddPacket(client, new WizardUpdatedPacket {WizardData = wizard.Data});
		}
	}

	private void GameLoop()
	{
		Stopwatch gameLoopTimer = new();
		gameLoopTimer.Start();
		
		while (_runGameLoop)
		{
			float deltaTimeSeconds = (float) gameLoopTimer.Elapsed.TotalSeconds;
			gameLoopTimer.Restart();
			
			_networkServer.Update();
			UpdateGame(deltaTimeSeconds);
			UpdateClients();

			float sleepTime = Math.Max(0, TickTimeMilliseconds - deltaTimeSeconds/1000);

			//_logger.Information("DeltaTime: {deltaTime}", deltaTimeSeconds);
			
			if (sleepTime > 0)
			{
				Thread.Sleep(TimeSpan.FromMilliseconds(sleepTime));
			}
			else
			{
				_logger.Warning("Game loop exceeded the allotted time of {allottedTime} ms. Time = {time}", TickTimeMilliseconds, deltaTimeSeconds);
			}
		}
		
		_networkServer.Stop();
		
		_networkEventListener.UnsubscribeFromPackets();
		_gameEventListener.UnsubscribeFromEvents();
		
		_networkServer.ServerStarted -= OnServerStarted;
		_networkServer.ClientConnected -= OnClientConnected;
	}

	private void UpdateGame(float deltaTimeSeconds)
	{
		_worldManager.Update(deltaTimeSeconds);
	}

	private void UpdateClients()
	{
		Thread thread = new(_packetBatchSender.SendBatch);
		thread.Start();
	}

	private void InitializeServerAccessManager(ServerSettings serverSettings)
	{
		_serverAccessManager.ClearWhiteList();
		_serverAccessManager.ClearBlackList();
		_serverAccessManager.WhiteListEnabled = serverSettings.WhiteListEnabled;
		_serverAccessManager.BlackListEnabled = serverSettings.BlackListEnabled;
		serverSettings.WhiteList.ForEach(_serverAccessManager.AddToWhiteList);
		serverSettings.BlackList.ForEach(_serverAccessManager.AddToBlackList);
	}
}