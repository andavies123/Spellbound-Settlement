using System;
using SpellboundSettlement.Inputs;
using SpellboundSettlement.UIStates;

namespace SpellboundSettlement.GameStates;

public class PauseMenuGameState : GameState
{
	public event Action ResumeGame;
	public event Action MainMenu;
	
	public override PauseMenuUIState UIState { get; }
	public override PauseMenuInputManager InputState { get; }

	public PauseMenuGameState(PauseMenuUIState pauseMenuUIState, PauseMenuInputManager pauseMenuInputManager)
	{
		UIState = pauseMenuUIState;
		InputState = pauseMenuInputManager;
	}

	public override void Start()
	{
		base.Start();
		
		InputState.ExitMenu.OnKeyUp += RaiseResumeGame;
		UIState.ResumeButtonPressed += RaiseResumeGame;
		UIState.MainMenuButtonPressed += RaiseMainMenu;
	}

	public override void End()
	{
		base.End();
		
		InputState.ExitMenu.OnKeyUp -= RaiseResumeGame;
		UIState.ResumeButtonPressed -= RaiseResumeGame;
		UIState.MainMenuButtonPressed -= RaiseMainMenu;
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