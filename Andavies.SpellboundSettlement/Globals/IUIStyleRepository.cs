using Andavies.MonoGame.UI.Styles;

namespace SpellboundSettlement.Globals;

public interface IUIStyleRepository
{
	ButtonStyle DefaultButtonStyle { get; set; }
	LabelStyle DefaultLabelStyle { get; set; }
	TextInputStyle DefaultTextInputStyle { get; set; }
}