using System.Text;
using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.Styles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Andavies.MonoGame.UI.UIElements;

public class TextInput : UIElement
{
	private readonly StringBuilder _stringBuilder = new();
	private KeyboardState? _currentKeyboardState;
	private KeyboardState? _previousKeyboardState;

	public string Text { get; set; } = string.Empty;
	public string HintText { get; set; } = "Enter Here";
	public int MaxLength { get; set; } = 15;
	public InputType InputType { get; set; } = InputType.NumberLettersAndSpecialCharactersOnly;
	public TextInputStyle? Style { get; set; }

	public void Clear()
	{
		_stringBuilder.Clear();
		Text = _stringBuilder.ToString();
	}
	
	public override void Draw(SpriteBatch spriteBatch)
	{
		if (Style == null)
			return;

		bool displayHint = Text.Length == 0;
		string text = displayHint ? HintText : Text;
		SpriteFont? font = displayHint ? Style.HintTextFont : Style.Font;
		
		spriteBatch.Draw(Style.BackgroundTexture, Bounds, Style.BackgroundColor);

		Vector2 textSize = font?.MeasureString(text) ?? Vector2.Zero;
		float verticalCenter = Bounds.Top + (Bounds.Height - textSize.Y) / 2;
		Vector2 textPosition = Style.TextAlignment switch
		{
			TextAlignment.Left => new Vector2(Bounds.Left, verticalCenter),
			TextAlignment.Center => new Vector2(Bounds.Left + (Bounds.Width - textSize.X)/2, verticalCenter),
			TextAlignment.Right => new Vector2(Bounds.Right - textSize.X, verticalCenter),
			_ => new Vector2(Bounds.Left, verticalCenter)
		};
		spriteBatch.DrawString(font, text, textPosition, Color.Black);
	}

	public override void Update()
	{
		base.Update();

		if (!HasFocus)
			return;

		_previousKeyboardState = _currentKeyboardState;
		_currentKeyboardState = Keyboard.GetState();

		if (_previousKeyboardState == null)
			return;
		
		// Backspace
		if ((_previousKeyboardState?.IsKeyDown(Keys.Back) ?? false) && (_currentKeyboardState?.IsKeyUp(Keys.Back) ?? false))
			_stringBuilder.Remove(_stringBuilder.Length - 1, 1);
		
		// Enter
		if ((_previousKeyboardState?.IsKeyDown(Keys.Enter) ?? false) && (_currentKeyboardState?.IsKeyUp(Keys.Enter) ?? false))
			_stringBuilder.Clear();

		if (_stringBuilder.Length >= MaxLength)
			return;

		switch (InputType)
		{
			case InputType.AllText: ListenForAllText(); break;
			case InputType.NumbersOnly: ListenForNumbersOnly(); break;
			case InputType.LettersOnly: break;
			case InputType.LowerCaseOnly: break;
			case InputType.UpperCaseOnly: break;
			case InputType.NumberAndLettersOnly: break;
			case InputType.NumberLettersAndSpecialCharactersOnly: break;
			default: ListenForAllText(); break;
		}

		Text = _stringBuilder.ToString();
	}

	private void ListenForAllText()
	{
		
	}

	private readonly Dictionary<Keys, char> _keyToCharMap = new()
	{
		{ Keys.D0, '0' },
		{ Keys.D1, '1' },
		{ Keys.D2, '2' },
		{ Keys.D3, '3' },
		{ Keys.D4, '4' },
		{ Keys.D5, '5' },
		{ Keys.D6, '6' },
		{ Keys.D7, '7' },
		{ Keys.D8, '8' },
		{ Keys.D9, '9' },
		{ Keys.Decimal, '.' },
		{ Keys.NumPad0, '0' },
		{ Keys.NumPad1, '1' },
		{ Keys.NumPad2, '2' },
		{ Keys.NumPad3, '3' },
		{ Keys.NumPad4, '4' },
		{ Keys.NumPad5, '5' },
		{ Keys.NumPad6, '6' },
		{ Keys.NumPad7, '7' },
		{ Keys.NumPad8, '8' },
		{ Keys.NumPad9, '9' },
		{ Keys.OemPeriod, '.' }
	};

	private void ListenForNumbersOnly()
	{
		foreach (KeyValuePair<Keys, char> kvp in _keyToCharMap)
		{
			if ((_previousKeyboardState?.IsKeyDown(kvp.Key) ?? false) && (_currentKeyboardState?.IsKeyUp(kvp.Key) ?? false))
				_stringBuilder.Append(kvp.Value);
		}
	}
}