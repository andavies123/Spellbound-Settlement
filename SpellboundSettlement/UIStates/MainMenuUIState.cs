using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Enums;
using UI.StateMachines;
using UI.Styles;
using UI.UIElements;

namespace SpellboundSettlement.UIStates;

public class MainMenuUIState : IUIState
{
	private const string PlayButtonText = "Play";
	private const string OptionsButtonText = "Options";
	private const string QuitButtonText = "Quit";
	
	private static readonly Point PlayButtonPosition = new(0, -100);
	private static readonly Point OptionsButtonPosition = new(0, 0);
	private static readonly Point QuitButtonPosition = new(0, 100);
	
	private static readonly Point ButtonSize = new(125, 75);

	private Button _playButton;
	private Button _optionsButton;
	private Button _quitButton;
	
	public event Action PlayButtonPressed
	{
		add => _playButton.MouseReleased += value;
		remove => _playButton.MouseReleased -= value;
	}
	
	public event Action OptionsButtonPressed
	{
		add => _optionsButton.MouseReleased += value;
		remove => _optionsButton.MouseReleased -= value;
	}
	
	public event Action QuitButtonPressed
	{
		add => _quitButton.MouseReleased += value;
		remove => _quitButton.MouseReleased -= value;
	}
	
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
		
		_playButton = new Button(
			PlayButtonPosition, 
			ButtonSize, 
			PlayButtonText,
			buttonStyle,
			LayoutAnchor.MiddleCenter);

		_optionsButton = new Button(
			OptionsButtonPosition,
			ButtonSize,
			OptionsButtonText,
			buttonStyle,
			LayoutAnchor.MiddleCenter);

		_quitButton = new Button(
			QuitButtonPosition,
			ButtonSize,
			QuitButtonText,
			buttonStyle,
			LayoutAnchor.MiddleCenter);
		
		_playButton.CalculateBounds(GameManager.Viewport.Bounds.Size);
		_optionsButton.CalculateBounds(GameManager.Viewport.Bounds.Size);
		_quitButton.CalculateBounds(GameManager.Viewport.Bounds.Size);
	}

	public void Update(float deltaTimeSeconds)
	{
		_playButton.Update();
		_optionsButton.Update();
		_quitButton.Update();
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		_playButton.Draw(spriteBatch);
		_optionsButton.Draw(spriteBatch);
		_quitButton.Draw(spriteBatch);
	}
}