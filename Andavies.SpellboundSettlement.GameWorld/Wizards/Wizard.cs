using Andavies.MonoGame.Utilities.StateMachines;
using Microsoft.Xna.Framework;

namespace Andavies.SpellboundSettlement.GameWorld.Wizards;

public abstract class Wizard
{
	// Todo: Add these constants to a general class
	public const float North = MathHelper.PiOver2 * 3;
	public const float South = MathHelper.PiOver2;
	public const float East = 0f;
	public const float West = MathHelper.Pi;
	
	private readonly StateMachine<WizardState> _stateMachine = new();
	protected readonly LoiterWizardState LoiterState;

	protected Wizard()
	{
		// Initialize common states
		LoiterState = new LoiterWizardState(this);
	}
	
	public event Action<Wizard>? Updated;

	public abstract WizardData Data { get; }
	public abstract WizardStats Stats { get; }
	
	public World? World { get; set; }

	public void Update(float deltaTimeSeconds)
	{
		_stateMachine.UpdateCurrentState(deltaTimeSeconds);
		if (Data.IsChanged)
		{
			Data.AcceptChanges();
			Updated?.Invoke(this);
		}
	}

	public void Loiter()
	{
		LoiterState.World = World;
		LoiterState.CenterPosition = Data.Position;
		LoiterState.MinStandingLength = 3;
		LoiterState.MaxStandingLength = 7;
		_stateMachine.SetCurrentState(LoiterState);
	}
}