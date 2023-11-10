using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.LayoutGroups;
using Andavies.MonoGame.UI.StateMachines;
using Andavies.MonoGame.UI.UIElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.Globals;

namespace SpellboundSettlement.UIStates.PauseMenu;

public class PauseMenuUIState : IUIState
{
	private static readonly Point ButtonSize = new(125, 75);

	private readonly IUIStyleCollection _uiStyleCollection;
	private VerticalLayoutGroup _verticalLayoutGroup;

	public PauseMenuUIState(IUIStyleCollection uiStyleCollection)
	{
		_uiStyleCollection = uiStyleCollection;
	}

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

		ResumeButton = new Button(ButtonSize, "Resume", _uiStyleCollection.DefaultButtonStyle);
		OptionsButton = new Button(ButtonSize, "Options", _uiStyleCollection.DefaultButtonStyle);
		MainMenuButton = new Button(ButtonSize, "Main Menu", _uiStyleCollection.DefaultButtonStyle);
		
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