using Andavies.MonoGame.Network.Server;
using Andavies.MonoGame.Network.Utilities;
using Andavies.SpellboundSettlement.GameWorld;
using Andavies.SpellboundSettlement.NetworkMessages.Messages.World;
using Andavies.SpellboundSettlement.Wizards;
using LiteNetLib;
using LiteNetLib.Utils;
using Serilog;

namespace Andavies.SpellboundSettlement.Server;

public class GameEventListener : IGameEventListener
{
	private readonly ILogger _logger;
	private readonly INetworkServer _networkServer;
	private readonly IPacketBatchSender _packetBatchSender;
	private readonly IWizardManager _wizardManager;
	
	public GameEventListener(ILogger logger, INetworkServer networkServer, IPacketBatchSender packetBatchSender, IWizardManager wizardManager)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_networkServer = networkServer ?? throw new ArgumentNullException(nameof(networkServer));
		_packetBatchSender = packetBatchSender ?? throw new ArgumentNullException(nameof(packetBatchSender));
		_wizardManager = wizardManager ?? throw new ArgumentNullException(nameof(wizardManager));

		_wizardManager.WizardUpdated += OnWizardUpdated;
		_wizardManager.WizardRemoved += OnWizardRemoved;
	}

	private void OnWizardUpdated(Wizard wizard)
	{
		SendToAllClients(new WizardUpdatedPacket {Wizard = wizard});
	}

	private void OnWizardRemoved(Wizard wizard)
	{
		SendToAllClients(new WizardRemovedPacket {Wizard = wizard});
	}

	private void SendToAllClients(INetSerializable packet)
	{
		foreach (NetPeer client in _networkServer.Clients)
		{
			_packetBatchSender.AddPacket(client, packet);
		}
	}
}

public interface IGameEventListener
{
	
}