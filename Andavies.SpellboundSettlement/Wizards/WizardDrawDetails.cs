using System;
using Andavies.MonoGame.Utilities;
using Andavies.SpellboundSettlement.GameWorld.Wizards;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.SpellboundSettlement.Wizards;

public abstract class WizardDrawDetails
{
	public abstract ModelDetails ModelDetails { get; }
	public abstract Type WizardType { get; }
	
	public Model Model { get; set; }
}

public class BasicWizardDrawDetails : WizardDrawDetails
{
	public override ModelDetails ModelDetails { get; } = new("Models/Wizard/wizard");
	public override Type WizardType => typeof(EarthWizardData);
}