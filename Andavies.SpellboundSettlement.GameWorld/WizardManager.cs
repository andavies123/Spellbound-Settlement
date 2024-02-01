using System.Collections.Concurrent;
using Andavies.SpellboundSettlement.GameWorld.Wizards;
using Serilog;

namespace Andavies.SpellboundSettlement.GameWorld;

public class WizardManager : IWizardManager
{
	private readonly ILogger _logger;
	
	private readonly ConcurrentDictionary<Guid, Wizard> _allWizards = new();

	public event Action<Wizard>? WizardUpdated;
	public event Action<Wizard>? WizardRemoved;

	public WizardManager(ILogger logger)
	{
		_logger = logger;
	}

	public IReadOnlyDictionary<Guid, Wizard> AllWizards => _allWizards;
	
	public void AddOrUpdateWizard(Wizard wizard)
	{
		_allWizards[wizard.Data.Id] = wizard;
		
		wizard.Updated += OnWizardUpdated;
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

	private void OnWizardUpdated(Wizard wizard)
	{
		WizardUpdated?.Invoke(wizard);
	}
}