using System;
using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.Interfaces;
using Andavies.MonoGame.UI.LayoutGroups;
using Andavies.MonoGame.UI.StateMachines;
using Andavies.MonoGame.UI.UIElements;
using Andavies.SpellboundSettlement.Globals;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.SpellboundSettlement.UIStates.MainMenu;

public class MainMenuPlayUIState : IUIState
{
	private static readonly Point ButtonSize = new(175, 60);

	private readonly IUIStyleRepository _uiStyleRepository;
	private VerticalLayoutGroup _verticalLayoutGroup;
	private Button _newGameButton;
	private Button _loadGameButton;
	private Button _multiplayerButton;
	private Button _backButton;

	public MainMenuPlayUIState(IUIStyleRepository uiStyleRepository)
	{
		_uiStyleRepository = uiStyleRepository;
	}

	public event Action NewGameActionRequested;
	public event Action LoadGameActionRequested;
	public event Action MultiplayerActionRequested;
	public event Action BackActionRequested;
	
	public void Init() { }

	public void LateInit()
	{
		_verticalLayoutGroup = new VerticalLayoutGroup(Point.Zero, GameManager.Viewport.Bounds.Size)
		{
			Spacing = 100,
			ChildAnchor = HorizontalAnchor.Center,
			ForceExpandChildWidth = false
		};

		_newGameButton = new Button(ButtonSize, "New Game", _uiStyleRepository.DefaultButtonStyle);
		_loadGameButton = new Button(ButtonSize, "Load Game", _uiStyleRepository.DefaultButtonStyle);
		_multiplayerButton = new Button(ButtonSize, "Multiplayer", _uiStyleRepository.DefaultButtonStyle);
		_backButton = new Button(ButtonSize, "Back", _uiStyleRepository.DefaultButtonStyle);
		
		_verticalLayoutGroup.AddChildren(
			_newGameButton,
			_loadGameButton,
			_multiplayerButton,
			_backButton);
	}

	public void Start()
	{
		_newGameButton.MouseClicked += OnNewGameButtonClicked;
		_loadGameButton.MouseClicked += OnLoadGameButtonClicked;
		_multiplayerButton.MouseClicked += OnMultiplayerButtonClicked;
		_backButton.MouseClicked += OnBackButtonClicked;
	}

	public void Update(float deltaTimeSeconds) => _verticalLayoutGroup.Update(deltaTimeSeconds);
	public void Draw(SpriteBatch spriteBatch) => _verticalLayoutGroup.Draw(spriteBatch);

	public void Exit()
	{
		_newGameButton.MouseClicked -= OnNewGameButtonClicked;
		_loadGameButton.MouseClicked -= OnLoadGameButtonClicked;
		_multiplayerButton.MouseClicked -= OnMultiplayerButtonClicked;
		_backButton.MouseClicked -= OnBackButtonClicked;
	}

	private void OnNewGameButtonClicked(IUIElement uiElement) => NewGameActionRequested?.Invoke();
	private void OnLoadGameButtonClicked(IUIElement uiElement) => LoadGameActionRequested?.Invoke();
	private void OnMultiplayerButtonClicked(IUIElement uiElement) => MultiplayerActionRequested?.Invoke();
	private void OnBackButtonClicked(IUIElement uiElement) => BackActionRequested?.Invoke();
}