using Andavies.MonoGame.UI.Styles;
using Andavies.MonoGame.UI.UIElements;

namespace Andavies.MonoGame.UI.Builders;

public class ButtonBuilder : UIElementBuilder<Button>
{
	public ButtonBuilder SetText(string text)
	{
		UIElement.Text = text;
		return this;
	}

	public ButtonBuilder SetStyle(ButtonStyle style)
	{
		UIElement.Style = style;
		return this;
	}
}