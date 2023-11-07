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
	private static readonly Point ButtonSize = new(175, 60);

	private VerticalLayoutGroup _verticalLayoutGroup;

	public Button PlayButton { get; private set; }
	public Button ConnectToServerButton { get; private set; }
	public Button CreateServerButton { get; private set; }
	public Button OptionsButton { get; private set; }
	public Button QuitButton { get; private set; }

	public void Init() { }

	public void LateInit()
	{
		_verticalLayoutGroup = new VerticalLayoutGroup(Point.Zero, GameManager.Viewport.Bounds.Size)
		{
			Spacing = 100,
			ChildAnchor = HorizontalAnchor.Center,
			ForceExpandChildWidth = false
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

		PlayButton = new Button(ButtonSize, "Play", buttonStyle);
		ConnectToServerButton = new Button(ButtonSize, "Connect to Server", buttonStyle);
		CreateServerButton = new Button(ButtonSize, "Create Server", buttonStyle);
		OptionsButton = new Button(ButtonSize, "Options", buttonStyle);
		QuitButton = new Button(ButtonSize, "Quit", buttonStyle);
		
		_verticalLayoutGroup.AddChildren(
			PlayButton,
			ConnectToServerButton,
			CreateServerButton,
			OptionsButton, 
			QuitButton);
	}

	public void Update(float deltaTimeSeconds)
	{
		_verticalLayoutGroup.Update(deltaTimeSeconds);
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		_verticalLayoutGroup.Draw(spriteBatch);
	}

	public void Exit() { }
}