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

public class MainMenuMainUIState : IUIState
{
	private static readonly Point ButtonSize = new(175, 60);

	private readonly IUIStyleCollection _uiStyleCollection;
	private VerticalLayoutGroup _verticalLayoutGroup;
	private Button _playButton;
	private Button _joinServerButton;
	private Button _createServerButton;
	private Button _optionsButton;
	private Button _quitButton;
	
	public MainMenuMainUIState(IUIStyleCollection uiStyleCollection)
	{
		_uiStyleCollection = uiStyleCollection;
	}

	public event Action PlayButtonClicked;
	public event Action JoinServerButtonClicked;
	public event Action CreateServerButtonClicked;
	public event Action OptionsButtonClicked;
	public event Action QuitButtonClicked;

	public void Init() { }

	public void LateInit()
	{
		_verticalLayoutGroup = new VerticalLayoutGroup(Point.Zero, GameManager.Viewport.Bounds.Size)
		{
			Spacing = 100,
			ChildAnchor = HorizontalAnchor.Center,
			ForceExpandChildWidth = false
		};

		_playButton = new Button(ButtonSize, "Play", _uiStyleCollection.DefaultButtonStyle);
		_joinServerButton = new Button(ButtonSize, "Join Server", _uiStyleCollection.DefaultButtonStyle);
		_createServerButton = new Button(ButtonSize, "Create Server", _uiStyleCollection.DefaultButtonStyle);
		_optionsButton = new Button(ButtonSize, "Options", _uiStyleCollection.DefaultButtonStyle);
		_quitButton = new Button(ButtonSize, "Quit", _uiStyleCollection.DefaultButtonStyle);
		
		_verticalLayoutGroup.AddChildren(
			_playButton,
			_joinServerButton,
			_createServerButton,
			_optionsButton,
			_quitButton);
	}

	public void Start()
	{
		_playButton.MouseClicked += OnPlayButtonMouseClicked;
		_joinServerButton.MouseClicked += OnJoinServerButtonMouseClicked;
		_createServerButton.MouseClicked += OnCreateServerButtonMouseClicked;
		_optionsButton.MouseClicked += OnOptionsButtonMouseClicked;
		_quitButton.MouseClicked += OnQuitButtonMouseClicked;
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
		_playButton.MouseClicked -= OnPlayButtonMouseClicked;
		_joinServerButton.MouseClicked -= OnJoinServerButtonMouseClicked;
		_createServerButton.MouseClicked -= OnCreateServerButtonMouseClicked;
		_optionsButton.MouseClicked -= OnOptionsButtonMouseClicked;
		_quitButton.MouseClicked -= OnQuitButtonMouseClicked;
	}

	private void OnPlayButtonMouseClicked(IUIElement _) => PlayButtonClicked?.Invoke();
	private void OnJoinServerButtonMouseClicked(IUIElement _) => JoinServerButtonClicked?.Invoke();
	private void OnCreateServerButtonMouseClicked(IUIElement _) => CreateServerButtonClicked?.Invoke();
	private void OnOptionsButtonMouseClicked(IUIElement _) => OptionsButtonClicked?.Invoke();
	private void OnQuitButtonMouseClicked(IUIElement _) => QuitButtonClicked?.Invoke();
}