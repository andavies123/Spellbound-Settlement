using Andavies.MonoGame.Network.Server;
using Andavies.MonoGame.Network.Utilities;
using Andavies.SpellboundSettlement.GameWorld;
using Andavies.SpellboundSettlement.GameWorld.Wizards;
using Andavies.SpellboundSettlement.NetworkMessages.Messages.World;
using LiteNetLib;
using LiteNetLib.Utils;
using Serilog;

namespace Andavies.SpellboundSettlement.Server;

public class GameEventListener : IGameEventListener
{
	private readonly ILogger _logger;
	private readonly INetworkServer _networkServer;
	private readonly IPacketBatchSender _packetBatchSender;
	private readonly IWorldManager _worldManager;
	private readonly IWizardManager _wizardManager;
	
	public GameEventListener(ILogger logger, INetworkServer networkServer, IPacketBatchSender packetBatchSender, IWorldManager worldManager, IWizardManager wizardManager)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_networkServer = networkServer ?? throw new ArgumentNullException(nameof(networkServer));
		_packetBatchSender = packetBatchSender ?? throw new ArgumentNullException(nameof(packetBatchSender));
		_wizardManager = wizardManager ?? throw new ArgumentNullException(nameof(wizardManager));
		_worldManager = worldManager ?? throw new ArgumentNullException(nameof(worldManager));
	}

	public void SubscribeToEvents()
	{
		if (_worldManager.World is not null)
			_worldManager.World.ChunkUpdated += OnChunkUpdated;
		else
			_logger.Warning("Unable to subscribe to chunk updates. World does not exist.");
		
		_wizardManager.WizardUpdated += OnWizardUpdated;
		_wizardManager.WizardRemoved += OnWizardRemoved;
	}

	public void UnsubscribeFromEvents()
	{
		if (_worldManager.World is not null)
			_worldManager.World.ChunkUpdated -= OnChunkUpdated;
		else
			_logger.Warning("Unable to unsubscribe to chunk updates. World does not exist.");
		
		_wizardManager.WizardUpdated -= OnWizardUpdated;
		_wizardManager.WizardRemoved -= OnWizardRemoved;
	}

	private void OnChunkUpdated(Chunk chunk)
	{
		SendToAllClients(new WorldChunkResponsePacket {ChunkData = chunk.ChunkData});
	}

	private void OnWizardUpdated(Wizard wizard)
	{
		SendToAllClients(new WizardUpdatedPacket {WizardData = wizard.Data});
	}

	private void OnWizardRemoved(Wizard wizard)
	{
		SendToAllClients(new WizardRemovedPacket {WizardData = wizard.Data});
	}

	private void SendToAllClients<T>(T packet) where T : INetSerializable
	{
		foreach (NetPeer client in _networkServer.Clients)
		{
			_packetBatchSender.AddPacket(client, packet);
		}
	}
}

public interface IGameEventListener
{
	void SubscribeToEvents();
	void UnsubscribeFromEvents();
}