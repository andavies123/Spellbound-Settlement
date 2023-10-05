using System;
using SpellboundSettlement.Inputs;
using SpellboundSettlement.UIStates;

namespace SpellboundSettlement.GameStates;

public class GameplayGameState : GameState
{
	public event Action PauseGame;
	
	public override GameplayUIState UIState { get; }
	public override GameplayInputManager InputState { get; }

	public GameplayGameState(GameplayUIState gameplayUIState, GameplayInputManager gameplayInputManager)
	{
		UIState = gameplayUIState;
		InputState = gameplayInputManager;
	}

	public override void Start()
	{
		base.Start();
		
		UIState.PauseButtonPressed += RaisePauseGame;
		InputState.PauseGame.OnKeyUp += RaisePauseGame;
	}

	public override void End()
	{
		UIState.PauseButtonPressed -= RaisePauseGame;
		InputState.PauseGame.OnKeyUp -= RaisePauseGame;
	}

	private void RaisePauseGame()
	{
		PauseGame?.Invoke();
	}
}