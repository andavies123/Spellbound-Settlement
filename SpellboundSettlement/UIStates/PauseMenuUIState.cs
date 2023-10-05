using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Enums;
using UI.StateMachines;
using UI.Styles;
using UI.UIElements;

namespace SpellboundSettlement.UIStates;

public class PauseMenuUIState : IUIState
{
	private const string ResumeButtonText = "Resume";
	private static readonly Point ResumeButtonPosition = new(0, 0);
	private static readonly Point ResumeButtonSize = new(75, 25);

	private Button _resumeButton;

	public event Action ResumeButtonPressed;
	
	public void Init() { }

	public void LateInit()
	{
		ButtonStyle pauseMenuButtonStyle = new()
		{
			Font = GameManager.Font,
			BackgroundColor = Color.LightSlateGray,
			HoverBackgroundColor = Color.SlateGray,
			MousePressedBackgroundColor = Color.DarkSlateGray,
			DisabledBackgroundColor = Color.Red,
			BackgroundTexture = GameManager.Texture
		};
		
		_resumeButton = new Button(
			ResumeButtonPosition, 
			ResumeButtonSize, 
			ResumeButtonText,
			pauseMenuButtonStyle,
			LayoutAnchor.MiddleCenter);
		
		_resumeButton.CalculateBounds(GameManager.Viewport.Bounds.Size);
		_resumeButton.MouseReleased += OnResumeButtonPressed;
	}

	public void Update(float deltaTimeSeconds)
	{
		_resumeButton.Update();
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		_resumeButton.Draw(spriteBatch);
	}

	private void OnResumeButtonPressed()
	{
		ResumeButtonPressed?.Invoke();
	}
}