using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.CameraObjects;
using SpellboundSettlement.GameStates;

namespace SpellboundSettlement;

public class GameManager : Game
{
	private static GameManager Instance;
	private readonly GraphicsDeviceManager _graphics;
	
	private readonly IGameStateManager _gameStateManager;
	private readonly ICameraController _cameraController;
	
	private SpriteBatch _spriteBatch;
	
	// Update Times
	private DateTime _currentTime;
	private DateTime _previousTime;
	private TimeSpan _deltaTime;
	
	// Drawing
	public static Texture2D Texture;
	public static SpriteFont Font;
	public static Effect Effect;
	public static Viewport Viewport;

	public GameManager(
		IGameStateManager gameStateManager,
		ICameraController cameraController)
	{
		Instance = this;
		
		_gameStateManager = gameStateManager;
		_cameraController = cameraController;
		
		_graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;

		_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
		_graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
		
		_graphics.IsFullScreen = false;
		_graphics.ApplyChanges();
		Viewport = GraphicsDevice.Viewport;
	}

	public static void QuitGame() => Instance.Exit();

	protected override void Initialize()
	{	
		_previousTime = DateTime.Now;

		_cameraController.ResetCamera();
		
		// Set default texture
		Texture = new Texture2D(GraphicsDevice, 1, 1);
		Color[] data = new Color[1 * 1];
		for (int i = 0; i < data.Length; i++)
			data[i] = Color.White;
		Texture.SetData(data);
		
		_gameStateManager.Init();

		base.Initialize();
	}

	protected override void LoadContent()
	{
		_spriteBatch = new SpriteBatch(GraphicsDevice);
		
		Effect = Content.Load<Effect>("TestShader");
		Font = Content.Load<SpriteFont>("TestFont");
		
		_gameStateManager.LateInit();
	}

	protected override void Update(GameTime gameTime)
	{
		_currentTime = DateTime.Now;
		_deltaTime = _currentTime - _previousTime;

		float deltaTimeSeconds = (float) _deltaTime.TotalSeconds;
		
		_gameStateManager.Update(deltaTimeSeconds);
		_cameraController.UpdateCamera(deltaTimeSeconds);

		base.Update(gameTime);
		_previousTime = _currentTime;
	}
    
	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.CornflowerBlue);
		
		// Draw 3D
		_gameStateManager.Draw3D(GraphicsDevice);
		
		// DrawUI UI
		GraphicsDevice.DepthStencilState = DepthStencilState.None;
		_spriteBatch.Begin();
		
		_gameStateManager.DrawUI(_spriteBatch);
		
		_spriteBatch.End();
		GraphicsDevice.DepthStencilState = DepthStencilState.Default;
		
		base.Draw(gameTime);
	}
}