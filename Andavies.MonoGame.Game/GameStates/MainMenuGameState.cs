using System;
using SpellboundSettlement.Inputs;
using SpellboundSettlement.UIStates;

namespace SpellboundSettlement.GameStates;

public class MainMenuGameState : GameState
{
	public override MainMenuUIState UIState { get; }
	public override IInputManager InputState { get; }
	
	public event Action PlayGame
	{
		add => UIState.PlayButtonPressed += value;
		remove => UIState.PlayButtonPressed -= value;
	}

	public event Action QuitGame
	{
		add => UIState.QuitButtonPressed += value;
		remove => UIState.QuitButtonPressed -= value;
	}

	public MainMenuGameState(MainMenuUIState uiState, IInputManager inputManager)
	{
		UIState = uiState;
		InputState = inputManager;
	}
}