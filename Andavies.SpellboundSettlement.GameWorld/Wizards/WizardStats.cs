namespace Andavies.SpellboundSettlement.GameWorld.Wizards;

public abstract class WizardStats
{
	/// <summary>
	/// How many tiles the wizard can move in one second
	/// </summary>
	public float Speed { get; set; } = 10f;
}