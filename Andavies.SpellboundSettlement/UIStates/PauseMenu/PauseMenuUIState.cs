using System;
using Andavies.MonoGame.Inputs;
using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.Interfaces;
using Andavies.MonoGame.UI.LayoutGroups;
using Andavies.MonoGame.UI.StateMachines;
using Andavies.MonoGame.UI.UIElements;
using Andavies.SpellboundSettlement.Globals;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.SpellboundSettlement.UIStates.PauseMenu;

public class PauseMenuUIState : IUIState
{
	private static readonly Point ButtonSize = new(125, 75);

	private readonly IInputManager _inputManager;
	private readonly IUIStyleRepository _uiStyleCollection;
	private VerticalLayoutGroup _verticalLayoutGroup;

	private Button _resumeButton;
	private Button _optionsButton;
	private Button _mainMenuButton;

	public PauseMenuUIState(IInputManager inputManager, IUIStyleRepository uiStyleCollection)
	{
		_inputManager = inputManager ?? throw new ArgumentNullException(nameof(inputManager));
		_uiStyleCollection = uiStyleCollection ?? throw new ArgumentNullException(nameof(uiStyleCollection));
	}

	public event Action ResumeButtonClicked;
	public event Action OptionsButtonClicked;
	public event Action MainMenuButtonClicked;
	
	public void Init() { }

	public void LateInit()
	{
		_verticalLayoutGroup = new VerticalLayoutGroup(_inputManager, 
			new Rectangle(0, 0, GameManager.Viewport.Width, GameManager.Viewport.Height))
		{
			ChildAnchor = HorizontalAnchor.Center,
			ForceExpandChildWidth = false,
			Spacing = 200
		};

		_resumeButton = new Button(_inputManager, ButtonSize, "Resume", _uiStyleCollection.DefaultButtonStyle);
		_optionsButton = new Button(_inputManager, ButtonSize, "Options", _uiStyleCollection.DefaultButtonStyle);
		_mainMenuButton = new Button(_inputManager, ButtonSize, "Main Menu", _uiStyleCollection.DefaultButtonStyle);
		
		_verticalLayoutGroup.AddChildren(
			_resumeButton, 
			_optionsButton, 
			_mainMenuButton);
	}

	public void Start()
	{
		_resumeButton.MouseClicked += OnResumeButtonMouseClicked;
		_optionsButton.MouseClicked += OnOptionsButtonMouseClicked;
		_mainMenuButton.MouseClicked += OnMainMenuButtonMouseClicked;
	}

	public void Update(float deltaTimeSeconds)
	{
		_verticalLayoutGroup.Update(deltaTimeSeconds);
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		_verticalLayoutGroup.Draw(spriteBatch);
	}

	public void Exit()
	{
		_resumeButton.MouseClicked -= OnResumeButtonMouseClicked;
		_optionsButton.MouseClicked -= OnOptionsButtonMouseClicked;
		_mainMenuButton.MouseClicked -= OnMainMenuButtonMouseClicked;
	}

	private void OnResumeButtonMouseClicked(IUIElement uiElement) => ResumeButtonClicked?.Invoke();
	private void OnOptionsButtonMouseClicked(IUIElement uiElement) => OptionsButtonClicked?.Invoke();
	private void OnMainMenuButtonMouseClicked(IUIElement uiElement) => MainMenuButtonClicked?.Invoke();
}