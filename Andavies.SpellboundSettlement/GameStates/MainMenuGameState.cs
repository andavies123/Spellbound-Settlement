using System;
using Andavies.SpellboundSettlement.Inputs;
using Andavies.SpellboundSettlement.UIStates.MainMenu;

namespace Andavies.SpellboundSettlement.GameStates;

public class MainMenuGameState : GameState
{
	public override IInputState InputState { get; }

	private readonly MainMenuMainUIState _mainUIState;
	private readonly MainMenuPlayUIState _playUIState;
	private readonly MainMenuNewGameUIState _newGameUIState;
	private readonly MainMenuJoinServerUIState _joinServerUIState;
	private readonly MainMenuCreateServerUIState _createServerUIState;
	private readonly MainMenuOptionsUIState _optionsUIState;

	public MainMenuGameState(
		MainMenuMainUIState mainUIState,
		MainMenuPlayUIState playUIState,
		MainMenuNewGameUIState newGameUIState,
		MainMenuJoinServerUIState joinServerUIState,
		MainMenuCreateServerUIState createServerUIState,
		MainMenuOptionsUIState optionsUIState,
		IInputState inputState)
	{
		_mainUIState = mainUIState;
		_playUIState = playUIState;
		_newGameUIState = newGameUIState;
		_joinServerUIState = joinServerUIState;
		_createServerUIState = createServerUIState;
		_optionsUIState = optionsUIState;
		InputState = inputState;
		
		UIStates.Add(_mainUIState);
		UIStates.Add(_playUIState);
		UIStates.Add(_newGameUIState);
		UIStates.Add(_joinServerUIState);
		UIStates.Add(_createServerUIState);
		UIStates.Add(_optionsUIState);
	}

	public event Action PlayGameRequested;
	public event Action QuitGameRequested;
	public event Action<string> StartServerRequested;
	public event Action<string> JoinServerRequested;

	public override void Start()
	{
		base.Start();

		_mainUIState.PlayButtonClicked += OnPlayButtonClicked;
		_mainUIState.JoinServerButtonClicked += OnJoinServerButtonClicked;
		_mainUIState.CreateServerButtonClicked += OnCreateServerButtonClicked;
		_mainUIState.OptionsButtonClicked += OnOptionsButtonClicked;
		_mainUIState.QuitButtonClicked += OnQuitButtonClicked;

		_playUIState.NewGameActionRequested += OnNewGameActionRequested;
		_playUIState.LoadGameActionRequested += OnLoadGameActionRequested;
		_playUIState.MultiplayerActionRequested += OnMultiplayerActionRequested;
		_playUIState.BackActionRequested += OnPlayMenuBackActionRequested;

		_newGameUIState.CreateWorldActionRequested += OnCreateWorldActionRequested;
		_newGameUIState.BackActionRequested += OnNewGameMenuBackActionRequested;
		
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

		_playUIState.NewGameActionRequested -= OnNewGameActionRequested;
		_playUIState.LoadGameActionRequested -= OnLoadGameActionRequested;
		_playUIState.MultiplayerActionRequested -= OnMultiplayerActionRequested;
		_playUIState.BackActionRequested -= OnPlayMenuBackActionRequested;

		_newGameUIState.CreateWorldActionRequested -= OnCreateWorldActionRequested;
		_newGameUIState.BackActionRequested -= OnNewGameMenuBackActionRequested;
		
		_joinServerUIState.ConnectButtonClicked -= OnJoinServerConnectButtonClicked;
		_joinServerUIState.BackButtonClicked -= OnJoinServerBackButtonClicked;

		_createServerUIState.CreateServerButtonClicked -= OnCreateServerCreateButtonClicked;
		_createServerUIState.BackButtonClicked -= OnCreateServerBackButtonClicked;
		
		_optionsUIState.BackButtonClicked -= OnOptionsBackButtonClicked;
	}

	// MainUI Actions
	private void OnPlayButtonClicked() => UIStateMachine.ChangeUIState(_playUIState);
	private void OnJoinServerButtonClicked() => UIStateMachine.ChangeUIState(_joinServerUIState);
	private void OnCreateServerButtonClicked() => UIStateMachine.ChangeUIState(_createServerUIState);
	private void OnOptionsButtonClicked() => UIStateMachine.ChangeUIState(_optionsUIState);
	private void OnQuitButtonClicked() => QuitGameRequested?.Invoke();
	
	// Play Menu Actions
	private void OnNewGameActionRequested() => UIStateMachine.ChangeUIState(_newGameUIState);
	private void OnLoadGameActionRequested() => UIStateMachine.ChangeUIState(_newGameUIState);
	private void OnMultiplayerActionRequested() => UIStateMachine.ChangeUIState(_joinServerUIState);
	private void OnPlayMenuBackActionRequested() => UIStateMachine.ChangeUIState(_mainUIState);
	
	// New Game Menu Actions
	private void OnCreateWorldActionRequested() => PlayGameRequested?.Invoke();
	private void OnNewGameMenuBackActionRequested() => UIStateMachine.ChangeUIState(_playUIState);
	
	// ConnectToServerUI Actions
	private void OnJoinServerConnectButtonClicked() => JoinServerRequested?.Invoke("192.1.1.1");
	private void OnJoinServerBackButtonClicked() => UIStateMachine.ChangeUIState(_playUIState);
	
	// CreateServerUI Actions
	private void OnCreateServerCreateButtonClicked() => StartServerRequested?.Invoke("192.1.1.1");
	private void OnCreateServerBackButtonClicked() => UIStateMachine.ChangeUIState(_mainUIState);

	// OptionsUI Actions
	private void OnOptionsBackButtonClicked() => UIStateMachine.ChangeUIState(_mainUIState);
}