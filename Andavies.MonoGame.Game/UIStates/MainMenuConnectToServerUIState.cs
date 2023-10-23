using System.Collections.Generic;
using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.StateMachines;
using Andavies.MonoGame.UI.Styles;
using Andavies.MonoGame.UI.UIElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpellboundSettlement.UIStates;

public class MainMenuConnectToServerUIState : IUIState
{
	private static readonly Point ConnectButtonPosition = new(0, 100);
	private static readonly Point BackButtonPosition = new(0, 200);
	
	private static readonly Point ButtonSize = new(175, 60);

	private readonly ButtonBuilder _buttonBuilder = new();
	private List<UIElement> _uiElements;

	public Button ConnectButton { get; private set; }
	public Button BackButton { get; private set; }
	
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

		ConnectButton = _buttonBuilder
			.SetText("Connect")
			.SetStyle(buttonStyle)
			.SetPositionAndSize(ConnectButtonPosition, ButtonSize)
			.SetLayoutAnchor(LayoutAnchor.MiddleCenter)
			.Build();
		
		BackButton = _buttonBuilder
			.SetText("Back")
			.SetStyle(buttonStyle)
			.SetPositionAndSize(BackButtonPosition, ButtonSize)
			.SetLayoutAnchor(LayoutAnchor.MiddleCenter)
			.Build();
		
		_uiElements = new List<UIElement>
		{
			ConnectButton,
			BackButton
		};
		
		_uiElements.ForEach(uiElement => uiElement.CalculateBounds(GameManager.Viewport.Bounds.Size));
	}

	public void Update(float deltaTimeSeconds)
	{
		_uiElements.ForEach(uiElement => uiElement.Update());
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		_uiElements.ForEach(uiElement => uiElement.Draw(spriteBatch));
	}
}