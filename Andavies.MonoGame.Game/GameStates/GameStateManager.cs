using Andavies.MonoGame.Game.Server;
using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.Globals;

namespace SpellboundSettlement.GameStates;

public class GameStateManager : IGameStateManager
{
	private readonly IServerStarter _serverStarter;
	private readonly MainMenuGameState _mainMenuGameState;
	private readonly GameplayGameState _gameplayGameState;
	private readonly PauseMenuGameState _pauseMenuGameState;

	public GameStateManager(
		IServerStarter serverStarter,
		MainMenuGameState mainMenuGameState,
		GameplayGameState gameplayGameState,
		PauseMenuGameState pauseMenuGameState)
	{
		_serverStarter = serverStarter;
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
	}

	public void LateInit()
	{
		_mainMenuGameState.LateInit();
		_gameplayGameState.LateInit();
		_pauseMenuGameState.LateInit();

		// Todo: Move these events elsewhere since there will probably be a lot of them
		_mainMenuGameState.PlayGameRequested += OnPlayGameRequested;
		_mainMenuGameState.QuitGameRequested += OnQuitGameRequested;
		_gameplayGameState.PauseGameRequested += OnPauseGameRequested;
		_pauseMenuGameState.ResumeGameRequested += OnResumeGameRequested;
		_pauseMenuGameState.OptionsMenuRequested += OnOptionsMenuRequested;
		_pauseMenuGameState.MainMenuRequested += OnMainMenuRequested;
		
		// Set initial state
		SetState(_mainMenuGameState);
	}

	public void Update(float deltaTimeSeconds) => CurrentGameState.Update(deltaTimeSeconds);
	public void Draw3D(GraphicsDevice graphicsDevice) => CurrentGameState.Draw3D(graphicsDevice);
	public void DrawUI(SpriteBatch spriteBatch) => CurrentGameState.DrawUI(spriteBatch);

	public void SetState(IGameState nextState)
	{
		CurrentGameState?.End();
		CurrentGameState = nextState;
		CurrentGameState?.Start();
	}

	private void OnPauseGameRequested() => SetState(_pauseMenuGameState);
	private void OnPlayGameRequested() => SetState(_gameplayGameState);
	private void OnQuitGameRequested() => Global.QuitGame();
	
	// Pause Menu Game State
	private void OnResumeGameRequested() => SetState(_gameplayGameState);
	private void OnOptionsMenuRequested() => SetState(_mainMenuGameState);
	private void OnMainMenuRequested() => SetState(_mainMenuGameState);
}