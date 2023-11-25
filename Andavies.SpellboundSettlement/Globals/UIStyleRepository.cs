using Andavies.MonoGame.UI.Styles;

namespace Andavies.SpellboundSettlement.Globals;

public class UIStyleRepository : IUIStyleRepository
{
	public ButtonStyle DefaultButtonStyle { get; set; }
	public LabelStyle DefaultLabelStyle { get; set; }
	public TextInputStyle DefaultTextInputStyle { get; set; }
}