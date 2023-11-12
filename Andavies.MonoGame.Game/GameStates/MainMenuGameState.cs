using System;
using SpellboundSettlement.Inputs;
using SpellboundSettlement.UIStates.MainMenu;

namespace SpellboundSettlement.GameStates;

public class MainMenuGameState : GameState
{
	public override IInputManager InputState { get; }

	private readonly MainMenuMainUIState _mainUIState;
	private readonly MainMenuJoinServerUIState _joinServerUIState;
	private readonly MainMenuCreateServerUIState _createServerUIState;
	private readonly MainMenuOptionsUIState _optionsUIState;

	public MainMenuGameState(
		MainMenuMainUIState mainUIState, 
		MainMenuJoinServerUIState joinServerUIState,
		MainMenuCreateServerUIState createServerUIState,
		MainMenuOptionsUIState optionsUIState,
		IInputManager inputManager)
	{
		_mainUIState = mainUIState;
		_joinServerUIState = joinServerUIState;
		_createServerUIState = createServerUIState;
		_optionsUIState = optionsUIState;
		InputState = inputManager;
		
		UIStates.Add(_mainUIState);
		UIStates.Add(_joinServerUIState);
		UIStates.Add(_createServerUIState);
		UIStates.Add(_optionsUIState);
	}

	public event Action PlayGameRequested;
	public event Action QuitGameRequested;

	public override void Start()
	{
		base.Start();

		_mainUIState.PlayButtonClicked += OnPlayButtonClicked;
		_mainUIState.JoinServerButtonClicked += OnJoinServerButtonClicked;
		_mainUIState.CreateServerButtonClicked += OnCreateServerButtonClicked;
		_mainUIState.OptionsButtonClicked += OnOptionsButtonClicked;
		_mainUIState.QuitButtonClicked += OnQuitButtonClicked;
		
		_joinServerUIState.ConnectButtonClicked += OnJoinServerConnectButtonClicked;
		_joinServerUIState.BackButtonClicked += OnJoinServerBackButtonClicked;

		_createServerUIState.CreateServerButtonClicked += OnCreateServerCreateButtonClicked;
		_createServerUIState.BackButtonClicked += OnCreateServerBackButtonClicked;
		
		_optionsUIState.BackButtonClicked += OnOptionsBackButtonClicked;
		
		UIStateMachine.ChangeUIState(_mainUIState);
	}

	public override void End()
	{
		base.End();

		_mainUIState.PlayButtonClicked -= OnPlayButtonClicked;
		_mainUIState.JoinServerButtonClicked -= OnJoinServerButtonClicked;
		_mainUIState.CreateServerButtonClicked -= OnCreateServerButtonClicked;
		_mainUIState.OptionsButtonClicked -= OnOptionsButtonClicked;
		_mainUIState.QuitButtonClicked -= OnQuitButtonClicked;
		
		_joinServerUIState.ConnectButtonClicked -= OnJoinServerConnectButtonClicked;
		_joinServerUIState.BackButtonClicked -= OnJoinServerBackButtonClicked;

		_createServerUIState.CreateServerButtonClicked -= OnCreateServerCreateButtonClicked;
		_createServerUIState.BackButtonClicked -= OnCreateServerBackButtonClicked;
		
		_optionsUIState.BackButtonClicked -= OnOptionsBackButtonClicked;
	}

	// MainUI Actions
	private void OnPlayButtonClicked() => PlayGameRequested?.Invoke();
	private void OnJoinServerButtonClicked() => UIStateMachine.ChangeUIState(_joinServerUIState);
	private void OnCreateServerButtonClicked() => UIStateMachine.ChangeUIState(_createServerUIState);
	private void OnOptionsButtonClicked() => UIStateMachine.ChangeUIState(_optionsUIState);
	private void OnQuitButtonClicked() => QuitGameRequested?.Invoke();
	
	// ConnectToServerUI Actions
	private void OnJoinServerConnectButtonClicked() => UIStateMachine.ChangeUIState(_mainUIState);
	private void OnJoinServerBackButtonClicked() => UIStateMachine.ChangeUIState(_mainUIState);
	
	// CreateServerUI Actions
	private void OnCreateServerCreateButtonClicked() => UIStateMachine.ChangeUIState(_mainUIState);
	private void OnCreateServerBackButtonClicked() => UIStateMachine.ChangeUIState(_mainUIState);

	// OptionsUI Actions
	private void OnOptionsBackButtonClicked() => UIStateMachine.ChangeUIState(_mainUIState);
}