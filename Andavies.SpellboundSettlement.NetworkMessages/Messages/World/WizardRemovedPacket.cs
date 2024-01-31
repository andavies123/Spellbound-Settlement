using System.ComponentModel;
using Andavies.SpellboundSettlement.GameWorld.Wizards;
using LiteNetLib.Utils;

namespace Andavies.SpellboundSettlement.NetworkMessages.Messages.World;

public class WizardRemovedPacket : INetSerializable
{
	public WizardData? WizardData { get; set; }

	public void Serialize(NetDataWriter writer)
	{
		writer.Put(WizardData?.GetType().Name);
		WizardData?.Serialize(writer);
	}

	public void Deserialize(NetDataReader reader)
	{
		string type = reader.GetString();

		WizardData = type switch
		{
			nameof(EarthWizardData) => new EarthWizardData(),
			_ => throw new InvalidEnumArgumentException($"{type} not implemented")
		};
		
		WizardData.Deserialize(reader);
	}
}