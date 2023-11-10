using System.Text;
using Andavies.MonoGame.Inputs;
using Andavies.MonoGame.Inputs.InputListeners;
using Andavies.MonoGame.UI.Core;
using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.Styles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Andavies.MonoGame.UI.UIElements.TextInputs;

public class TextInput : UIElement
{
	private const float TimeBetweenBackspaces = 0.1f;
	private const float InitialTimeBetweenBackspaces = 0.5f;

	private string _text = string.Empty;
	
	private readonly StringBuilder _stringBuilder = new();
	private KeyboardState? _currentKeyboardState;
	private KeyboardState? _previousKeyboardState;
	private float _timeSinceLastBackspace = 0f;
	private bool _isWaitingForInitialPause = false;

	public TextInput(Point position, Point size, TextInputStyle style, ITextListener textListener) : 
		base(position, size)
	{
		Style = style;
		TextListener = textListener;
	}

	public TextInput(Point size, TextInputStyle style, ITextListener textListener) : base(size)
	{
		Style = style;
		TextListener = textListener;
	}

	public string Text
	{
		get => _text;
		set
		{
			bool changed = _text != value;
			_text = value;
			if (changed)
				ValidateText();
		}
	}
	
	public string HintText { get; set; } = "Enter Here";
	public int MaxLength { get; set; } = 15;
	public bool ContainsValidString { get; set; } = false;
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
		_timeSinceLastBackspace += deltaTimeSeconds;
		
		HandleBackspaceKey();
		HandleEnterKey();

		if (_stringBuilder.Length >= MaxLength)
			return;

		TextListener.Listen(_previousKeyboardState, _currentKeyboardState, _stringBuilder);

		Text = _stringBuilder.ToString();
	}

	protected virtual void ValidateText()
	{
		ContainsValidString = true;
	}

	private void HandleBackspaceKey()
	{
		if (_stringBuilder.Length == 0) // Can't backspace if there are no characters
			return;

		if (Input.WasKeyPressed(Keys.Back)) // Initial key press
		{
			_stringBuilder.Remove(_stringBuilder.Length - 1, 1);
			_timeSinceLastBackspace = 0f;
			_isWaitingForInitialPause = true;
		}
		else if (Input.IsKeyDown(Keys.Back))
		{
			if (_isWaitingForInitialPause && _timeSinceLastBackspace >= InitialTimeBetweenBackspaces)
			{
				_stringBuilder.Remove(_stringBuilder.Length - 1, 1);
				_timeSinceLastBackspace = 0f;
				_isWaitingForInitialPause = false;
			}
			else if (!_isWaitingForInitialPause && _timeSinceLastBackspace >= TimeBetweenBackspaces)
			{
				_stringBuilder.Remove(_stringBuilder.Length - 1, 1);
				_timeSinceLastBackspace = 0f;
			}
		}
	}

	private void HandleEnterKey()
	{
		if (Inputs.Input.WasKeyPressed(Keys.Enter))
			_stringBuilder.Clear();
	}
}