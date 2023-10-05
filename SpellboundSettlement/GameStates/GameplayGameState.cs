using System;
using Autofac;
using SpellboundSettlement.Inputs;
using SpellboundSettlement.UIStates;
using UI.StateMachines;

namespace SpellboundSettlement.GameStates;

public class GameplayGameState : GameState
{
	public event Action PauseGame;
	
	public override IUIState UIState { get; } = new GameplayUIState();
	public override IInputManager InputState { get; } = Program.Container.Resolve<GameplayInputManager>();

	public override void Start()
	{
		base.Start();
		
		((GameplayUIState) UIState).PauseButtonPressed += RaisePauseGame;
		((GameplayInputManager) InputState).PauseGame.OnKeyUp += RaisePauseGame;
	}

	public override void End()
	{
		((GameplayUIState) UIState).PauseButtonPressed -= RaisePauseGame;
		((GameplayInputManager) InputState).PauseGame.OnKeyUp -= RaisePauseGame;
	}

	private void RaisePauseGame()
	{
		PauseGame?.Invoke();
	}
}