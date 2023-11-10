using Andavies.MonoGame.Inputs.InputListeners;
using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.Interfaces;
using Andavies.MonoGame.UI.LayoutGroups;
using Andavies.MonoGame.UI.StateMachines;
using Andavies.MonoGame.UI.UIElements;
using Andavies.MonoGame.UI.UIElements.TextInputs;
using Autofac.Features.AttributeFilters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.Globals;

namespace SpellboundSettlement.UIStates.MainMenu;

public class MainMenuCreateServerUIState : IUIState
{
	private static readonly Point ButtonSize = new(175, 60);

	private readonly ITextListener _numbersOnlyTextListener;
	private readonly IUIStyleCollection _uiStyleCollection;
	private IUIElement _focusedUIElement;

	private VerticalLayoutGroup _verticalGroup;
	private HorizontalLayoutGroup _horizontalGroup;

	public MainMenuCreateServerUIState(IUIStyleCollection uiStyleCollection,
		[KeyFilter(nameof(NumbersOnlyTextListener))] ITextListener numbersOnlyTextListener)
	{
		_uiStyleCollection = uiStyleCollection;
		_numbersOnlyTextListener = numbersOnlyTextListener;
	}

	public Label EnterIpLabel { get; private set; }
	public TextInput IpInput { get; private set; }
	public TextInput ServerPortInput { get; private set; }
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

		EnterIpLabel = new Label(ButtonSize, "IP Address:", _uiStyleCollection.DefaultLabelStyle);
		IpInput = new IpAddressTextInput(ButtonSize, _uiStyleCollection.DefaultTextInputStyle)
		{
			MaxLength = 15, // Max length of an IP address
			HintText = "Ip Address"
		};
		ServerPortInput = new TextInput(ButtonSize, _uiStyleCollection.DefaultTextInputStyle, _numbersOnlyTextListener)
		{
			MaxLength = 5, // Max port length
			HintText = "Ip Port"
		};
		CreateButton = new Button(ButtonSize, "Create", _uiStyleCollection.DefaultButtonStyle);
		BackButton = new Button(ButtonSize, "Back", _uiStyleCollection.DefaultButtonStyle);
		
		_horizontalGroup.AddChildren(EnterIpLabel, IpInput, ServerPortInput);
		_verticalGroup.AddChildren(_horizontalGroup, CreateButton, BackButton);
		
		IpInput.MouseReleased += SetFocusedElement;
		ServerPortInput.MouseReleased += SetFocusedElement;

		SetFocusedElement(IpInput);
	}

	public void Update(float deltaTimeSeconds)
	{
		_verticalGroup.Update(deltaTimeSeconds);
		CreateButton.IsInteractable = IpInput.ContainsValidString;
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		_verticalGroup.Draw(spriteBatch);
	}

	public void Exit()
	{
		IpInput.Clear();
		ServerPortInput.Clear();
	}

	private void SetFocusedElement(IUIElement uiElement)
	{
		if (_focusedUIElement != null)
			_focusedUIElement.HasFocus = false;

		_focusedUIElement = uiElement;
		
		if (_focusedUIElement != null)
			_focusedUIElement.HasFocus = true;
	}
}