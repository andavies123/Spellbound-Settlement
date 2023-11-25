namespace Andavies.SpellboundSettlement.StateMachines;

public interface IState
{
	void Begin();
	void Update();
	void End();
}