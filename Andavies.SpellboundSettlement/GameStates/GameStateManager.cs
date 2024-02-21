using System.Collections.Generic;
using Andavies.MonoGame.Utilities.Interfaces;
using Andavies.SpellboundSettlement.Globals;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Serilog;
using IUpdateable = Andavies.MonoGame.Utilities.Interfaces.IUpdateable;

namespace Andavies.SpellboundSettlement.GameStates;

public class GameStateManager : IGameStateManager, IInitializable, ILateInitializable, IUpdateable
{
	private readonly List<IGameState> _gameStates;

	private readonly ILogger _logger;
	private readonly MainMenuGameState _mainMenuGameState;
	private readonly LoadGameState _loadGameState;
	private readonly GameplayGameState _gameplayGameState;
	private readonly PauseMenuGameState _pauseMenuGameState;

	public GameStateManager(
		ILogger logger,
		MainMenuGameState mainMenuGameState,
		LoadGameState loadGameState,
		GameplayGameState gameplayGameState,
		PauseMenuGameState pauseMenuGameState,
		int initOrder,
		int lateInitOrder,
		int updateOrder)
	{
		_logger = logger;
		_mainMenuGameState = mainMenuGameState;
		_loadGameState = loadGameState;
		_gameplayGameState = gameplayGameState;
		_pauseMenuGameState = pauseMenuGameState;
		InitOrder = initOrder;
		LateInitOrder = lateInitOrder;
		UpdateOrder = updateOrder;

		_gameStates = new List<IGameState>
		{
			_mainMenuGameState,
			_loadGameState,
			_gameplayGameState,
			_pauseMenuGameState
		};
	}
	
	public IGameState CurrentGameState { get; private set; }

	public bool UpdateEnabled { get; set; } = true;
	
	public int InitOrder { get; }
	public int LateInitOrder { get; }
	public int UpdateOrder { get; }
	
	public void Update(GameTime gameTime)
	{
		CurrentGameState.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
	}

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
	private void OnUnableToLoadGame() => _logger.Information("Unable to Load Game");
	
	// Gameplay Game State
	private void OnPauseGameRequested() => SetState(_pauseMenuGameState);
	
	// Pause Menu Game State
	private void OnResumeGameRequested() => SetState(_gameplayGameState);
	private void OnOptionsMenuRequested() => SetState(_mainMenuGameState);
	private void OnMainMenuRequested() => SetState(_mainMenuGameState);
}