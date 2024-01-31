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
	
	private float _timeSinceLastBackspace = 0f;
	private bool _isWaitingForInitialPause = false;
	private Cursor _cursor = new();

	public TextInput(IInputManager inputManager, Point position, Point size, TextInputStyle style, IInputListener inputListener) : 
		base(inputManager, position, size)
	{
		Style = style;
		InputListener = inputListener;
	}

	public TextInput(IInputManager inputManager, Point size, TextInputStyle style, IInputListener inputListener) : base(inputManager, size)
	{
		Style = style;
		InputListener = inputListener;
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
	public IInputListener InputListener { get; set; }
	public TextInputStyle Style { get; set; }

	public void Clear()
	{
		InputListener.ResetListener();
		Text = InputListener.Text;
	}

	public override void Update(float deltaTimeSeconds)
	{
		base.Update(deltaTimeSeconds);

		_cursor.IsVisible = HasFocus;
		
		if (!HasFocus)
			return;

		_cursor.Update(deltaTimeSeconds);
		_timeSinceLastBackspace += deltaTimeSeconds;
		
		HandleBackspaceKey();
		HandleEnterKey();

		if (InputListener.Length >= MaxLength)
			return;

		InputListener.Listen();

		Text = InputListener.Text;
	}
	
	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);
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
		_cursor.Draw(spriteBatch, textPosition + new Vector2(textSize.X + font?.MeasureString("a").X/2 ?? 5, 0), textSize.Y);
	}

	protected virtual void ValidateText()
	{
		ContainsValidString = true;
	}

	private void HandleBackspaceKey()
	{
		if (InputManager.WasKeyPressed(Keys.Back)) // Initial key press
		{
			InputListener.RemoveLastCharacter();
			_timeSinceLastBackspace = 0f;
			_isWaitingForInitialPause = true;
		}
		else if (InputManager.IsKeyDown(Keys.Back))
		{
			if (_isWaitingForInitialPause && _timeSinceLastBackspace >= InitialTimeBetweenBackspaces)
			{
				InputListener.RemoveLastCharacter();
				_timeSinceLastBackspace = 0f;
				_isWaitingForInitialPause = false;
			}
			else if (!_isWaitingForInitialPause && _timeSinceLastBackspace >= TimeBetweenBackspaces)
			{
				InputListener.RemoveLastCharacter();
				_timeSinceLastBackspace = 0f;
			}
		}
	}

	private void HandleEnterKey()
	{
		if (InputManager.WasKeyPressed(Keys.Enter))
			InputListener.ResetListener();
	}
}