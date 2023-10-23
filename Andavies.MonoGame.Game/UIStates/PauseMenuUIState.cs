using System;
using Andavies.MonoGame.UI.Builders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.StateMachines;
using Andavies.MonoGame.UI.Styles;
using Andavies.MonoGame.UI.UIElements;

namespace SpellboundSettlement.UIStates;

public class PauseMenuUIState : IUIState
{
	private static readonly Point ResumeButtonPosition = new(0, -100);
	private static readonly Point OptionsButtonPosition = new(0, 0);
	private static readonly Point MainMenuButtonPosition = new(0, 100);
	
	private static readonly Point ButtonSize = new(125, 75);

	private readonly ButtonBuilder _buttonBuilder = new();

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

		_resumeButton = _buttonBuilder
			.SetText("Resume")
			.SetStyle(buttonStyle)
			.SetPositionAndSize(ResumeButtonPosition, ButtonSize)
			.SetLayoutAnchor(LayoutAnchor.MiddleCenter)
			.Build();

		_optionsButton = _buttonBuilder
			.SetText("Options")
			.SetStyle(buttonStyle)
			.SetPositionAndSize(OptionsButtonPosition, ButtonSize)
			.SetLayoutAnchor(LayoutAnchor.MiddleCenter)
			.Build();

		_mainMenuButton = _buttonBuilder
			.SetText("Main Menu")
			.SetStyle(buttonStyle)
			.SetPositionAndSize(MainMenuButtonPosition, ButtonSize)
			.SetLayoutAnchor(LayoutAnchor.MiddleCenter)
			.Build();
		
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