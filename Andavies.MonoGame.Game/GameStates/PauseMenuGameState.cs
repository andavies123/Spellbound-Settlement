using System;
using SpellboundSettlement.Inputs;
using SpellboundSettlement.UIStates.PauseMenu;

namespace SpellboundSettlement.GameStates;

public class PauseMenuGameState : GameState
{
	private readonly PauseMenuUIState _pauseMenuUIState;

	public PauseMenuGameState(
		PauseMenuUIState pauseMenuUIState, 
		PauseMenuInputManager pauseMenuInputManager)
	{
		_pauseMenuUIState = pauseMenuUIState;
		InputState = pauseMenuInputManager;
		
		UIStates.Add(_pauseMenuUIState);
	}

	public event Action ResumeGameRequested;
	public event Action MainMenuRequested;
	public event Action OptionsMenuRequested;
	
	public override PauseMenuInputManager InputState { get; }

	public override void Start()
	{
		UIStateMachine.ChangeUIState(_pauseMenuUIState);

		InputState.ExitMenu.OnKeyUp += OnExitMenuKeyReleased;
		_pauseMenuUIState.ResumeButtonClicked += OnResumeButtonClicked;
		_pauseMenuUIState.OptionsButtonClicked += OnOptionsButtonClicked;
		_pauseMenuUIState.MainMenuButtonClicked += OnMainMenuButtonClicked;
	}

	public override void End()
	{
		InputState.ExitMenu.OnKeyUp -= OnExitMenuKeyReleased;
		_pauseMenuUIState.ResumeButtonClicked -= OnResumeButtonClicked;
		_pauseMenuUIState.OptionsButtonClicked -= OnOptionsButtonClicked;
		_pauseMenuUIState.MainMenuButtonClicked -= OnMainMenuButtonClicked;
	}
	
	private void OnExitMenuKeyReleased() => ResumeGameRequested?.Invoke();
	private void OnResumeButtonClicked() => ResumeGameRequested?.Invoke();
	private void OnOptionsButtonClicked() => OptionsMenuRequested?.Invoke();
	private void OnMainMenuButtonClicked() => MainMenuRequested?.Invoke();
}