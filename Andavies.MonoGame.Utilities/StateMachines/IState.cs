namespace Andavies.MonoGame.Utilities.StateMachines;

public interface IState
{
	void Begin();
	void Update(float deltaTime);
	void End();
}