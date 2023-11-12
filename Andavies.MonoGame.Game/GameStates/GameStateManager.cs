using Andavies.MonoGame.Game.Server;
using Andavies.MonoGame.UI.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.Globals;

namespace SpellboundSettlement.GameStates;

public class GameStateManager : IGameStateManager
{
	private readonly IServerManager _serverManager;
	private readonly MainMenuGameState _mainMenuGameState;
	private readonly GameplayGameState _gameplayGameState;
	private readonly PauseMenuGameState _pauseMenuGameState;

	public GameStateManager(
		IServerManager serverManager,
		MainMenuGameState mainMenuGameState,
		GameplayGameState gameplayGameState,
		PauseMenuGameState pauseMenuGameState)
	{
		_serverManager = serverManager;
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
		
		SubscribeToEvents();
		
		// Set initial state
		SetState(_mainMenuGameState);
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

	private void SubscribeToEvents()
	{
		// Main Menu Events
		_mainMenuGameState.PlayGame += OnPlayGame;
		_mainMenuGameState.QuitGame += OnQuitGame;
		_mainMenuGameState.StartServerRequested += OnStartServerRequested;
		_mainMenuGameState.JoinServerRequested += OnJoinServerRequested;
		
		// Gameplay Events
		_gameplayGameState.PauseGame += OnPauseGame;
		
		// Pause Menu Events
		_pauseMenuGameState.ResumeGame += OnResumeGame;
		_pauseMenuGameState.MainMenu += OnMainMenu;
	}

	/*
	 * Todo: This needs to be called somewhere
	 * Todo: Possibly find a way to add these to a loop as there might be a lot
	 */
	private void UnsubscribeFromEvents()
	{
		// Main Menu Events
		_mainMenuGameState.PlayGame -= OnPlayGame;
		_mainMenuGameState.QuitGame -= OnQuitGame;
		_mainMenuGameState.StartServerRequested -= OnStartServerRequested;
		_mainMenuGameState.JoinServerRequested -= OnJoinServerRequested;
		
		// Gameplay Events
		_gameplayGameState.PauseGame -= OnPauseGame;
		
		// Pause Menu Events
		_pauseMenuGameState.ResumeGame -= OnResumeGame;
		_pauseMenuGameState.MainMenu -= OnMainMenu;
	}

	private void OnPauseGame() => SetState(_pauseMenuGameState);
	private void OnResumeGame() => SetState(_gameplayGameState);
	private void OnStartServerRequested(string ipAddress) => _serverManager.StartServer(ipAddress);
	private void OnJoinServerRequested(string ipAddress) { }
	
	private void OnMainMenu() => SetState(_mainMenuGameState);
	private void OnPlayGame(IUIElement uiElement) => SetState(_gameplayGameState);
	private void OnQuitGame(IUIElement uiElement) => Global.QuitGame();
}