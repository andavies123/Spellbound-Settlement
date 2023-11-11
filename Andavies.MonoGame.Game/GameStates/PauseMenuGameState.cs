using System;
using Andavies.MonoGame.UI.Interfaces;
using SpellboundSettlement.Inputs;
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
		
		InputState.ExitMenu.OnKeyUp += RaiseKeyPressResumeGame;
		_pauseMenuUIState.ResumeButton.MouseClicked += RaiseResumeGame;
		_pauseMenuUIState.MainMenuButton.MouseClicked += RaiseMainMenu;
	}

	public override void End()
	{
		InputState.ExitMenu.OnKeyUp -= RaiseKeyPressResumeGame;
		_pauseMenuUIState.ResumeButton.MouseClicked -= RaiseResumeGame;
		_pauseMenuUIState.MainMenuButton.MouseClicked -= RaiseMainMenu;
	}

	private void RaiseKeyPressResumeGame()
	{
		ResumeGame?.Invoke();
	}

	private void RaiseResumeGame(IUIElement uiElement)
	{
		ResumeGame?.Invoke();
	}

	private void RaiseMainMenu(IUIElement uiElement)
	{
		MainMenu?.Invoke();
	}
}