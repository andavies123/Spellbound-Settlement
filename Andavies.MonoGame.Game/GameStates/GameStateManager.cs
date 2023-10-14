using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.Globals;

namespace SpellboundSettlement.GameStates;

public class GameStateManager : IGameStateManager
{
	private readonly MainMenuGameState _mainMenuGameState;
	private readonly GameplayGameState _gameplayGameState;
	private readonly PauseMenuGameState _pauseMenuGameState;

	public GameStateManager(
		MainMenuGameState mainMenuGameState,
		GameplayGameState gameplayGameState,
		PauseMenuGameState pauseMenuGameState)
	{
		_mainMenuGameState = mainMenuGameState;
		_gameplayGameState = gameplayGameState;
		_pauseMenuGameState = pauseMenuGameState;
	}
	
	public IGameState CurrentGameState { get; private set; }
	
	public void Init()
	{
		_mainMenuGameState.Init();
		_gameplayGameState.Init();
		_pauseMenuGameState.Init();
		
		// Set initial state
		SetState(_mainMenuGameState);
	}

	public void LateInit()
	{
		_mainMenuGameState.LateInit();
		_gameplayGameState.LateInit();
		_pauseMenuGameState.LateInit();

		// Todo: Move these events elsewhere since there will probably be a lot of them
		_mainMenuGameState.PlayGame += OnPlayGame;
		_mainMenuGameState.QuitGame += OnQuitGame;
		_gameplayGameState.PauseGame += OnPauseGame;
		_pauseMenuGameState.ResumeGame += OnResumeGame;
		_pauseMenuGameState.MainMenu += OnMainMenu;
	}

	public void Update(float deltaTimeSeconds)
	{
		CurrentGameState.Update(deltaTimeSeconds);
	}

	public void Draw3D(GraphicsDevice graphicsDevice)
	{
		CurrentGameState.Draw3D(graphicsDevice);
	}

	public void DrawUI(SpriteBatch spriteBatch)
	{
		CurrentGameState.DrawUI(spriteBatch);
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

	private void OnMainMenu()
	{
		SetState(_mainMenuGameState);
	}

	private void OnPlayGame()
	{
		SetState(_gameplayGameState);
	}
	
	private void OnQuitGame()
	{
		Global.QuitGame();
	}
}