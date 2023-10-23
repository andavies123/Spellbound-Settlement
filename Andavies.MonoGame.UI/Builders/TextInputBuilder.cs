using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.Styles;
using Andavies.MonoGame.UI.UIElements;

namespace Andavies.MonoGame.UI.Builders;

public class TextInputBuilder : UIElementBuilder<TextInput>
{
	public TextInputBuilder SetHintText(string text)
	{
		UIElement.HintText = text;
		return this;
	}

	public TextInputBuilder SetStyle(TextInputStyle style)
	{
		UIElement.Style = style;
		return this;
	}

	public TextInputBuilder SetInputType(InputType inputType)
	{
		UIElement.InputType = inputType;
		return this;
	}

	public TextInputBuilder SetMaxLength(int maxLength)
	{
		UIElement.MaxLength = maxLength;
		return this;
	}
}