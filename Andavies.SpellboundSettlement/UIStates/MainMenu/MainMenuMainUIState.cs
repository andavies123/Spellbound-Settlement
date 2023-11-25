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

public class MainMenuMainUIState : IUIState
{
	private static readonly Point ButtonSize = new(175, 60);

	private readonly IUIStyleRepository _uiStyleRepository;
	private VerticalLayoutGroup _verticalLayoutGroup;
	private Button _playButton;
	private Button _joinServerButton;
	private Button _createServerButton;
	private Button _optionsButton;
	private Button _quitButton;
	
	public MainMenuMainUIState(IUIStyleRepository uiStyleRepository)
	{
		_uiStyleRepository = uiStyleRepository;
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

		_playButton = new Button(ButtonSize, "Play", _uiStyleRepository.DefaultButtonStyle);
		_joinServerButton = new Button(ButtonSize, "Join Server", _uiStyleRepository.DefaultButtonStyle);
		_createServerButton = new Button(ButtonSize, "Create Server", _uiStyleRepository.DefaultButtonStyle);
		_optionsButton = new Button(ButtonSize, "Options", _uiStyleRepository.DefaultButtonStyle);
		_quitButton = new Button(ButtonSize, "Quit", _uiStyleRepository.DefaultButtonStyle);
		
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