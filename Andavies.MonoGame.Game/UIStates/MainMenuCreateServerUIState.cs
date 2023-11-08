using System;
using Andavies.MonoGame.Input.InputListeners;
using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.Interfaces;
using Andavies.MonoGame.UI.LayoutGroups;
using Andavies.MonoGame.UI.StateMachines;
using Andavies.MonoGame.UI.Styles;
using Andavies.MonoGame.UI.UIElements;
using Autofac.Features.AttributeFilters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.Globals;

namespace SpellboundSettlement.UIStates;

public class MainMenuCreateServerUIState : IUIState
{
	private static readonly Point ButtonSize = new(175, 60);

	private readonly ITextListener _numbersOnlyTextListener;
	private IUIElement _focusedUIElement;

	private VerticalLayoutGroup _verticalGroup;
	private HorizontalLayoutGroup _horizontalGroup;

	public MainMenuCreateServerUIState(
		[KeyFilter(nameof(NumbersOnlyTextListener))] ITextListener numbersOnlyTextListener)
	{
		_numbersOnlyTextListener = numbersOnlyTextListener;
	}

	public Label EnterIpLabel { get; private set; }
	public TextInput IpInput { get; private set; }
	public Button CreateButton { get; private set; }
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

		_horizontalGroup = new HorizontalLayoutGroup(new Point(GameManager.Viewport.Width, 60))
		{
			Spacing = 200,
			ChildAnchor = VerticalAnchor.Center,
			ForceExpandChildHeight = false
		};
		
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

		EnterIpLabel = new Label(ButtonSize, "IP Address:", labelStyle);
		IpInput = new TextInput(ButtonSize, textInputStyle, _numbersOnlyTextListener)
		{
			InputType = InputType.NumbersOnly,
			MaxLength = 15, // Max length of an IP address
			HintText = "Ip Address"
		};
		CreateButton = new Button(ButtonSize, "Create", buttonStyle);
		BackButton = new Button(ButtonSize, "Back", buttonStyle);
		
		_horizontalGroup.AddChildren(EnterIpLabel, IpInput);
		_verticalGroup.AddChildren(_horizontalGroup, CreateButton, BackButton);
		
		//_uiElements.ForEach(uiElement => uiElement.ReceivedFocus += OnUIElementReceivedFocus);

		IpInput.HasFocus = true;
	}

	public void Update(float deltaTimeSeconds)
	{
		_verticalGroup.Update(deltaTimeSeconds);
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