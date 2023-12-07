using System.Net;
using Andavies.MonoGame.Network.Server;
using Andavies.MonoGame.Network.Utilities;
using Andavies.SpellboundSettlement.GameWorld;
using Andavies.SpellboundSettlement.NetworkMessages.Messages.General;
using Andavies.SpellboundSettlement.NetworkMessages.Messages.World;
using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
using Serilog;

namespace Andavies.SpellboundSettlement.Server;

public class GameServer
{
	private const int TickRate = 50;
	private readonly ILogger _logger;
	private readonly INetworkServer _networkServer;
	private readonly IPacketBatchSender _packetBatchSender;
	private readonly World _world = new((0, 0), 5);
	private bool _runGameLoop = false;
	private int _tickRate = 50;
    
	public GameServer(ILogger logger, INetworkServer networkServer, IPacketBatchSender packetBatchSender)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_networkServer = networkServer ?? throw new ArgumentNullException(nameof(networkServer));
		_packetBatchSender = packetBatchSender ?? throw new ArgumentNullException(nameof(packetBatchSender));
	}

	private float TickTimeMilliseconds => 1000f / _tickRate;
	
	public void Start(IPAddress ipAddress, int port, int maxAllowedUsers, int tickRate)
	{
		_networkServer.AddSubscription<WorldChunkRequestPacket>(OnWorldChunkRequestPacketReceived);
		
		_networkServer.ServerStarted += OnServerStarted;
		_networkServer.ClientConnected += OnClientConnected;

		_tickRate = tickRate;
		_networkServer.Start(ipAddress, port, maxAllowedUsers);
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
	}

	private void OnWorldChunkRequestPacketReceived(INetSerializable packet, NetPeer client)
	{
		if (packet is not WorldChunkRequestPacket requestPacket)
			return;
		
		foreach (Vector2 chunkPosition in requestPacket.ChunkPositions)
		{
			_packetBatchSender.AddPacket(client, new WorldChunkResponsePacket {Chunk = _world.GetChunk(chunkPosition)});
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
}