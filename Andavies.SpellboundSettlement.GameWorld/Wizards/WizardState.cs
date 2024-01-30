using Andavies.MonoGame.Utilities.StateMachines;

namespace Andavies.SpellboundSettlement.GameWorld.Wizards;

public abstract class WizardState : IState
{
	protected readonly Wizard Wizard;

	protected WizardState(Wizard wizard)
	{
		Wizard = wizard;
	}
	
	public virtual void Begin() { }
	public virtual void Update(float deltaTime) { }
	public virtual void End() { }
}