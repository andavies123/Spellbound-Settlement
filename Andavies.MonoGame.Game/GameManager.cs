using System;
using Andavies.MonoGame.Game.Client;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.CameraObjects;
using SpellboundSettlement.GameStates;
using SpellboundSettlement.Globals;

namespace SpellboundSettlement;

public class GameManager : Game
{
	private readonly IGameStateManager _gameStateManager;
	private readonly ICameraController _cameraController;
	private readonly LiteNetClient _client;
	
	// Update Times
	private DateTime _currentTime;
	private DateTime _previousTime;
	private TimeSpan _deltaTime;
	
	// Drawing
	public static Texture2D Texture;
	public static Effect Effect;
	public static Viewport Viewport;

	public GameManager(
		IGameStateManager gameStateManager,
		ICameraController cameraController)
	{
		Global.GameManager = this;
		
		_gameStateManager = gameStateManager;
		_cameraController = cameraController;
		
		Global.GraphicsDeviceManager = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;

		Global.GraphicsDeviceManager.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
		Global.GraphicsDeviceManager.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
		
		Global.GraphicsDeviceManager.IsFullScreen = true;
		Global.GraphicsDeviceManager.ApplyChanges();
		Viewport = GraphicsDevice.Viewport;
	}

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

		base.Initialize(); // Calls LoadContent
		
		Global.SpriteBatch = new SpriteBatch(GraphicsDevice);
		_gameStateManager.LateInit();
		
		//_client.Start();
	}

	protected override void LoadContent()
	{
		Effect = Content.Load<Effect>("TestShader");
		GlobalFonts.DefaultFont = Content.Load<SpriteFont>("TestFont");
		GlobalFonts.HintFont = Content.Load<SpriteFont>("HintFont");
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
		Global.SpriteBatch.Begin();
		_gameStateManager.DrawUI(Global.SpriteBatch);
		Global.SpriteBatch.End();
		GraphicsDevice.DepthStencilState = DepthStencilState.Default;
		
		base.Draw(gameTime);
	}
}