namespace Andavies.MonoGame.Utilities.StateMachines;

public class StateMachine<T> : IStateMachine<T> where T : IState
{
	public T? CurrentState { get; set; }

	public void SetCurrentState(T nextState)
	{
		CurrentState?.End();
		CurrentState = nextState;
		CurrentState?.Begin();
	}

	public void UpdateCurrentState(float deltaTimeSeconds)
	{
		CurrentState?.Update(deltaTimeSeconds);
	}
}