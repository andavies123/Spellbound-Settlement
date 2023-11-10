using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.LayoutGroups;
using Andavies.MonoGame.UI.StateMachines;
using Andavies.MonoGame.UI.UIElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.Globals;

namespace SpellboundSettlement.UIStates.MainMenu;

public class MainMenuMainUIState : IUIState
{
	private static readonly Point ButtonSize = new(175, 60);

	private readonly IUIStyleCollection _uiStyleCollection;
	private VerticalLayoutGroup _verticalLayoutGroup;

	public MainMenuMainUIState(IUIStyleCollection uiStyleCollection)
	{
		_uiStyleCollection = uiStyleCollection;
	}

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

		PlayButton = new Button(ButtonSize, "Play", _uiStyleCollection.DefaultButtonStyle);
		ConnectToServerButton = new Button(ButtonSize, "Connect to Server", _uiStyleCollection.DefaultButtonStyle);
		CreateServerButton = new Button(ButtonSize, "Create Server", _uiStyleCollection.DefaultButtonStyle);
		OptionsButton = new Button(ButtonSize, "Options", _uiStyleCollection.DefaultButtonStyle);
		QuitButton = new Button(ButtonSize, "Quit", _uiStyleCollection.DefaultButtonStyle);
		
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