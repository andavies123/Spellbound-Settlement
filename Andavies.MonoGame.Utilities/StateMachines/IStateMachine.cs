namespace Andavies.MonoGame.Utilities.StateMachines;

public interface IStateMachine<T> where T : IState
{
	T? CurrentState { get; }

	void SetCurrentState(T nextState);
	void UpdateCurrentState(float deltaTimeSeconds);
}