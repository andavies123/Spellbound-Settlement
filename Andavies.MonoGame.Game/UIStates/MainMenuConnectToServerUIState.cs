using System.Collections.Generic;
using Andavies.MonoGame.Input.InputListeners;
using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.StateMachines;
using Andavies.MonoGame.UI.Styles;
using Andavies.MonoGame.UI.UIElements;
using Autofac.Features.AttributeFilters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.Globals;
using Label = Andavies.MonoGame.UI.UIElements.Label;

namespace SpellboundSettlement.UIStates;

public class MainMenuConnectToServerUIState : IUIState
{
	private static readonly Point IpLabelPosition = new(-100, 0);
	private static readonly Point IpInputPosition = new(100, 0);
	private static readonly Point ConnectButtonPosition = new(0, 100);
	private static readonly Point BackButtonPosition = new(0, 200);
	
	private static readonly Point ButtonSize = new(175, 60);

	private readonly ITextListener _numbersOnlyTextListener;
	private List<UIElement> _uiElements;
	private UIElement _focusedUIElement = null;

	public MainMenuConnectToServerUIState(
		[KeyFilter(nameof(NumbersOnlyTextListener))] ITextListener numbersOnlyTextListener)
	{
		_numbersOnlyTextListener = numbersOnlyTextListener;
	}

	public Label IpLabel { get; private set; }
	public TextInput IpInput { get; private set; }
	public Button ConnectButton { get; private set; }
	public Button BackButton { get; private set; }
	
	public void Init() { }

	public void LateInit()
	{
		ButtonStyle buttonStyle = new()
		{
			Font = GlobalFonts.DefaultFont,
			BackgroundColor = Color.LightSlateGray,
			HoverBackgroundColor = Color.SlateGray,
			MousePressedBackgroundColor = Color.DarkSlateGray,
			DisabledBackgroundColor = Color.Red,
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

		IpLabel = new Label(IpLabelPosition, ButtonSize, "IP Address:", labelStyle);
		IpInput = new TextInput(IpInputPosition, ButtonSize, textInputStyle, _numbersOnlyTextListener)
		{
			InputType = InputType.NumbersOnly,
			MaxLength = 15, // Max length of an IP address
			HintText = "Ip Address"
		};
		ConnectButton = new Button(ConnectButtonPosition, ButtonSize, "Connect", buttonStyle);
		BackButton = new Button(BackButtonPosition, ButtonSize, "Back", buttonStyle);
		
		_uiElements = new List<UIElement>
		{
			IpLabel,
			IpInput,
			ConnectButton,
			BackButton
		};
		
		_uiElements.ForEach(uiElement => uiElement.ReceivedFocus += OnUIElementReceivedFocus);

		IpInput.HasFocus = true;
	}

	public void Update(float deltaTimeSeconds)
	{
		_uiElements.ForEach(uiElement => uiElement.Update());
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		_uiElements.ForEach(uiElement => uiElement.Draw(spriteBatch));
	}

	public void Exit()
	{
		IpInput.Clear();
	}
	
	private void OnUIElementReceivedFocus(UIElement uiElement)
	{
		if (_focusedUIElement != null)
			_focusedUIElement.HasFocus = false;
		_focusedUIElement = uiElement;
	}
}