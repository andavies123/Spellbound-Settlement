using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.Interfaces;
using Andavies.MonoGame.UI.LayoutGroups;
using Andavies.MonoGame.UI.StateMachines;
using Andavies.MonoGame.UI.Styles;
using Andavies.MonoGame.UI.UIElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.Globals;
using Label = Andavies.MonoGame.UI.UIElements.Label;

namespace SpellboundSettlement.UIStates.MainMenu;

public class MainMenuConnectToServerUIState : IUIState
{
	private static readonly Point ButtonSize = new(175, 60);

	private VerticalLayoutGroup _verticalGroup;
	private IUIElement _focusedUIElement;

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
		
		ButtonStyle buttonStyle = new()
		{
			Font = GlobalFonts.DefaultFont,
			BackgroundColor = Color.LightSlateGray,
			HoverBackgroundColor = Color.SlateGray,
			MousePressedBackgroundColor = Color.DarkSlateGray,
			DisabledBackgroundColor = new Color(.3f, .3f, .3f),
			BackgroundTexture = GameManager.Texture
		};

		LabelStyle labelStyle = new()
		{
			Font = GlobalFonts.DefaultFont,
			BackgroundColor = Color.Transparent,
			BackgroundTexture = GameManager.Texture
		};

		TextInputStyle textInputStyle = new()
		{
			Font = GlobalFonts.DefaultFont,
			HintTextFont = GlobalFonts.HintFont,
			BackgroundColor = Color.LightGray,
			BackgroundTexture = GameManager.Texture
		};

		IpLabel = new Label(ButtonSize, "IP Address:", labelStyle);
		IpInput = new IpAddressTextInput(ButtonSize, textInputStyle)
		{
			MaxLength = 15, // Max length of an IP address
			HintText = "Ip Address"
		};
		ConnectButton = new Button(ButtonSize, "Connect", buttonStyle);
		BackButton = new Button(ButtonSize, "Back", buttonStyle);
		
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