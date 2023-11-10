using System;
using Andavies.MonoGame.UI.Interfaces;
using SpellboundSettlement.Inputs;
using SpellboundSettlement.UIStates.MainMenu;

namespace SpellboundSettlement.GameStates;

public class MainMenuGameState : GameState
{
	public override IInputManager InputState { get; }

	private readonly MainMenuMainUIState _mainUIState;
	private readonly MainMenuConnectToServerUIState _connectToServerUIState;
	private readonly MainMenuCreateServerUIState _createServerUIState;
	private readonly MainMenuOptionsUIState _optionsUIState;
	
	public event Action<IUIElement> PlayGame
	{
		add => _mainUIState.PlayButton.MouseReleased += value;
		remove => _mainUIState.PlayButton.MouseReleased -= value;
	}

	public event Action<IUIElement> QuitGame
	{
		add => _mainUIState.QuitButton.MouseReleased += value;
		remove => _mainUIState.QuitButton.MouseReleased -= value;
	}

	public MainMenuGameState(
		MainMenuMainUIState mainUIState, 
		MainMenuConnectToServerUIState connectToServerUIState,
		MainMenuCreateServerUIState createServerUIState,
		MainMenuOptionsUIState optionsUIState,
		IInputManager inputManager)
	{
		_mainUIState = mainUIState;
		_connectToServerUIState = connectToServerUIState;
		_createServerUIState = createServerUIState;
		_optionsUIState = optionsUIState;
		InputState = inputManager;
		
		UIStates.Add(_mainUIState);
		UIStates.Add(_connectToServerUIState);
		UIStates.Add(_createServerUIState);
		UIStates.Add(_optionsUIState);
	}

	public override void Start()
	{
		base.Start();

		_mainUIState.ConnectToServerButton.MouseReleased += OnConnectToServerButtonPressed;
		_mainUIState.CreateServerButton.MouseReleased += OnCreateServerButtonPressed;
		_mainUIState.OptionsButton.MouseReleased += OnOptionsButtonPressed;
		
		_connectToServerUIState.ConnectButton.MouseReleased += OnConnectToServerConnectButtonPressed;
		_connectToServerUIState.BackButton.MouseReleased += OnConnectToServerBackButtonPressed;

		_createServerUIState.CreateButton.MouseReleased += OnCreateServerCreateButtonPressed;
		_createServerUIState.BackButton.MouseReleased += OnCreateServerBackButtonPressed;
		
		_optionsUIState.BackButton.MouseReleased += OnOptionsBackButtonPressed;
		
		UIStateMachine.ChangeUIState(_mainUIState);
	}

	public override void End()
	{
		base.End();
		
		_mainUIState.ConnectToServerButton.MouseReleased -= OnConnectToServerButtonPressed;
		_mainUIState.CreateServerButton.MouseReleased -= OnCreateServerButtonPressed;
		_mainUIState.OptionsButton.MouseReleased -= OnOptionsButtonPressed;

		_connectToServerUIState.ConnectButton.MouseReleased -= OnConnectToServerConnectButtonPressed;
		_connectToServerUIState.BackButton.MouseReleased -= OnConnectToServerBackButtonPressed;

		_createServerUIState.CreateButton.MouseReleased -= OnCreateServerCreateButtonPressed;
		_createServerUIState.BackButton.MouseReleased -= OnCreateServerBackButtonPressed;

		_optionsUIState.BackButton.MouseReleased -= OnOptionsBackButtonPressed;
	}

	// MainUI Actions
	private void OnConnectToServerButtonPressed(IUIElement uiElement) => UIStateMachine.ChangeUIState(_connectToServerUIState);
	private void OnCreateServerButtonPressed(IUIElement uiElement) => UIStateMachine.ChangeUIState(_createServerUIState);
	private void OnOptionsButtonPressed(IUIElement uiElement) => UIStateMachine.ChangeUIState(_optionsUIState);
	
	// ConnectToServerUI Actions
	private void OnConnectToServerConnectButtonPressed(IUIElement uiElement) => UIStateMachine.ChangeUIState(_mainUIState);
	private void OnConnectToServerBackButtonPressed(IUIElement uiElement) => UIStateMachine.ChangeUIState(_mainUIState);
	
	// CreateServerUI Actions
	private void OnCreateServerCreateButtonPressed(IUIElement uiElement) => UIStateMachine.ChangeUIState(_mainUIState);
	private void OnCreateServerBackButtonPressed(IUIElement uiElement) => UIStateMachine.ChangeUIState(_mainUIState);

	// OptionsUI Actions
	private void OnOptionsBackButtonPressed(IUIElement uiElement) => UIStateMachine.ChangeUIState(_mainUIState);
}