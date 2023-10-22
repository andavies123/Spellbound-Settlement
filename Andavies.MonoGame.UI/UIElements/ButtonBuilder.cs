using Andavies.MonoGame.UI.Styles;

namespace Andavies.MonoGame.UI.UIElements;

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