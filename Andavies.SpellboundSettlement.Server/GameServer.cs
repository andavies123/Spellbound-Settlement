using System.Net;
using Andavies.MonoGame.Network.Server;
using Andavies.MonoGame.Network.Utilities;
using Andavies.SpellboundSettlement.GameWorld;
using Andavies.SpellboundSettlement.NetworkMessages.Messages.General;
using Andavies.SpellboundSettlement.NetworkMessages.Messages.World;
using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;

namespace Andavies.SpellboundSettlement.Server;

public class GameServer
{
	private readonly INetworkServer _networkServer;
	private readonly IPacketBatchSender _packetBatchSender;
	private readonly World _world = new((0, 0), 5);
	private bool _runGameLoop = false;
    
	public GameServer(INetworkServer networkServer, IPacketBatchSender packetBatchSender)
	{
		_networkServer = networkServer ?? throw new ArgumentNullException(nameof(networkServer));
		_packetBatchSender = packetBatchSender ?? throw new ArgumentNullException(nameof(packetBatchSender));
	}

	public void Start(IPAddress ipAddress, int port, int maxAllowedUsers)
	{
		_networkServer.AddSubscription<WorldChunkRequestPacket>(OnWorldChunkRequestPacketReceived);
		
		_networkServer.ServerStarted += OnServerStarted;
		_networkServer.ClientConnected += OnClientConnected;
		
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
			_networkServer.Update();
			UpdateGame();
			UpdateClients();
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