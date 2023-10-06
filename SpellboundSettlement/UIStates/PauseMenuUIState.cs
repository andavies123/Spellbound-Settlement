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
	private const string OptionsButtonText = "Options";
	private const string MainMenuButtonText = "Main Menu";
	
	private static readonly Point ResumeButtonPosition = new(0, -100);
	private static readonly Point OptionsButtonPosition = new(0, 0);
	private static readonly Point MainMenuButtonPosition = new(0, 100);
	
	private static readonly Point ButtonSize = new(125, 75);

	private Button _resumeButton;
	private Button _optionsButton;
	private Button _mainMenuButton;

	public event Action ResumeButtonPressed;
	public event Action OptionsButtonPressed;
	public event Action MainMenuButtonPressed;
	
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
		
		_resumeButton = new Button(
			ResumeButtonPosition, 
			ButtonSize, 
			ResumeButtonText,
			buttonStyle,
			LayoutAnchor.MiddleCenter);

		_optionsButton = new Button(
			OptionsButtonPosition,
			ButtonSize,
			OptionsButtonText,
			buttonStyle,
			LayoutAnchor.MiddleCenter);

		_mainMenuButton = new Button(
			MainMenuButtonPosition,
			ButtonSize,
			MainMenuButtonText,
			buttonStyle,
			LayoutAnchor.MiddleCenter);
		
		_resumeButton.CalculateBounds(GameManager.Viewport.Bounds.Size);
		_optionsButton.CalculateBounds(GameManager.Viewport.Bounds.Size);
		_mainMenuButton.CalculateBounds(GameManager.Viewport.Bounds.Size);
		
		_resumeButton.MouseReleased += OnResumeButtonPressed;
		_optionsButton.MouseReleased += OnOptionsButtonPressed;
		_mainMenuButton.MouseReleased += OnMainMenuButtonPressed;
	}

	public void Update(float deltaTimeSeconds)
	{
		_resumeButton.Update();
		_optionsButton.Update();
		_mainMenuButton.Update();
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		_resumeButton.Draw(spriteBatch);
		_optionsButton.Draw(spriteBatch);
		_mainMenuButton.Draw(spriteBatch);
	}

	private void OnResumeButtonPressed()
	{
		ResumeButtonPressed?.Invoke();
	}

	private void OnOptionsButtonPressed()
	{
		OptionsButtonPressed?.Invoke();
	}

	private void OnMainMenuButtonPressed()
	{
		MainMenuButtonPressed?.Invoke();
	}
}