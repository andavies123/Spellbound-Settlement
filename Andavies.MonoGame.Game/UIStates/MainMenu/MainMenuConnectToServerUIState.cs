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

public class MainMenuConnectToServerUIState : IUIState
{
	private static readonly Point ButtonSize = new(175, 60);

	private readonly IUIStyleCollection _uiStyleCollection;
	private VerticalLayoutGroup _verticalGroup;
	private IUIElement _focusedUIElement;

	public MainMenuConnectToServerUIState(IUIStyleCollection uiStyleCollection)
	{
		_uiStyleCollection = uiStyleCollection;
	}

	public Label IpLabel { get; private set; }
	public TextInput IpInput { get; private set; }
	public Button ConnectButton { get; private set; }
	public Button BackButton { get; private set; }
	
	public void Init() { }

	public void LateInit()
	{
		_verticalGroup = new VerticalLayoutGroup(Point.Zero, GameManager.Viewport.Bounds.Size)
		{
			Spacing = 100,
			ChildAnchor = HorizontalAnchor.Center,
			ForceExpandChildWidth = false
		};

		IpLabel = new Label(ButtonSize, "IP Address:", _uiStyleCollection.DefaultLabelStyle);
		IpInput = new IpAddressTextInput(ButtonSize, _uiStyleCollection.DefaultTextInputStyle)
		{
			MaxLength = 15, // Max length of an IP address
			HintText = "Ip Address"
		};
		ConnectButton = new Button(ButtonSize, "Connect", _uiStyleCollection.DefaultButtonStyle);
		BackButton = new Button(ButtonSize, "Back", _uiStyleCollection.DefaultButtonStyle);
		
		_verticalGroup.AddChildren(IpLabel, IpInput, ConnectButton, BackButton);

		ConnectButton.ReceivedFocus += OnUIElementReceivedFocus;

		IpInput.HasFocus = true;
	}

	public void Update(float deltaTimeSeconds)
	{
		_verticalGroup.Update(deltaTimeSeconds);
		ConnectButton.IsInteractable = IpInput.ContainsValidString;
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		_verticalGroup.Draw(spriteBatch);
	}

	public void Exit()
	{
		IpInput.Clear();
	}
	
	private void OnUIElementReceivedFocus(IUIElement uiElement)
	{
		if (_focusedUIElement != null)
			_focusedUIElement.HasFocus = false;
		_focusedUIElement = uiElement;
	}
}