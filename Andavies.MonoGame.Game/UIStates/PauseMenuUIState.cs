using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.LayoutGroups;
using Andavies.MonoGame.UI.StateMachines;
using Andavies.MonoGame.UI.Styles;
using Andavies.MonoGame.UI.UIElements;
using SpellboundSettlement.Globals;

namespace SpellboundSettlement.UIStates;

public class PauseMenuUIState : IUIState
{
	private static readonly Point ResumeButtonPosition = new(0, -100);
	private static readonly Point OptionsButtonPosition = new(0, 0);
	private static readonly Point MainMenuButtonPosition = new(0, 100);
	
	private static readonly Point ButtonSize = new(125, 75);

	private VerticalLayoutGroup _verticalLayoutGroup;

	public Button ResumeButton { get; private set; }
	public Button OptionsButton { get; private set; }
	public Button MainMenuButton { get; private set; }
	
	public void Init() { }

	public void LateInit()
	{
		_verticalLayoutGroup = new VerticalLayoutGroup(
			new Rectangle(0, 0, GameManager.Viewport.Width, GameManager.Viewport.Height))
		{
			ChildAnchor = HorizontalAnchor.Center,
			ForceExpandChildWidth = false,
			Spacing = 200
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

		ResumeButton = new Button(ResumeButtonPosition, ButtonSize, "Resume", buttonStyle);
		OptionsButton = new Button(OptionsButtonPosition, ButtonSize, "Options", buttonStyle);
		MainMenuButton = new Button(MainMenuButtonPosition, ButtonSize, "Main Menu", buttonStyle);
		
		_verticalLayoutGroup.AddChildren(
			ResumeButton, 
			OptionsButton, 
			MainMenuButton);
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