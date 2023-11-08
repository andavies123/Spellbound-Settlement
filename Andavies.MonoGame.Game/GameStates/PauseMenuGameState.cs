using System;
using Andavies.MonoGame.UI.StateMachines;
using SpellboundSettlement.Inputs;
using SpellboundSettlement.UIStates;
using SpellboundSettlement.UIStates.PauseMenu;

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
		_pauseMenuUIState.ResumeButton.MousePressed += RaiseResumeGame;
		_pauseMenuUIState.MainMenuButton.MousePressed += RaiseMainMenu;
	}

	public override void End()
	{
		InputState.ExitMenu.OnKeyUp -= RaiseResumeGame;
		_pauseMenuUIState.ResumeButton.MousePressed -= RaiseResumeGame;
		_pauseMenuUIState.MainMenuButton.MousePressed -= RaiseMainMenu;
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