using System.ComponentModel;
using Andavies.SpellboundSettlement.GameWorld.Wizards;
using Andavies.SpellboundSettlement.Wizards;
using LiteNetLib.Utils;

namespace Andavies.SpellboundSettlement.NetworkMessages.Messages.World;

public class WizardRemovedPacket : INetSerializable
{
	public Wizard? Wizard { get; set; }

	public void Serialize(NetDataWriter writer)
	{
		writer.Put(Wizard?.GetType().Name);
		Wizard?.Serialize(writer);
	}

	public void Deserialize(NetDataReader reader)
	{
		string type = reader.GetString();

		Wizard = type switch
		{
			nameof(BasicWizard) => new BasicWizard(),
			_ => throw new InvalidEnumArgumentException($"{type} not implemented")
		};
		
		Wizard.Deserialize(reader);
	}
}