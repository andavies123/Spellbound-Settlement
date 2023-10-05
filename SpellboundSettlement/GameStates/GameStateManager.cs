using Microsoft.Xna.Framework.Graphics;

namespace SpellboundSettlement.GameStates;

public class GameStateManager : IGameStateManager
{
	private readonly IGameState _gameplayGameState = new GameplayGameState();
	private readonly IGameState _pauseMenuGameState = new PauseMenuGameState();
	
	public IGameState CurrentGameState { get; private set; }
	
	public void Init()
	{
		_gameplayGameState.Init();
		_pauseMenuGameState.Init();
		
		// Set initial state
		SetState(_gameplayGameState);

		((GameplayGameState) _gameplayGameState).PauseGame += OnPauseGame;
		((PauseMenuGameState) _pauseMenuGameState).ResumeGame += OnResumeGame;
	}

	public void LateInit()
	{
		_gameplayGameState.LateInit();
		_pauseMenuGameState.LateInit();
	}

	public void Update(float deltaTimeSeconds)
	{
		CurrentGameState.Update(deltaTimeSeconds);
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		CurrentGameState.Draw(spriteBatch);
	}

	public void SetState(IGameState nextState)
	{
		CurrentGameState?.End();
		CurrentGameState = nextState;
		CurrentGameState?.Start();
	}

	private void OnPauseGame()
	{
		SetState(_pauseMenuGameState);
	}

	private void OnResumeGame()
	{
		SetState(_gameplayGameState);
	}
}