using System;
using Andavies.MonoGame.UI.Interfaces;
using SpellboundSettlement.Inputs;
using SpellboundSettlement.UIStates;
using SpellboundSettlement.UIStates.MainMenu;

namespace SpellboundSettlement.GameStates;

public class MainMenuGameState : GameState
{
	public override IInputManager InputState { get; }

	private readonly MainMenuMainUIState _mainUIState;
	private readonly MainMenuConnectToServerUIState _connectToServerUIState;
	private readonly MainMenuCreateServerUIState _createServerUIState;
	
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
		IInputManager inputManager)
	{
		_mainUIState = mainUIState;
		_connectToServerUIState = connectToServerUIState;
		_createServerUIState = createServerUIState;
		InputState = inputManager;
		
		UIStates.Add(_mainUIState);
		UIStates.Add(_connectToServerUIState);
		UIStates.Add(_createServerUIState);
	}

	public override void Start()
	{
		base.Start();

		_mainUIState.ConnectToServerButton.MouseReleased += OnConnectToServerButtonPressed;
		_mainUIState.CreateServerButton.MouseReleased += OnCreateServerButtonPressed;
		
		_connectToServerUIState.ConnectButton.MouseReleased += OnConnectToServerConnectButtonPressed;
		_connectToServerUIState.BackButton.MouseReleased += OnConnectToServerBackButtonPressed;

		_createServerUIState.CreateButton.MouseReleased += OnCreateServerCreateButtonPressed;
		_createServerUIState.BackButton.MouseReleased += OnCreateServerBackButtonPressed;
		
		UIStateMachine.ChangeUIState(_mainUIState);
	}

	public override void End()
	{
		base.End();
		
		_mainUIState.ConnectToServerButton.MouseReleased -= OnConnectToServerButtonPressed;
		_mainUIState.CreateServerButton.MouseReleased -= OnCreateServerButtonPressed;

		_connectToServerUIState.ConnectButton.MouseReleased -= OnConnectToServerConnectButtonPressed;
		_connectToServerUIState.BackButton.MouseReleased -= OnConnectToServerBackButtonPressed;

		_createServerUIState.CreateButton.MouseReleased -= OnCreateServerCreateButtonPressed;
		_createServerUIState.BackButton.MouseReleased -= OnCreateServerBackButtonPressed;
	}

	// MainUI Actions
	private void OnConnectToServerButtonPressed(IUIElement uiElement) => UIStateMachine.ChangeUIState(_connectToServerUIState);
	private void OnCreateServerButtonPressed(IUIElement uiElement) => UIStateMachine.ChangeUIState(_createServerUIState);
	
	// ConnectToServerUI Actions
	private void OnConnectToServerConnectButtonPressed(IUIElement uiElement) => UIStateMachine.ChangeUIState(_mainUIState);
	private void OnConnectToServerBackButtonPressed(IUIElement uiElement) => UIStateMachine.ChangeUIState(_mainUIState);
	
	// CreateServerUI Actions
	private void OnCreateServerCreateButtonPressed(IUIElement uiElement) => UIStateMachine.ChangeUIState(_mainUIState);
	private void OnCreateServerBackButtonPressed(IUIElement uiElement) => UIStateMachine.ChangeUIState(_mainUIState);
}