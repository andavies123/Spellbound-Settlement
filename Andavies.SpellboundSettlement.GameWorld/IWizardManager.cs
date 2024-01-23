using Andavies.SpellboundSettlement.Wizards;

namespace Andavies.SpellboundSettlement.GameWorld;

public interface IWizardManager
{
	event Action<Wizard> WizardAdded;
	event Action<Wizard> WizardRemoved;
	
	IReadOnlyDictionary<Guid, Wizard> AllWizards { get; }
	
	void AddWizard(Wizard wizard);
	void RemoveWizard(Guid wizardId);
}