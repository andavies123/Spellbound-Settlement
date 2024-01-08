﻿using System;
using System.Collections.Generic;
using System.IO;
using Andavies.MonoGame.Drawing;
using Andavies.MonoGame.Inputs;
using Andavies.MonoGame.UI.Styles;
using Andavies.SpellboundSettlement.CameraObjects;
using Andavies.SpellboundSettlement.GameStates;
using Andavies.SpellboundSettlement.GameWorld;
using Andavies.SpellboundSettlement.Globals;
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
	private readonly ITileLoader _tileLoader;
	private readonly ITileRepository _tileRepository;
	private readonly IModelRepository _modelRepository;
	
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
		IUIStyleRepository uiStyleCollection,
		ITileLoader tileLoader,
		ITileRepository tileRepository,
		IModelRepository modelRepository)
	{
		Global.GameManager = this;

		_logger = logger;
		_gameStateManager = gameStateManager;
		_cameraController = cameraController;
		_uiStyleCollection = uiStyleCollection ?? throw new ArgumentNullException(nameof(uiStyleCollection));
		_tileLoader = tileLoader ?? throw new ArgumentNullException(nameof(tileLoader));
		_tileRepository = tileRepository ?? throw new ArgumentNullException(nameof(tileRepository));
		_modelRepository = modelRepository ?? throw new ArgumentNullException(nameof(modelRepository));
		
		Global.GraphicsDeviceManager = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;
		
		// Frame rate settings
		IsFixedTimeStep = false;
		Global.GraphicsDeviceManager.SynchronizeWithVerticalRetrace = true; // VSync

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
		Texture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
		Texture.SetData(new[]{ Color.White });
		SpriteBatchExtensions.InitializePixelTexture(Texture); // Initialize SpriteBatchExtensions to allow drawing
		
		_gameStateManager.Init();

		base.Initialize(); // Calls LoadContent
		
		Global.SpriteBatch = new SpriteBatch(GraphicsDevice);
		InitializeUIStyleCollection();
		
		_gameStateManager.LateInit();
	}

	protected override void LoadContent()
	{
		_logger.Debug("Loading Content...");
		Effect = Content.Load<Effect>("TestShader");
		GlobalFonts.DefaultFont = Content.Load<SpriteFont>("TestFont");
		GlobalFonts.HintFont = Content.Load<SpriteFont>("HintFont");

		InitializeTileRepository();
		InitializeModelRepository();
	}

	private DateTime _fpsUpdateTime = DateTime.Now;
	private int _frameCount = 0;

	protected override void Update(GameTime gameTime)
	{
		_currentTime = DateTime.Now;
		_deltaTime = _currentTime - _previousTime;

		float deltaTimeSeconds = (float) _deltaTime.TotalSeconds;

		// Frame count
		_frameCount++;
		if ((_currentTime - _fpsUpdateTime).TotalSeconds >= 1.0)
		{
			_logger.Debug("FPS: {fps}", _frameCount);
			_frameCount = 0;
			_fpsUpdateTime = _currentTime;
		}

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

	private void InitializeTileRepository()
	{
		string path = Path.Combine(Environment.CurrentDirectory, @"Assets\Config\world_tiles.json");
		_tileLoader.LoadTilesFromJson(path, _tileRepository);
	}

	private void InitializeModelRepository()
	{
		List<ModelTileDetails> modelTileDetailsList = _tileRepository.GetAllTileDetailsOfType<ModelTileDetails>();
		
		foreach (ModelTileDetails modelTileDetails in modelTileDetailsList)
		{
			_modelRepository.TryAddModel(modelTileDetails.ContentModelPath, Content.Load<Model>(modelTileDetails.ContentModelPath));
		}
	}
}