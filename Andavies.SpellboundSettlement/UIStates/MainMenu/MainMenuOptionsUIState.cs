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

namespace Andavies.SpellboundSettlement.UIStates.MainMenu;

public class MainMenuOptionsUIState : IUIState
{
	private static readonly Point ButtonSize = new(175, 60);

	private readonly IInputManager _inputManager;
	private readonly IUIStyleRepository _uiStyleCollection;
	private VerticalLayoutGroup _mainLayoutGroup;
	private Button _backButton;

	public MainMenuOptionsUIState(IInputManager inputManager, IUIStyleRepository uiStyleCollection)
	{
		_inputManager = inputManager ?? throw new ArgumentNullException(nameof(inputManager));
		_uiStyleCollection = uiStyleCollection ?? throw new ArgumentNullException(nameof(uiStyleCollection));
	}

	public event Action BackButtonClicked;
	
	public void Init() { }

	public void LateInit()
	{
		_mainLayoutGroup = new VerticalLayoutGroup(_inputManager, GameManager.Viewport.Bounds)
		{
			Spacing = 100,
			ChildAnchor = HorizontalAnchor.Center,
			ForceExpandChildWidth = false
		};
		
		_backButton = new Button(_inputManager, ButtonSize, "Back", _uiStyleCollection.DefaultButtonStyle);
		
		_mainLayoutGroup.AddChildren(_backButton);
	}

	public void Start()
	{
		_backButton.MouseClicked += OnBackButtonMouseClicked;
	}

	public void Update(float deltaTimeSeconds) => _mainLayoutGroup.Update(deltaTimeSeconds);
	public void Draw(SpriteBatch spriteBatch) => _mainLayoutGroup.Draw(spriteBatch);

	public void Exit()
	{
		_backButton.MouseClicked -= OnBackButtonMouseClicked;
	}

	private void OnBackButtonMouseClicked(IUIElement _) => BackButtonClicked?.Invoke();
}