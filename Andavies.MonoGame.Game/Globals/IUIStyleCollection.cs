using Andavies.MonoGame.UI.Styles;

namespace SpellboundSettlement.Globals;

public interface IUIStyleCollection
{
	ButtonStyle DefaultButtonStyle { get; set; }
	LabelStyle DefaultLabelStyle { get; set; }
	TextInputStyle DefaultTextInputStyle { get; set; }
}