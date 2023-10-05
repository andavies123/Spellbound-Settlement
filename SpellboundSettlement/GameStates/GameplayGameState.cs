using System;
using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.Inputs;
using SpellboundSettlement.UIStates;
using UI.StateMachines;

namespace SpellboundSettlement.GameStates;

public class GameplayGameState : IGameState
{
	public event Action PauseGame;
	
	public IUIState UIState { get; } = new GameplayUIState();
	public IInputManager InputState { get; } = new GameplayInputManager();

	public void Init()
	{
		UIState.Init();
	}

	public void LateInit()
	{
		UIState.LateInit();
	}

	public void Start()
	{
		((GameplayUIState) UIState).PauseButtonPressed += RaisePauseGame;
		((GameplayInputManager) InputState).PauseGame.OnKeyUp += RaisePauseGame;
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
		((GameplayUIState) UIState).PauseButtonPressed -= RaisePauseGame;
		((GameplayInputManager) InputState).PauseGame.OnKeyUp -= RaisePauseGame;
	}

	private void RaisePauseGame()
	{
		PauseGame?.Invoke();
	}
}