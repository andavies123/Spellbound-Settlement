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
	private readonly INetworkServer _networkServer;
	private readonly IPacketBatchSender _packetBatchSender;
	private readonly IWizardManager _wizardManager;
	
	public GameEventListener(INetworkServer networkServer, IPacketBatchSender packetBatchSender, IWizardManager wizardManager)
	{
		_networkServer = networkServer ?? throw new ArgumentNullException(nameof(networkServer));
		_packetBatchSender = packetBatchSender ?? throw new ArgumentNullException(nameof(packetBatchSender));
		_wizardManager = wizardManager ?? throw new ArgumentNullException(nameof(wizardManager));

		_wizardManager.WizardUpdated += OnWizardUpdated;
		_wizardManager.WizardRemoved += OnWizardRemoved;
	}

	private void OnWizardUpdated(Wizard wizard)
	{
		SendToAllClients(new WizardUpdatedPacket {WizardData = wizard.WizardData});
	}

	private void OnWizardRemoved(Wizard wizard)
	{
		SendToAllClients(new WizardRemovedPacket {WizardData = wizard.WizardData});
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
	
}