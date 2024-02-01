using System.Collections.Concurrent;
using Andavies.SpellboundSettlement.GameWorld.Wizards;

namespace Andavies.SpellboundSettlement.GameWorld;

public class WizardManager : IWizardManager
{
	private readonly ConcurrentDictionary<Guid, Wizard> _allWizards = new();

	public event Action<Wizard>? WizardUpdated;
	public event Action<Wizard>? WizardRemoved;

	public IReadOnlyDictionary<Guid, Wizard> AllWizards => _allWizards;
	
	public void AddOrUpdateWizard(Wizard wizard)
	{
		_allWizards[wizard.Data.Id] = wizard;
		
		wizard.Updated += WizardUpdated;
		WizardUpdated?.Invoke(wizard);
	}

	public void RemoveWizard(Guid wizardId)
	{
		if (_allWizards.TryRemove(wizardId, out Wizard? wizard))
		{
			wizard.Updated -= WizardUpdated;
			WizardRemoved?.Invoke(wizard);
		}
	}
}