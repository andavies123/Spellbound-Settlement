using System.Diagnostics;
using System.Net;
using Andavies.MonoGame.Network.Server;
using Andavies.MonoGame.Network.Utilities;
using Andavies.MonoGame.Utilities;
using Andavies.MonoGame.Utilities.GameEvents;
using Andavies.SpellboundSettlement.GameWorld;
using Andavies.SpellboundSettlement.GameWorld.Wizards;
using Andavies.SpellboundSettlement.NetworkMessages.Messages.General;
using Andavies.SpellboundSettlement.NetworkMessages.Messages.World;
using LiteNetLib;
using LiteNetLib.Utils;
using Serilog;

namespace Andavies.SpellboundSettlement.Server;

public class GameServer
{
	private const int TickRate = 50;
    
	private readonly ILogger _logger;
	private readonly INetworkServer _networkServer;
	private readonly IPacketBatchSender _packetBatchSender;
	private readonly IServerAccessManager _serverAccessManager;
	private readonly IGameEventSystem _gameEventSystem;
	private readonly IWorldManager _worldManager;
	private readonly IWizardManager _wizardManager;
	private readonly ITileRegister _tileRegister;
	private readonly IGameEventListener _gameEventListener;
	private readonly World _world;
	
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
		IGameEventListener gameEventListener, // Only added here so it gets started
		World world)
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
		_world = world ?? throw new ArgumentNullException(nameof(world));
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
		
		_worldManager.CreateWorld();
		
		_gameEventSystem.Publish(new WorldCreatedGameEvent());
		
		_networkServer.AddSubscription<WorldChunkRequestPacket>(OnWorldChunkRequestPacketReceived);
		
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

	private void OnWorldChunkRequestPacketReceived(INetSerializable packet, NetPeer client)
	{
		if (packet is not WorldChunkRequestPacket requestPacket)
			return;
		
		foreach (Vector2Int chunkPosition in requestPacket.ChunkPositions)
		{
			_packetBatchSender.AddPacket(client, new WorldChunkResponsePacket
			{
				Chunk = _world.GetChunk(chunkPosition)
			});
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
		
		_networkServer.RemoveSubscription<WorldChunkRequestPacket>(OnWorldChunkRequestPacketReceived);
		
		_networkServer.ServerStarted -= OnServerStarted;
		_networkServer.ClientConnected -= OnClientConnected;
	}

	private void UpdateGame(float deltaTimeSeconds)
	{
		_worldManager.Tick(deltaTimeSeconds);
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