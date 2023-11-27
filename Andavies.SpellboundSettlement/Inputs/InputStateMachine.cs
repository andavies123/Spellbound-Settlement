namespace Andavies.SpellboundSettlement.Inputs;

public class InputStateMachine : IInputStateMachine
{
	private IInputState _currentInputState;

	public void ChangeInputState(IInputState newInputState) => _currentInputState = newInputState;
	public void Update() => _currentInputState?.UpdateInput();
}