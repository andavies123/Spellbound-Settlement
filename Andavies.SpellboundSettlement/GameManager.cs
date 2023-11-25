using System;
using Andavies.MonoGame.Inputs;
using Andavies.MonoGame.UI.Styles;
using Andavies.SpellboundSettlement.CameraObjects;
using Andavies.SpellboundSettlement.GameStates;
using Andavies.SpellboundSettlement.Globals;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Serilog;

namespace Andavies.SpellboundSettlement;

public class GameManager : Game
{
	private readonly ILogger _logger;
	private readonly IGameStateManager _gameStateManager;
	private readonly ICameraController _cameraController;
	private readonly IUIStyleRepository _uiStyleCollection;
	
	// Update Times
	private DateTime _currentTime;
	private DateTime _previousTime;
	private TimeSpan _deltaTime;
	
	// Drawing
	public static Texture2D Texture;
	public static Effect Effect;
	public static Viewport Viewport;

	public GameManager(
		ILogger logger,
		IGameStateManager gameStateManager,
		ICameraController cameraController,
		IUIStyleRepository uiStyleCollection)
	{
		Global.GameManager = this;

		_logger = logger;
		_gameStateManager = gameStateManager;
		_cameraController = cameraController;
		_uiStyleCollection = uiStyleCollection;
		
		Global.GraphicsDeviceManager = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;

		Global.GraphicsDeviceManager.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
		Global.GraphicsDeviceManager.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
		
		Global.GraphicsDeviceManager.IsFullScreen = false;
		Global.GraphicsDeviceManager.ApplyChanges();
		Viewport = GraphicsDevice.Viewport;
	}

	protected override void Initialize()
	{
		_logger.Debug("Initializing Game...");
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
		InitializeUIStyleCollection();
		
		_gameStateManager.LateInit();
		
		//_client.Start();
	}

	protected override void LoadContent()
	{
		_logger.Debug("Loading Content...");
		Effect = Content.Load<Effect>("TestShader");
		GlobalFonts.DefaultFont = Content.Load<SpriteFont>("TestFont");
		GlobalFonts.HintFont = Content.Load<SpriteFont>("HintFont");
	}

	protected override void Update(GameTime gameTime)
	{
		_currentTime = DateTime.Now;
		_deltaTime = _currentTime - _previousTime;

		float deltaTimeSeconds = (float) _deltaTime.TotalSeconds;

		Input.Update();
		
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

	private void InitializeUIStyleCollection()
	{
		_uiStyleCollection.DefaultButtonStyle = new ButtonStyle
		{
			Font = GlobalFonts.DefaultFont,
			BackgroundColor = Color.LightSlateGray,
			HoverBackgroundColor = Color.SlateGray,
			MousePressedBackgroundColor = Color.DarkSlateGray,
			DisabledBackgroundColor = new Color(.4f, .3f, .3f),
			BackgroundTexture = Texture
		};

		_uiStyleCollection.DefaultLabelStyle = new LabelStyle
		{
			Font = GlobalFonts.DefaultFont,
			BackgroundColor = Color.Transparent,
			BackgroundTexture = Texture
		};

		_uiStyleCollection.DefaultTextInputStyle = new TextInputStyle
		{
			Font = GlobalFonts.DefaultFont,
			HintTextFont = GlobalFonts.HintFont,
			BackgroundColor = Color.LightGray,
			BackgroundTexture = Texture
		};
	}
}