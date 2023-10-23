using Andavies.MonoGame.UI.Styles;
using Andavies.MonoGame.UI.UIElements;

namespace Andavies.MonoGame.UI.Builders;

public class LabelBuilder : UIElementBuilder<Label>
{
	public LabelBuilder SetText(string text)
	{
		UIElement.Text = text;
		return this;
	}

	public LabelBuilder SetStyle(LabelStyle style)
	{
		UIElement.Style = style;
		return this;
	}
}