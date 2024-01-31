using System;
using Andavies.MonoGame.Inputs;
using Andavies.MonoGame.Inputs.InputListeners;
using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.Interfaces;
using Andavies.MonoGame.UI.LayoutGroups;
using Andavies.MonoGame.UI.StateMachines;
using Andavies.MonoGame.UI.UIElements;
using Andavies.MonoGame.UI.UIElements.TextInputs;
using Andavies.SpellboundSettlement.Globals;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.SpellboundSettlement.UIStates.MainMenu;

public class MainMenuNewGameUIState : IUIState
{
	private static readonly Point ButtonSize = new(175, 60);

	private readonly IInputManager _inputManager;
	private readonly IUIStyleRepository _uiStyleRepository;
	private VerticalLayoutGroup _verticalLayoutGroup;
	private Label _worldNameLabel;
	private TextInput _worldNameTextInput;
	private Button _createWorldButton;
	private Button _backButton;

	public MainMenuNewGameUIState(IInputManager inputManager, IUIStyleRepository uiStyleRepository)
	{
		_inputManager = inputManager ?? throw new ArgumentNullException(nameof(inputManager));
		_uiStyleRepository = uiStyleRepository ?? throw new ArgumentNullException(nameof(uiStyleRepository));
	}

	public event Action CreateWorldActionRequested;
	public event Action BackActionRequested;
	
	public void Init() { }

	public void LateInit()
	{
		_verticalLayoutGroup = new VerticalLayoutGroup(_inputManager, Point.Zero, GameManager.Viewport.Bounds.Size)
		{
			Spacing = 100,
			ChildAnchor = HorizontalAnchor.Center,
			ForceExpandChildWidth = false
		};

		_worldNameLabel = new Label(_inputManager, ButtonSize, "World Name:", _uiStyleRepository.DefaultLabelStyle);
		_worldNameTextInput = new TextInput(_inputManager, ButtonSize, _uiStyleRepository.DefaultTextInputStyle, new NumberDecimalInputListener(_inputManager));
		_createWorldButton = new Button(_inputManager, ButtonSize, "Create World", _uiStyleRepository.DefaultButtonStyle);
		_backButton = new Button(_inputManager, ButtonSize, "Back", _uiStyleRepository.DefaultButtonStyle);
		
		_verticalLayoutGroup.AddChildren(
			_worldNameLabel,
			_worldNameTextInput,
			_createWorldButton,
			_backButton);
	}

	public void Start()
	{
		_createWorldButton.MouseClicked += OnCreateWorldButtonClicked;
		_backButton.MouseClicked += OnBackButtonClicked;
	}

	public void Update(float deltaTimeSeconds) => _verticalLayoutGroup.Update(deltaTimeSeconds);
	public void Draw(SpriteBatch spriteBatch) => _verticalLayoutGroup.Draw(spriteBatch);

	public void Exit()
	{
		_createWorldButton.MouseClicked -= OnCreateWorldButtonClicked;
		_backButton.MouseClicked -= OnBackButtonClicked;
	}

	private void OnCreateWorldButtonClicked(IUIElement uiElement) => CreateWorldActionRequested?.Invoke();
	private void OnBackButtonClicked(IUIElement uiElement) => BackActionRequested?.Invoke();
}