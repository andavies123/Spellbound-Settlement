using System;
using SpellboundSettlement.Inputs;
using SpellboundSettlement.UIStates;

namespace SpellboundSettlement.GameStates;

public class MainMenuGameState : GameState
{
	public override IInputManager InputState { get; }

	private readonly MainMenuMainUIState _mainUIState;
	private readonly MainMenuConnectToServerUIState _connectToServerUIState;
	
	public event Action PlayGame
	{
		add => _mainUIState.PlayButton.MouseReleased += value;
		remove => _mainUIState.PlayButton.MouseReleased -= value;
	}

	public event Action QuitGame
	{
		add => _mainUIState.QuitButton.MouseReleased += value;
		remove => _mainUIState.QuitButton.MouseReleased -= value;
	}

	public MainMenuGameState(
		MainMenuMainUIState mainUIState, 
		MainMenuConnectToServerUIState connectToServerUIState, 
		IInputManager inputManager)
	{
		_mainUIState = mainUIState;
		_connectToServerUIState = connectToServerUIState;
		InputState = inputManager;
		
		UIStates.Add(_mainUIState);
		UIStates.Add(_connectToServerUIState);
	}

	public override void Start()
	{
		base.Start();

		_mainUIState.ConnectToServerButton.MouseReleased += OnConnectToServerButtonPressed;

		_connectToServerUIState.ConnectButton.MouseReleased += OnConnectToServerConnectButtonPressed;
		_connectToServerUIState.BackButton.MouseReleased += OnConnectToServerBackButtonPressed;
		
		UIStateMachine.ChangeUIState(_mainUIState);
	}

	public override void End()
	{
		base.End();
		
		_mainUIState.ConnectToServerButton.MouseReleased -= OnConnectToServerButtonPressed;

		_connectToServerUIState.ConnectButton.MouseReleased -= OnConnectToServerConnectButtonPressed;
		_connectToServerUIState.BackButton.MouseReleased -= OnConnectToServerBackButtonPressed;
	}

	private void OnConnectToServerButtonPressed() => UIStateMachine.ChangeUIState(_connectToServerUIState);
	
	private void OnConnectToServerConnectButtonPressed() => UIStateMachine.ChangeUIState(_mainUIState);
	private void OnConnectToServerBackButtonPressed() => UIStateMachine.ChangeUIState(_mainUIState);
}