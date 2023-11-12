using System;
using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.Interfaces;
using Andavies.MonoGame.UI.LayoutGroups;
using Andavies.MonoGame.UI.StateMachines;
using Andavies.MonoGame.UI.UIElements;
using Andavies.MonoGame.UI.UIElements.TextInputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.Globals;
using Label = Andavies.MonoGame.UI.UIElements.Label;

namespace SpellboundSettlement.UIStates.MainMenu;

public class MainMenuJoinServerUIState : IUIState
{
	private static readonly Point ButtonSize = new(175, 60);

	private readonly IUIStyleCollection _uiStyleCollection;
	private VerticalLayoutGroup _verticalGroup;
	private IUIElement _focusedUIElement;

	private Label _ipAddressLabel;
	private TextInput _ipAddressTextInput;
	private Button _connectButton;
	private Button _backButton;

	public MainMenuJoinServerUIState(IUIStyleCollection uiStyleCollection)
	{
		_uiStyleCollection = uiStyleCollection;
	}

	public event Action ConnectButtonClicked;
	public event Action BackButtonClicked;
	
	public void Init() { }

	public void LateInit()
	{
		_verticalGroup = new VerticalLayoutGroup(Point.Zero, GameManager.Viewport.Bounds.Size)
		{
			Spacing = 100,
			ChildAnchor = HorizontalAnchor.Center,
			ForceExpandChildWidth = false
		};

		_ipAddressLabel = new Label(ButtonSize, "IP Address:", _uiStyleCollection.DefaultLabelStyle);
		_ipAddressTextInput = new IpAddressTextInput(ButtonSize, _uiStyleCollection.DefaultTextInputStyle)
		{
			MaxLength = 15, // Max length of an IP address
			HintText = "Ip Address"
		};
		_connectButton = new Button(ButtonSize, "Connect", _uiStyleCollection.DefaultButtonStyle);
		_backButton = new Button(ButtonSize, "Back", _uiStyleCollection.DefaultButtonStyle);
		
		_verticalGroup.AddChildren(_ipAddressLabel, _ipAddressTextInput, _connectButton, _backButton);

		_connectButton.ReceivedFocus += OnUIElementReceivedFocus;

		_ipAddressTextInput.HasFocus = true;
	}

	public void Start()
	{
		_connectButton.MouseClicked += OnConnectButtonMouseClicked;
		_backButton.MouseClicked += OnBackButtonMouseClicked;
	}

	public void Update(float deltaTimeSeconds)
	{
		_verticalGroup.Update(deltaTimeSeconds);
		_connectButton.IsInteractable = _ipAddressTextInput.ContainsValidString;
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		_verticalGroup.Draw(spriteBatch);
	}

	public void Exit()
	{
		_connectButton.MouseClicked -= OnConnectButtonMouseClicked;
		_backButton.MouseClicked -= OnBackButtonMouseClicked;
		
		_ipAddressTextInput.Clear();
	}
	
	private void OnUIElementReceivedFocus(IUIElement uiElement)
	{
		if (_focusedUIElement != null)
			_focusedUIElement.HasFocus = false;
		_focusedUIElement = uiElement;
	}

	private void OnConnectButtonMouseClicked(IUIElement _) => ConnectButtonClicked?.Invoke();
	private void OnBackButtonMouseClicked(IUIElement _) => BackButtonClicked?.Invoke();
}