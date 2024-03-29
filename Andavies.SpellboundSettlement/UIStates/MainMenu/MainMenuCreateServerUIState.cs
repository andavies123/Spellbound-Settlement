﻿using System;
using Andavies.MonoGame.Inputs;
using Andavies.MonoGame.Inputs.InputListeners;
using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.Interfaces;
using Andavies.MonoGame.UI.LayoutGroups;
using Andavies.MonoGame.UI.StateMachines;
using Andavies.MonoGame.UI.UIElements;
using Andavies.MonoGame.UI.UIElements.TextInputs;
using Andavies.SpellboundSettlement.Globals;
using Autofac.Features.AttributeFilters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.SpellboundSettlement.UIStates.MainMenu;

public class MainMenuCreateServerUIState : IUIState
{
	private static readonly Point ButtonSize = new(175, 60);

	private readonly IInputManager _inputManager;
	private readonly IInputListener _numbersOnlyInputListener;
	private readonly IUIStyleRepository _uiStyleCollection;
	private IUIElement _focusedUIElement;

	private VerticalLayoutGroup _verticalGroup;
	private HorizontalLayoutGroup _horizontalGroup;

	private Label _enterIpLabel;
	private TextInput _ipAddressInput;
	private TextInput _portInput;
	private Button _createServerButton;
	private Button _backButton;

	public MainMenuCreateServerUIState(
		IInputManager inputManager,
		IUIStyleRepository uiStyleCollection,
		[KeyFilter(nameof(NumberDecimalInputListener))] IInputListener numbersOnlyInputListener)
	{
		_inputManager = inputManager ?? throw new ArgumentNullException(nameof(inputManager));
		_uiStyleCollection = uiStyleCollection ?? throw new ArgumentNullException(nameof(uiStyleCollection));
		_numbersOnlyInputListener = numbersOnlyInputListener ?? throw new ArgumentNullException(nameof(numbersOnlyInputListener));
	}

	public event Action CreateServerButtonClicked;
	public event Action BackButtonClicked;
	
	public void Init() { }

	public void LateInit()
	{
		_verticalGroup = new VerticalLayoutGroup(_inputManager, Point.Zero, GameManager.Viewport.Bounds.Size)
		{
			Spacing = 100,
			ChildAnchor = HorizontalAnchor.Center,
			ForceExpandChildWidth = false
		};

		_horizontalGroup = new HorizontalLayoutGroup(_inputManager, new Point(GameManager.Viewport.Width, 60))
		{
			Spacing = 200,
			ChildAnchor = VerticalAnchor.Center,
			ForceExpandChildHeight = false
		};

		_enterIpLabel = new Label(_inputManager, ButtonSize, "IP Address:", _uiStyleCollection.DefaultLabelStyle);
		_ipAddressInput = new IpAddressTextInput(_inputManager, ButtonSize, _uiStyleCollection.DefaultTextInputStyle)
		{
			MaxLength = 15, // Max length of an IP address
			HintText = "Ip Address"
		};
		_portInput = new TextInput(_inputManager, ButtonSize, _uiStyleCollection.DefaultTextInputStyle, _numbersOnlyInputListener)
		{
			MaxLength = 5, // Max port length
			HintText = "Ip Port"
		};
		_createServerButton = new Button(_inputManager, ButtonSize, "Create", _uiStyleCollection.DefaultButtonStyle);
		_backButton = new Button(_inputManager, ButtonSize, "Back", _uiStyleCollection.DefaultButtonStyle);
		
		_horizontalGroup.AddChildren(_enterIpLabel, _ipAddressInput, _portInput);
		_verticalGroup.AddChildren(_horizontalGroup, _createServerButton, _backButton);
		
		_ipAddressInput.MouseClicked += SetFocusedElement;
		_createServerButton.MouseClicked += SetFocusedElement;

		SetFocusedElement(_ipAddressInput);
	}

	public void Start()
	{
		_createServerButton.MouseClicked += OnCreateServerButtonMouseClicked;
		_backButton.MouseClicked += OnBackButtonMouseClicked;
		
		_ipAddressInput.MouseClicked += SetFocusedElement;
		_createServerButton.MouseClicked += SetFocusedElement;
	}

	public void Update(float deltaTimeSeconds)
	{
		_verticalGroup.Update(deltaTimeSeconds);
		_createServerButton.IsInteractable = _ipAddressInput.ContainsValidString;
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		_verticalGroup.Draw(spriteBatch);
	}

	public void Exit()
	{
		_createServerButton.MouseClicked -= OnCreateServerButtonMouseClicked;
		_backButton.MouseClicked -= OnBackButtonMouseClicked;
		
		_ipAddressInput.MouseClicked -= SetFocusedElement;
		_createServerButton.MouseClicked -= SetFocusedElement;
		
		_ipAddressInput.Clear();
		_portInput.Clear();
	}

	private void OnCreateServerButtonMouseClicked(IUIElement _) => CreateServerButtonClicked?.Invoke();
	private void OnBackButtonMouseClicked(IUIElement _) => BackButtonClicked?.Invoke();

	private void SetFocusedElement(IUIElement uiElement)
	{
		if (_focusedUIElement != null)
			_focusedUIElement.HasFocus = false;

		_focusedUIElement = uiElement;
		
		if (_focusedUIElement != null)
			_focusedUIElement.HasFocus = true;
	}
}