using Andavies.SpellboundSettlement.GameWorld.Wizards;
using Andavies.SpellboundSettlement.Wizards;

namespace Andavies.SpellboundSettlement.GameWorld;

public interface IWizardManager
{
	event Action<Wizard> WizardUpdated;
	event Action<Wizard> WizardRemoved;
	
	IReadOnlyDictionary<Guid, Wizard> AllWizards { get; }
	
	void AddOrUpdateWizard(Wizard wizard);
	void RemoveWizard(Guid wizardId);
}