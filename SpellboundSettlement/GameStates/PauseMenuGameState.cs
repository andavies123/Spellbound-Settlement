using System;
using SpellboundSettlement.Inputs;
using SpellboundSettlement.UIStates;

namespace SpellboundSettlement.GameStates;

public class PauseMenuGameState : GameState
{
	public event Action ResumeGame;
	
	public override PauseMenuUIState UIState { get; }
	public override PauseMenuInputManager InputState { get; }

	public PauseMenuGameState(PauseMenuUIState pauseMenuUIState, PauseMenuInputManager pauseMenuInputManager)
	{
		UIState = pauseMenuUIState;
		InputState = pauseMenuInputManager;
	}

	public override void Start()
	{
		InputState.ExitMenu.OnKeyUp += RaiseResumeGame;
		UIState.ResumeButtonPressed += RaiseResumeGame;
	}

	public override void End()
	{
		InputState.ExitMenu.OnKeyUp -= RaiseResumeGame;
		UIState.ResumeButtonPressed -= RaiseResumeGame;
	}

	private void RaiseResumeGame()
	{
		ResumeGame?.Invoke();
	}
}