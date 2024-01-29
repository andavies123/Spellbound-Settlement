using Andavies.MonoGame.Utilities;
using Andavies.MonoGame.Utilities.StateMachines;

namespace Andavies.SpellboundSettlement.GameWorld.Wizards;

public abstract class WizardState : IState
{
	protected readonly Wizard Wizard;

	protected WizardState(Wizard wizard)
	{
		Wizard = wizard;
	}
	
	public virtual void Begin() { }
	public virtual void Update(float deltaTime) { }
	public virtual void End() { }
}

public class LoiterWizardState : WizardState
{
	private readonly IStateMachine<WizardState> _stateMachine = new StateMachine<WizardState>();
	private readonly StandingSubState _standingState;
	private readonly MovingSubState _movingState;

	public LoiterWizardState(Wizard wizard) : base(wizard)
	{
		_standingState = new StandingSubState(Wizard);
		_movingState = new MovingSubState(Wizard);
	}
	
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
		_movingState.MoveToPosition = new Vector3Int(CenterPosition.X + Random.Shared.Next(-5, 6), CenterPosition.Y, CenterPosition.Z + Random.Shared.Next(-5, 6));
		_movingState.MovementSpeed = 0.25f;
		
		_stateMachine.SetCurrentState(_movingState);
	}
}

public class StandingSubState : WizardState
{
	private float _standingTime;
	
	public StandingSubState(Wizard wizard) : base(wizard) { }

	public event Action? FinishedStanding;
	
	public float StandingLength { get; set; }
	
	public override void Begin()
	{
		_standingTime = 0f;
	}

	public override void Update(float deltaTime)
	{
		_standingTime += deltaTime;
		
		if (_standingTime >= StandingLength)
			FinishedStanding?.Invoke();
	}
}

public class MovingSubState : WizardState
{
	private float _movementTime;
	
	public MovingSubState(Wizard wizard) : base(wizard) { }

	public event Action? ReachedDestination;
	
	public Vector3Int MoveToPosition { get; set; }
	public float MovementSpeed { get; set; }

	public override void Update(float deltaTime)
	{
		_movementTime += deltaTime;

		if (_movementTime < MovementSpeed)
			return;

		_movementTime = 0;
		
		if (Wizard.Position.Distance(MoveToPosition) == 0)
		{
			ReachedDestination?.Invoke();
			return;
		}
		
		// Simple movement for now
		// moves over to match X then moves over to match Z
		if (Wizard.Position.X > MoveToPosition.X)
		{
			Wizard.Position = new Vector3Int(Wizard.Position.X - 1, Wizard.Position.Y, Wizard.Position.Z);
			Wizard.Rotation = Wizard.West;
		}
		else if (Wizard.Position.X < MoveToPosition.X)
		{
			Wizard.Position = new Vector3Int(Wizard.Position.X + 1, Wizard.Position.Y, Wizard.Position.Z);
			Wizard.Rotation = Wizard.East;
		}
		else if (Wizard.Position.Z > MoveToPosition.Z)
		{
			Wizard.Position = new Vector3Int(Wizard.Position.X, Wizard.Position.Y, Wizard.Position.Z - 1);
			Wizard.Rotation = Wizard.South;
		}
		else if (Wizard.Position.Z < MoveToPosition.Z)
		{
			Wizard.Position = new Vector3Int(Wizard.Position.X, Wizard.Position.Y, Wizard.Position.Z + 1);
			Wizard.Rotation = Wizard.North;
		}
	}
}