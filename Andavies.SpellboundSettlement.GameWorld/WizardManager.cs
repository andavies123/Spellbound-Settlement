using System.Collections.Concurrent;
using Serilog;
using Andavies.SpellboundSettlement.Wizards;

namespace Andavies.SpellboundSettlement.GameWorld;

public class WizardManager : IWizardManager
{
	private readonly ILogger _logger;
	
	private readonly ConcurrentDictionary<Guid, Wizard> _allWizards = new();

	public event Action<Wizard>? WizardAdded;
	public event Action<Wizard>? WizardRemoved;

	public WizardManager(ILogger logger)
	{
		_logger = logger;
	}

	public IReadOnlyDictionary<Guid, Wizard> AllWizards => _allWizards;
	
	public void AddWizard(Wizard wizard)
	{
		if (!_allWizards.TryAdd(wizard.Id, wizard))
		{
			_logger.Warning("Unable to add wizard: {name}, {id}", wizard.Name, wizard.Id);
			return;
		}
		
		WizardAdded?.Invoke(wizard);
	}

	public void RemoveWizard(Guid wizardId)
	{
		if (!_allWizards.TryRemove(wizardId, out Wizard? wizard))
		{
			_logger.Warning("Unable to remove wizard: {id}", wizardId);
			return;
		}
		
		WizardRemoved?.Invoke(wizard);
	}
}