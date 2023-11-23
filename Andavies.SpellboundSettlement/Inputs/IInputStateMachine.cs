namespace SpellboundSettlement.Inputs;

public interface IInputStateMachine
{
	void ChangeInputManager(IInputManager newInputManager);
	void Update();
}