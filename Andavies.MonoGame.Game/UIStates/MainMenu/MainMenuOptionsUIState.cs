using System;
using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.Interfaces;
using Andavies.MonoGame.UI.LayoutGroups;
using Andavies.MonoGame.UI.StateMachines;
using Andavies.MonoGame.UI.UIElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.Globals;

namespace SpellboundSettlement.UIStates.MainMenu;

public class MainMenuOptionsUIState : IUIState
{
	private static readonly Point ButtonSize = new(175, 60);
	
	private readonly IUIStyleCollection _uiStyleCollection;
	private VerticalLayoutGroup _mainLayoutGroup;
	private Button _backButton;

	public MainMenuOptionsUIState(IUIStyleCollection uiStyleCollection)
	{
		_uiStyleCollection = uiStyleCollection;
	}

	public event Action BackButtonClicked;
	
	public void Init() { }

	public void LateInit()
	{
		_mainLayoutGroup = new VerticalLayoutGroup(GameManager.Viewport.Bounds)
		{
			Spacing = 100,
			ChildAnchor = HorizontalAnchor.Center,
			ForceExpandChildWidth = false
		};
		
		_backButton = new Button(ButtonSize, "Back", _uiStyleCollection.DefaultButtonStyle);
		
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