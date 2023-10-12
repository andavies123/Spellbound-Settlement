namespace SpellboundSettlement.StateMachines;

public interface IState
{
	void Init();
	void BeginState();
	void Update();
	void EndState();
}