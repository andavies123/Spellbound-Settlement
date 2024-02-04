using Andavies.MonoGame.Utilities;
using Andavies.MonoGame.Utilities.StateMachines;

namespace Andavies.SpellboundSettlement.GameWorld.Wizards;

public class LoiterWizardState : WizardState
{
	private readonly IStateMachine<WizardState> _stateMachine = new StateMachine<WizardState>();
	private readonly StandingWizardState _standingState;
	private readonly MovingWizardState _movingState;

	public LoiterWizardState(Wizard wizard) : base(wizard)
	{
		_standingState = new StandingWizardState(Wizard);
		_movingState = new MovingWizardState(Wizard);
	}
	
	public World? World { get; set; }
	public Vector3Int CenterPosition { get; set; }
	public float MinStandingLength { get; set; }
	public float MaxStandingLength { get; set; }
    
	public override void Begin()
	{
		_stateMachine.SetCurrentState(_standingState);
		
		_standingState.FinishedStanding += ChangeToMovingState; 
		_movingState.ReachedDestination += ChangeToStandingState;
	}

	public override void Update(float deltaTime)
	{
		_stateMachine.UpdateCurrentState(deltaTime);
	}

	public override void End()
	{
		_standingState.FinishedStanding -= ChangeToMovingState;
		_movingState.ReachedDestination -= ChangeToStandingState;
	}

	private void ChangeToStandingState()
	{
		_standingState.StandingLength = Random.Shared.NextSingle() * (MaxStandingLength - MinStandingLength) + MinStandingLength;
		
		_stateMachine.SetCurrentState(_standingState);
	}

	private void ChangeToMovingState()
	{
		_movingState.World = World;
		_movingState.MoveToPosition = new Vector3Int(CenterPosition.X + Random.Shared.Next(-5, 6), CenterPosition.Y, CenterPosition.Z + Random.Shared.Next(-5, 6));
		
		_stateMachine.SetCurrentState(_movingState);
	}
}