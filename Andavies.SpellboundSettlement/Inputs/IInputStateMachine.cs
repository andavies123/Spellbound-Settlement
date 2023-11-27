namespace Andavies.SpellboundSettlement.Inputs;

public interface IInputStateMachine
{
	void ChangeInputState(IInputState newInputState);
	void Update();
}