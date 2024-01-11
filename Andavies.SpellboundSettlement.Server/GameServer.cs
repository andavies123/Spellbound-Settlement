using System.Net;
using Andavies.MonoGame.Network.Server;
using Andavies.MonoGame.Network.Utilities;
using Andavies.MonoGame.Utilities;
using Andavies.SpellboundSettlement.GameWorld;
using Andavies.SpellboundSettlement.GameWorld.Repositories;
using Andavies.SpellboundSettlement.NetworkMessages.Messages.General;
using Andavies.SpellboundSettlement.NetworkMessages.Messages.World;
using LiteNetLib;
using LiteNetLib.Utils;
using Serilog;

namespace Andavies.SpellboundSettlement.Server;

public class GameServer
{
	private const int TickRate = 50;
	
	private static readonly string TileDetailsJsonAssetPath = Path.Combine(Environment.CurrentDirectory, @"Assets\Config\world_tiles.json");
    
	private readonly ILogger _logger;
	private readonly INetworkServer _networkServer;
	private readonly IPacketBatchSender _packetBatchSender;
	private readonly IServerAccessManager _serverAccessManager;
	private readonly ITileRepository _tileRepository;
	private readonly ITileLoader _tileLoader;
	private readonly World _world;
	
	private bool _runGameLoop = false;
	private int _tickRate = 50;
    
	public GameServer(ILogger logger,
		INetworkServer networkServer,
		IPacketBatchSender packetBatchSender,
		IServerAccessManager serverAccessManager,
		ITileRepository tileRepository,
		ITileLoader tileLoader,
		World world)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_networkServer = networkServer ?? throw new ArgumentNullException(nameof(networkServer));
		_packetBatchSender = packetBatchSender ?? throw new ArgumentNullException(nameof(packetBatchSender));
		_serverAccessManager = serverAccessManager ?? throw new ArgumentNullException(nameof(serverAccessManager));
		_tileRepository = tileRepository ?? throw new ArgumentNullException(nameof(tileRepository));
		_tileLoader = tileLoader ?? throw new ArgumentNullException(nameof(tileLoader));
		_world = world ?? throw new ArgumentNullException(nameof(world));
		
		LoadTiles();
		_world.CreateNewWorld(Vector2Int.Zero, 5);
	}

	private float TickTimeMilliseconds => 1000f / _tickRate;
	
	public void Start(ServerSettings serverSettings, int maxAllowedUsers, int tickRate)
	{
		if (!IPAddress.TryParse(serverSettings.IpAddress, out IPAddress? parsedIpAddress))
			parsedIpAddress = IPAddress.Any;
		if (!int.TryParse(serverSettings.Port, out int parsedPort))
			parsedPort = 5555;

		InitializeServerAccessManager(serverSettings);
		
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
		_packetBatchSender.SendPacketNow(client, new WelcomePacket {WelcomeMessage = "Welcome to this Spellbound Settlement server"});
		
		
		if (File.Exists(TileDetailsJsonAssetPath))
		{
			string jsonContent = File.ReadAllText(TileDetailsJsonAssetPath);
			_packetBatchSender.SendPacketNow(client, new TileDetailsResponsePacket
			{
				TileDetailsJson = jsonContent
			});
		}
		else
		{
			_logger.Warning("Unable to send {packetName} packet", nameof(TileDetailsResponsePacket));
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
		while (_runGameLoop)
		{
			DateTime startTime = DateTime.Now;
			
			_networkServer.Update();
			UpdateGame();
			UpdateClients();

			TimeSpan elapsedTime = DateTime.Now - startTime;
			float sleepTime = TickTimeMilliseconds - elapsedTime.Milliseconds;

			if (sleepTime > 0)
			{
				Thread.Sleep((int)sleepTime);
			}
			else
			{
				_logger.Warning("Game loop exceeded the allotted time of {allottedTime} ms. TickRate = {tickRate}", TickTimeMilliseconds, TickRate);
			}
		}
		
		_networkServer.Stop();
		
		_networkServer.RemoveSubscription<WorldChunkRequestPacket>(OnWorldChunkRequestPacketReceived);
		
		_networkServer.ServerStarted -= OnServerStarted;
		_networkServer.ClientConnected -= OnClientConnected;
	}

	private void UpdateGame()
	{
		// Update game logic
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
	
	private void LoadTiles()
	{
		if (!File.Exists(TileDetailsJsonAssetPath))
		{
			_logger.Warning("Unable to load tiles. File path does not exist. Path: {filePath}", TileDetailsJsonAssetPath);
			return;
		}
		
		string jsonContent = File.ReadAllText(TileDetailsJsonAssetPath);
		_tileLoader.LoadTilesFromJson(jsonContent, _tileRepository);
	}
}