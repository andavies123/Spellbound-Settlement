using System.Collections.Generic;
using Andavies.MonoGame.UI.Builders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.LayoutGroups;
using Andavies.MonoGame.UI.StateMachines;
using Andavies.MonoGame.UI.Styles;
using Andavies.MonoGame.UI.UIElements;
using SpellboundSettlement.Globals;

namespace SpellboundSettlement.UIStates;

public class MainMenuMainUIState : IUIState
{
	private static readonly Point PlayButtonPosition = new(0, -200);
	private static readonly Point ConnectToServerButtonPosition = new(0, -100);
	private static readonly Point CreateServerButtonPosition = new(0, 0);
	private static readonly Point OptionsButtonPosition = new(0, 100);
	private static readonly Point QuitButtonPosition = new(0, 200);
	
	private static readonly Point ButtonSize = new(175, 60);

	private readonly ButtonBuilder _buttonBuilder = new();
	private VerticalLayoutGroup _verticalLayoutGroup;
	private List<UIElement> _uiElements;

	public Button PlayButton { get; private set; }
	public Button ConnectToServerButton { get; private set; }
	public Button CreateServerButton { get; private set; }
	public Button OptionsButton { get; private set; }
	public Button QuitButton { get; private set; }

	public void Init() { }

	public void LateInit()
	{
		_verticalLayoutGroup = new VerticalLayoutGroup
		{
			Spacing = 100,
			ChildAnchor = HorizontalAnchor.Center,
			ForceExpandChildWidth = false,
			Bounds = new Rectangle(0, 0, GameManager.Viewport.Width, GameManager.Viewport.Height)
		};
		
		ButtonStyle buttonStyle = new()
		{
			Font = GlobalFonts.DefaultFont,
			BackgroundColor = Color.LightSlateGray,
			HoverBackgroundColor = Color.SlateGray,
			MousePressedBackgroundColor = Color.DarkSlateGray,
			DisabledBackgroundColor = Color.Red,
			BackgroundTexture = GameManager.Texture
		};

		PlayButton = _buttonBuilder
			.SetText("Play")
			.SetStyle(buttonStyle)
			.SetPositionAndSize(PlayButtonPosition, ButtonSize)
			.Build();

		ConnectToServerButton = _buttonBuilder
			.SetText("Connect to Server")
			.SetStyle(buttonStyle)
			.SetPositionAndSize(ConnectToServerButtonPosition, ButtonSize)
			.Build();

		CreateServerButton = _buttonBuilder
			.SetText("Create Server")
			.SetStyle(buttonStyle)
			.SetPositionAndSize(CreateServerButtonPosition, ButtonSize)
			.Build();

		OptionsButton = _buttonBuilder
			.SetText("Options")
			.SetStyle(buttonStyle)
			.SetPositionAndSize(OptionsButtonPosition, ButtonSize)
			.Build();

		QuitButton = _buttonBuilder
			.SetText("Quit")
			.SetStyle(buttonStyle)
			.SetPositionAndSize(QuitButtonPosition, ButtonSize)
			.Build();
		
		_verticalLayoutGroup.AddUIElements(
			PlayButton,
			ConnectToServerButton,
			CreateServerButton,
			OptionsButton, 
			QuitButton);
		
		_uiElements = new List<UIElement>
		{
			PlayButton,
			ConnectToServerButton,
			CreateServerButton,
			OptionsButton,
			QuitButton
		};
	}

	public void Update(float deltaTimeSeconds)
	{
		_uiElements.ForEach(uiElement => uiElement.Update());
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		_uiElements.ForEach(uiElement => uiElement.Draw(spriteBatch));
	}

	public void Exit() { }
}