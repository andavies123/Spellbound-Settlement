using System;
using Autofac;
using SpellboundSettlement.Inputs;
using SpellboundSettlement.UIStates;
using UI.StateMachines;

namespace SpellboundSettlement.GameStates;

public class PauseMenuGameState : GameState
{
	public event Action ResumeGame;
	
	public override IUIState UIState { get; } = new PauseMenuUIState();
	public override IInputManager InputState { get; } = Program.Container.Resolve<PauseMenuInputManager>();

	public override void Start()
	{
		((PauseMenuInputManager) InputState).ExitMenu.OnKeyUp += RaiseResumeGame;
		((PauseMenuUIState) UIState).ResumeButtonPressed += RaiseResumeGame;
	}

	public override void End()
	{
		((PauseMenuInputManager) InputState).ExitMenu.OnKeyUp -= RaiseResumeGame;
		((PauseMenuUIState) UIState).ResumeButtonPressed -= RaiseResumeGame;
	}

	private void RaiseResumeGame()
	{
		ResumeGame?.Invoke();
	}
}