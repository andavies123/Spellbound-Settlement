using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Enums;
using UI.StateMachines;
using UI.Styles;
using UI.UIElements;

namespace SpellboundSettlement.UIStates;

public class GameplayUIState : IUIState
{
	private const string PauseButtonText = "Pause";
	private const LayoutAnchor PauseButtonAnchor = LayoutAnchor.TopRight;
	private static readonly Point PauseButtonPosition = new(-20, 20);
	private static readonly Point PauseButtonSize = new(75, 25);
	
	private Button _pauseButton;

	public event Action PauseButtonPressed;

	public void Init() { }

	public void LateInit()
	{
		ButtonStyle buttonStyle = new()
		{
			Font = GameManager.Font,
			BackgroundColor = Color.LightSlateGray,
			HoverBackgroundColor = Color.SlateGray,
			MousePressedBackgroundColor = Color.DarkSlateGray,
			DisabledBackgroundColor = Color.Red,
			BackgroundTexture = GameManager.Texture
		};

		_pauseButton = new Button(PauseButtonPosition, PauseButtonSize, PauseButtonText, buttonStyle, PauseButtonAnchor);
		_pauseButton.CalculateBounds(GameManager.Viewport.Bounds.Size);
		_pauseButton.MousePressed += RaisePauseButtonPressed;
	}

	public void Update(float deltaTimeSeconds)
	{
		_pauseButton.Update();
	}
    
	public void Draw(SpriteBatch spriteBatch)
	{
		_pauseButton.Draw(spriteBatch);
	}

	private void RaisePauseButtonPressed() => PauseButtonPressed?.Invoke();
}

public class PauseMenuUIState : IUIState
{
	public void Init() { }
	public void LateInit() { }
	public void Update(float deltaTimeSeconds) { }
	public void Draw(SpriteBatch spriteBatch) { }
}