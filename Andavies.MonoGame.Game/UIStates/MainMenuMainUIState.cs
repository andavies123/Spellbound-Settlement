using System.Collections.Generic;
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

		PlayButton = new Button(PlayButtonPosition, ButtonSize, "Play", buttonStyle);
		ConnectToServerButton = new Button(ConnectToServerButtonPosition, ButtonSize, "Connect to Server", buttonStyle);
		CreateServerButton = new Button(CreateServerButtonPosition, ButtonSize, "Create Server", buttonStyle);
		OptionsButton = new Button(OptionsButtonPosition, ButtonSize, "Options", buttonStyle);
		QuitButton = new Button(QuitButtonPosition, ButtonSize, "Quit", buttonStyle);
		
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