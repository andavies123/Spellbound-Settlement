using Andavies.MonoGame.Network.Server;
using Andavies.MonoGame.Network.Utilities;
using Andavies.MonoGame.Utilities;
using Andavies.SpellboundSettlement.GameWorld;
using Andavies.SpellboundSettlement.NetworkMessages.Messages.World;
using LiteNetLib;
using LiteNetLib.Utils;
using Serilog;

namespace Andavies.SpellboundSettlement.Server;

public class NetworkEventListener : INetworkEventListener
{
	private readonly ILogger _logger;
	private readonly INetworkServer _networkServer;
	private readonly IPacketBatchSender _packetBatchSender;
	private readonly IWorldManager _worldManager;
	
	public NetworkEventListener(
		ILogger logger,
		INetworkServer networkServer,
		IPacketBatchSender packetBatchSender,
		IWorldManager worldManager)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_networkServer = networkServer ?? throw new ArgumentNullException(nameof(networkServer));
		_packetBatchSender = packetBatchSender ?? throw new ArgumentNullException(nameof(packetBatchSender));
		_worldManager = worldManager ?? throw new ArgumentNullException(nameof(worldManager));
	}

	public void SubscribeToPackets()
	{
		_networkServer.AddSubscription<WorldChunkRequestPacket>(OnWorldChunkRequestPacketReceived);
		_networkServer.AddSubscription<UpdateTileRequestPacket>(OnUpdateTileRequestPacketReceived);
	}

	public void UnsubscribeFromPackets()
	{
		_networkServer.RemoveSubscription<WorldChunkRequestPacket>(OnWorldChunkRequestPacketReceived);
		_networkServer.RemoveSubscription<UpdateTileRequestPacket>(OnUpdateTileRequestPacketReceived);
	}
	
	private void OnWorldChunkRequestPacketReceived(INetSerializable packet, NetPeer client)
	{
		if (packet is not WorldChunkRequestPacket requestPacket)
			return;

		if (_worldManager.World is null)
		{
			_logger.Warning("Unable to create {packetName}. World does not exist.", nameof(WorldChunkResponsePacket));
			return;
		}
		
		foreach (Vector2Int chunkPosition in requestPacket.ChunkPositions)
		{
			if (!_worldManager.World.TryGetChunk(chunkPosition, out Chunk? chunk))
			{
				//Generate new chunk here
			}
			
			_packetBatchSender.AddPacket(client, new WorldChunkResponsePacket
			{
				ChunkData = chunk.ChunkData
			});
		}
	}

	private void OnUpdateTileRequestPacketReceived(INetSerializable netSerializable, NetPeer client)
	{
		if (netSerializable is not UpdateTileRequestPacket packet)
			return;
		
		_worldManager.UpdateTile(packet.WorldTilePosition, packet.TileId);
	}
}

public interface INetworkEventListener
{
	void SubscribeToPackets();
	void UnsubscribeFromPackets();
}