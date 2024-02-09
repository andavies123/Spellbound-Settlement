using Andavies.MonoGame.Network.Server;
using Andavies.MonoGame.Network.Utilities;
using Andavies.MonoGame.Utilities;
using Andavies.SpellboundSettlement.GameWorld;
using Andavies.SpellboundSettlement.NetworkMessages.Messages.World;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Andavies.SpellboundSettlement.Server;

public class NetworkEventListener : INetworkEventListener
{
	private readonly INetworkServer _networkServer;
	private readonly IPacketBatchSender _packetBatchSender;
	private readonly IWorldManager _worldManager;
	private readonly World _world;
	
	public NetworkEventListener(
		INetworkServer networkServer,
		IPacketBatchSender packetBatchSender,
		IWorldManager worldManager,
		World world)
	{
		_networkServer = networkServer ?? throw new ArgumentNullException();
		_packetBatchSender = packetBatchSender ?? throw new ArgumentNullException();
		_worldManager = worldManager ?? throw new ArgumentNullException();
		_world = world ?? throw new ArgumentNullException();
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
		
		foreach (Vector2Int chunkPosition in requestPacket.ChunkPositions)
		{
			_packetBatchSender.AddPacket(client, new WorldChunkResponsePacket
			{
				Chunk = _world.GetChunk(chunkPosition)
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