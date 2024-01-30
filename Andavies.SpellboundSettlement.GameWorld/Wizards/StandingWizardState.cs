namespace Andavies.SpellboundSettlement.GameWorld.Wizards;

public class StandingWizardState : WizardState
{
	private float _standingTime;
	
	public StandingWizardState(Wizard wizard) : base(wizard) { }

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