using System;
using System.Collections.Generic;
using System.Linq;
using Andavies.MonoGame.Drawing;
using Andavies.MonoGame.UI.Styles;
using Andavies.MonoGame.Utilities.Interfaces;
using Andavies.SpellboundSettlement.GameStates;
using Andavies.SpellboundSettlement.GameWorld;
using Andavies.SpellboundSettlement.Globals;
using Andavies.SpellboundSettlement.Repositories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Serilog;
using IUpdateable = Andavies.MonoGame.Utilities.Interfaces.IUpdateable;

namespace Andavies.SpellboundSettlement;

public class GameManager : Game
{
	private readonly ILogger _logger;
	private readonly IGameStateManager _gameStateManager;
	private readonly IUIStyleRepository _uiStyleCollection;
	private readonly IFontRepository _fontRepository;
	private readonly ITileRegister _tileRegister;
	private readonly IEnumerable<IInitializable> _initializables;
	private readonly IEnumerable<ILateInitializable> _lateInitializables;
	private readonly IEnumerable<IUpdateable> _updateables;
	
	// Drawing
	public static Texture2D Texture;
	public static Effect Effect;
	public static Viewport Viewport;

	public GameManager(
		ILogger logger,
		IGameStateManager gameStateManager,
		IUIStyleRepository uiStyleCollection,
		IFontRepository fontRepository,
		ITileRegister tileRegister,
		IEnumerable<IInitializable> initializables,
		IEnumerable<ILateInitializable> lateInitializables,
		IEnumerable<IUpdateable> updateables)
	{
		Global.GameManager = this;
		
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_gameStateManager = gameStateManager ?? throw new ArgumentNullException(nameof(gameStateManager));
		_uiStyleCollection = uiStyleCollection ?? throw new ArgumentNullException(nameof(uiStyleCollection));
		_fontRepository = fontRepository ?? throw new ArgumentNullException(nameof(fontRepository));
		_tileRegister = tileRegister ?? throw new ArgumentNullException(nameof(tileRegister));
		_initializables = initializables ?? throw new ArgumentNullException(nameof(initializables));
		_lateInitializables = lateInitializables ?? throw new ArgumentNullException(nameof(lateInitializables));
		_updateables = updateables ?? throw new ArgumentNullException(nameof(updateables));

		_initializables = _initializables.OrderBy(initializable => initializable.InitOrder).ToList();
		_lateInitializables = _lateInitializables.OrderBy(lateInitializable => lateInitializable.LateInitOrder).ToList();
		_updateables = _updateables.OrderBy(updateable => updateable.UpdateOrder).ToList();

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
		
		// Set default texture
		Texture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
		Texture.SetData(new[]{ Color.White });
		SpriteBatchExtensions.InitializePixelTexture(Texture); // Init SpriteBatchExtensions to allow drawing

		foreach (IInitializable initializable in _initializables)
		{
			initializable.Init();
		}

		base.Initialize(); // Calls LoadContent and updates all GameComponents
		
		Global.SpriteBatch = new SpriteBatch(GraphicsDevice);
		InitializeUIStyleCollection();
		_tileRegister.RegisterTiles();

		foreach (ILateInitializable lateInitializable in _lateInitializables)
		{
			lateInitializable.LateInit();
		}
	}

	protected override void LoadContent()
	{
		_logger.Debug("Loading Content...");
		Effect = Content.Load<Effect>("Shaders/BaseShader");

		InitializeFontRepository();
	}
	
	protected override void Update(GameTime gameTime)
	{
		foreach (IUpdateable updateable in _updateables)
		{
			if (updateable.UpdateEnabled)
				updateable.Update(gameTime);
		}

		base.Update(gameTime);
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
		_fontRepository.TryGetFont("Default", out SpriteFont defaultFont);
		_fontRepository.TryGetFont("TextInputHint", out SpriteFont textInputHintFont);
		
		_uiStyleCollection.DefaultButtonStyle = new ButtonStyle
		{
			Font = defaultFont,
			BackgroundColor = Color.LightSlateGray,
			HoverBackgroundColor = Color.SlateGray,
			MousePressedBackgroundColor = Color.DarkSlateGray,
			DisabledBackgroundColor = new Color(.4f, .3f, .3f),
			BackgroundTexture = Texture
		};

		_uiStyleCollection.DefaultLabelStyle = new LabelStyle
		{
			Font = defaultFont,
			BackgroundColor = Color.Transparent,
			BackgroundTexture = Texture
		};

		_uiStyleCollection.DefaultTextInputStyle = new TextInputStyle
		{
			Font = defaultFont,
			HintTextFont = textInputHintFont,
			BackgroundColor = Color.LightGray,
			BackgroundTexture = Texture
		};
	}

	private void InitializeFontRepository()
	{
		_fontRepository.TryAddFont("Default", Content.Load<SpriteFont>("TestFont"));
		_fontRepository.TryAddFont("TextInputHint", Content.Load<SpriteFont>("HintFont"));
	}
}