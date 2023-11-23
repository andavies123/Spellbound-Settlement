using System;
using Andavies.MonoGame.Inputs.InputListeners;
using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.Interfaces;
using Andavies.MonoGame.UI.LayoutGroups;
using Andavies.MonoGame.UI.StateMachines;
using Andavies.MonoGame.UI.UIElements;
using Andavies.MonoGame.UI.UIElements.TextInputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.Globals;

namespace SpellboundSettlement.UIStates.MainMenu;

public class MainMenuNewGameUIState : IUIState
{
	private static readonly Point ButtonSize = new(175, 60);

	private readonly IUIStyleRepository _uiStyleRepository;
	private VerticalLayoutGroup _verticalLayoutGroup;
	private Label _worldNameLabel;
	private TextInput _worldNameTextInput;
	private Button _createWorldButton;
	private Button _backButton;

	public MainMenuNewGameUIState(IUIStyleRepository uiStyleRepository)
	{
		_uiStyleRepository = uiStyleRepository;
	}

	public event Action CreateWorldActionRequested;
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

		_worldNameLabel = new Label(ButtonSize, "World Name:", _uiStyleRepository.DefaultLabelStyle);
		_worldNameTextInput = new TextInput(ButtonSize, _uiStyleRepository.DefaultTextInputStyle, new NumbersOnlyTextListener());
		_createWorldButton = new Button(ButtonSize, "Create World", _uiStyleRepository.DefaultButtonStyle);
		_backButton = new Button(ButtonSize, "Back", _uiStyleRepository.DefaultButtonStyle);
		
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