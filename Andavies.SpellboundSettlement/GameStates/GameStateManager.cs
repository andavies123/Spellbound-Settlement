using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.Globals;

namespace SpellboundSettlement.GameStates;

public class GameStateManager : IGameStateManager
{
	private readonly List<IGameState> _gameStates;
	
	private readonly MainMenuGameState _mainMenuGameState;
	private readonly LoadGameState _loadGameState;
	private readonly GameplayGameState _gameplayGameState;
	private readonly PauseMenuGameState _pauseMenuGameState;

	public GameStateManager(
		MainMenuGameState mainMenuGameState,
		LoadGameState loadGameState,
		GameplayGameState gameplayGameState,
		PauseMenuGameState pauseMenuGameState)
	{
		_mainMenuGameState = mainMenuGameState;
		_loadGameState = loadGameState;
		_gameplayGameState = gameplayGameState;
		_pauseMenuGameState = pauseMenuGameState;

		_gameStates = new List<IGameState>
		{
			_mainMenuGameState,
			_loadGameState,
			_gameplayGameState,
			_pauseMenuGameState
		};
	}
	
	public IGameState CurrentGameState { get; private set; }
	
	public void Init()
	{
		_gameStates.ForEach(gameState => gameState.Init());
	}

	public void LateInit()
	{
		_gameStates.ForEach(gameState => gameState.LateInit());
		
		SubscribeToGameStateEvents();
		
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

	private void SubscribeToGameStateEvents()
	{
		// Main Menu Game State
		_mainMenuGameState.PlayGameRequested += OnPlayGameRequested;
		_mainMenuGameState.QuitGameRequested += OnQuitGameRequested;
		
		// Load Game Game State
		_loadGameState.GameLoaded += OnGameLoaded;
		_loadGameState.UnableToLoadGame += OnUnableToLoadGame;
		
		// Gameplay Game State
		_gameplayGameState.PauseGameRequested += OnPauseGameRequested;
		
		// Pause Menu Game State
		_pauseMenuGameState.ResumeGameRequested += OnResumeGameRequested;
		_pauseMenuGameState.OptionsMenuRequested += OnOptionsMenuRequested;
		_pauseMenuGameState.MainMenuRequested += OnMainMenuRequested;
	}

	private void UnsubscribeFromGameStateEvents()
	{
		// Main Menu Game State
		_mainMenuGameState.PlayGameRequested -= OnPlayGameRequested;
		_mainMenuGameState.QuitGameRequested -= OnQuitGameRequested;
		
		// Load Game Game State
		_loadGameState.GameLoaded -= OnGameLoaded;
		_loadGameState.UnableToLoadGame -= OnUnableToLoadGame;
		
		// Gameplay Game State
		_gameplayGameState.PauseGameRequested -= OnPauseGameRequested;
		
		// Pause Menu Game State
		_pauseMenuGameState.ResumeGameRequested -= OnResumeGameRequested;
		_pauseMenuGameState.OptionsMenuRequested -= OnOptionsMenuRequested;
		_pauseMenuGameState.MainMenuRequested -= OnMainMenuRequested;
	}

	// Main Menu Game State
	private void OnPlayGameRequested() => SetState(_loadGameState);
	private void OnQuitGameRequested() => Global.QuitGame();
	
	// Load Game Game State
	private void OnGameLoaded() => SetState(_gameplayGameState);
	private void OnUnableToLoadGame() => Console.WriteLine("Unable to Load Game");
	
	// Gameplay Game State
	private void OnPauseGameRequested() => SetState(_pauseMenuGameState);
	
	// Pause Menu Game State
	private void OnResumeGameRequested() => SetState(_gameplayGameState);
	private void OnOptionsMenuRequested() => SetState(_mainMenuGameState);
	private void OnMainMenuRequested() => SetState(_mainMenuGameState);
}