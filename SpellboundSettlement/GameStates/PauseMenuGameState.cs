using System;
using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.Inputs;
using SpellboundSettlement.UIStates;
using UI.StateMachines;

namespace SpellboundSettlement.GameStates;

public class PauseMenuGameState : IGameState
{
	public event Action ResumeGame;
	
	public IUIState UIState { get; } = new PauseMenuUIState();
	public IInputManager InputState { get; } = new PauseMenuInputManager();

	public void Init() { }
	public void LateInit() { }

	public void Start()
	{
		((PauseMenuInputManager) InputState).ExitMenu.OnKeyUp += RaiseResumeGame;
	}
	
	public void Update(float deltaTimeSeconds)
	{
		InputState.UpdateInput();
		UIState.Update(deltaTimeSeconds);
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		UIState.Draw(spriteBatch);
	}

	public void End()
	{
		((PauseMenuInputManager) InputState).ExitMenu.OnKeyUp -= RaiseResumeGame;
	}

	private void RaiseResumeGame()
	{
		ResumeGame?.Invoke();
	}
}