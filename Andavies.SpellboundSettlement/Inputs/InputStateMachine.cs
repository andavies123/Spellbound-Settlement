namespace SpellboundSettlement.Inputs;

public class InputStateMachine : IInputStateMachine
{
	private IInputManager _currentInputManager;

	public void ChangeInputManager(IInputManager newInputManager) => _currentInputManager = newInputManager;
	public void Update() => _currentInputManager?.UpdateInput();
}