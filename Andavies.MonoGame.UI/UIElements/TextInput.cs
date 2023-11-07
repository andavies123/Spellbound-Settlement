﻿using System.Text;
using Andavies.MonoGame.Input.InputListeners;
using Andavies.MonoGame.UI.Core;
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

	public TextInput(Point position, Point size, TextInputStyle style, ITextListener textListener)
		: base(position, size)
	{
		Style = style;
		TextListener = textListener;
	}

	public TextInput(Point size, TextInputStyle style, ITextListener textListener) : base(size)
	{
		Style = style;
		TextListener = textListener;
	}

	public string Text { get; set; } = string.Empty;
	public string HintText { get; set; } = "Enter Here";
	public int MaxLength { get; set; } = 15;
	public InputType InputType { get; set; } = InputType.NumberLettersAndSpecialCharactersOnly;
	public ITextListener TextListener { get; set; }
	public TextInputStyle Style { get; set; }

	public void Clear()
	{
		_stringBuilder.Clear();
		Text = _stringBuilder.ToString();
	}
	
	public override void Draw(SpriteBatch spriteBatch)
	{
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

	public override void Update(float deltaTimeSeconds)
	{
		base.Update(deltaTimeSeconds);

		if (!HasFocus)
			return;

		_previousKeyboardState = _currentKeyboardState;
		_currentKeyboardState = Keyboard.GetState();
		
		// Backspace
		if (Input.Input.WasKeyPressed(Keys.Back))
			_stringBuilder.Remove(_stringBuilder.Length - 1, 1);
		
		// Enter
		if (Input.Input.WasKeyPressed(Keys.Enter))
			_stringBuilder.Clear();

		if (_stringBuilder.Length >= MaxLength)
			return;

		TextListener.Listen(_previousKeyboardState, _currentKeyboardState, _stringBuilder);

		Text = _stringBuilder.ToString();
	}
}