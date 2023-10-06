using Microsoft.Xna.Framework.Graphics;

namespace SpellboundSettlement.GameStates;

public class GameStateManager : IGameStateManager
{
	private readonly GameplayGameState _gameplayGameState;
	private readonly PauseMenuGameState _pauseMenuGameState;

	public GameStateManager(
		GameplayGameState gameplayGameState,
		PauseMenuGameState pauseMenuGameState)
	{
		_gameplayGameState = gameplayGameState;
		_pauseMenuGameState = pauseMenuGameState;
	}
	
	public IGameState CurrentGameState { get; private set; }
	
	public void Init()
	{
		_gameplayGameState.Init();
		_pauseMenuGameState.Init();
		
		// Set initial state
		SetState(_gameplayGameState);

		_gameplayGameState.PauseGame += OnPauseGame;
		_pauseMenuGameState.ResumeGame += OnResumeGame;
		_pauseMenuGameState.QuitGame += OnQuitGame;
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

	private void OnQuitGame()
	{
		GameManager.QuitGame();
	}
}