using System;
using Andavies.MonoGame.UI.StateMachines;
using SpellboundSettlement.Inputs;
using SpellboundSettlement.UIStates;

namespace SpellboundSettlement.GameStates;

public class PauseMenuGameState : GameState
{
	public event Action ResumeGame;
	public event Action MainMenu;

	public override PauseMenuInputManager InputState { get; }

	private readonly PauseMenuUIState _pauseMenuUIState;

	public PauseMenuGameState(PauseMenuUIState pauseMenuUIState, PauseMenuInputManager pauseMenuInputManager)
	{
		_pauseMenuUIState = pauseMenuUIState;
		InputState = pauseMenuInputManager;
		
		UIStates.Add(_pauseMenuUIState);
	}

	public override void Start()
	{
		UIStateMachine.ChangeUIState(_pauseMenuUIState);
		
		InputState.ExitMenu.OnKeyUp += RaiseResumeGame;
		_pauseMenuUIState.ResumeButtonPressed += RaiseResumeGame;
		_pauseMenuUIState.MainMenuButtonPressed += RaiseMainMenu;
	}

	public override void End()
	{
		InputState.ExitMenu.OnKeyUp -= RaiseResumeGame;
		_pauseMenuUIState.ResumeButtonPressed -= RaiseResumeGame;
		_pauseMenuUIState.MainMenuButtonPressed -= RaiseMainMenu;
	}

	private void RaiseResumeGame()
	{
		ResumeGame?.Invoke();
	}

	private void RaiseMainMenu()
	{
		MainMenu?.Invoke();
	}
}