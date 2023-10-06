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
	private const string SettingsButtonText = "Settings";
	private const string QuitButtonText = "Quit";
	
	private static readonly Point ResumeButtonPosition = new(0, -100);
	private static readonly Point SettingsButtonPosition = new(0, 0);
	private static readonly Point QuitButtonPosition = new(0, 100);
	
	private static readonly Point PauseMenuButtonSize = new(125, 75);

	private Button _resumeButton;
	private Button _settingsButton;
	private Button _quitButton;

	public event Action ResumeButtonPressed;
	public event Action SettingsButtonPressed;
	public event Action QuitButtonPressed;
	
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
			PauseMenuButtonSize, 
			ResumeButtonText,
			pauseMenuButtonStyle,
			LayoutAnchor.MiddleCenter);

		_settingsButton = new Button(
			SettingsButtonPosition,
			PauseMenuButtonSize,
			SettingsButtonText,
			pauseMenuButtonStyle,
			LayoutAnchor.MiddleCenter);

		_quitButton = new Button(
			QuitButtonPosition,
			PauseMenuButtonSize,
			QuitButtonText,
			pauseMenuButtonStyle,
			LayoutAnchor.MiddleCenter);
		
		_resumeButton.CalculateBounds(GameManager.Viewport.Bounds.Size);
		_settingsButton.CalculateBounds(GameManager.Viewport.Bounds.Size);
		_quitButton.CalculateBounds(GameManager.Viewport.Bounds.Size);
		
		_resumeButton.MouseReleased += OnResumeButtonPressed;
		_settingsButton.MouseReleased += OnSettingsButtonPressed;
		_quitButton.MouseReleased += OnQuitButtonPressed;
	}

	public void Update(float deltaTimeSeconds)
	{
		_resumeButton.Update();
		_settingsButton.Update();
		_quitButton.Update();
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		_resumeButton.Draw(spriteBatch);
		_settingsButton.Draw(spriteBatch);
		_quitButton.Draw(spriteBatch);
	}

	private void OnResumeButtonPressed()
	{
		ResumeButtonPressed?.Invoke();
	}

	private void OnSettingsButtonPressed()
	{
		SettingsButtonPressed?.Invoke();
	}

	private void OnQuitButtonPressed()
	{
		QuitButtonPressed?.Invoke();
	}
}