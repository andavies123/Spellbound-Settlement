using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.CameraObjects;
using SpellboundSettlement.GameStates;
using SpellboundSettlement.Inputs;
using SpellboundSettlement.Meshes;
using SpellboundSettlement.WorldObjects;

namespace SpellboundSettlement;

public class GameManager : Game
{
	private readonly GraphicsDeviceManager _graphics;
	
	private readonly IGameStateManager _gameStateManager;
	private readonly ICameraController _cameraController;
	private readonly Camera _camera;
	
	private SpriteBatch _spriteBatch;
	private Effect _effect;
	
	// World
	private readonly World _world = new((0, 0), 5);
	private WorldMesh _worldMesh;
	
	// Update Times
	private DateTime _currentTime;
	private DateTime _previousTime;
	private TimeSpan _deltaTime;
	
	// Drawing
	public static Texture2D Texture;
	public static SpriteFont Font;
	public static Viewport Viewport;

	public GameManager(
		IGameStateManager gameStateManager,
		ICameraController cameraController,
		GameplayInputManager gameplayInput,
		PauseMenuInputManager pauseMenuInput,
		Camera camera)
	{
		_gameStateManager = gameStateManager;
		
		_cameraController = cameraController ?? throw new ArgumentNullException();
		_camera = camera ?? throw new ArgumentNullException();
		
		// Todo: Move this to a separate location that handles all input/UI states
		gameStateManager.SetState(new GameplayGameState());

		// Todo: Move this logic to a better location that handles pausing/resuming the game
		
		_graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;

		_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
		_graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
		
		_graphics.IsFullScreen = false;
		_graphics.ApplyChanges();
		Viewport = GraphicsDevice.Viewport;
	}

	protected override void Initialize()
	{	
		_effect = Content.Load<Effect>("TestShader");
		_previousTime = DateTime.Now;

		_cameraController.ResetCamera();
		_worldMesh = new WorldMesh(_world);
		
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
		
		// Draw World
		foreach (ChunkMesh chunkMesh in _worldMesh.ChunkMeshes.Values)
			DrawMesh(chunkMesh);
		
		// Draw UI
		GraphicsDevice.DepthStencilState = DepthStencilState.None;
		_spriteBatch.Begin();
		
		_gameStateManager.Draw(_spriteBatch);
		
		_spriteBatch.End();
		GraphicsDevice.DepthStencilState = DepthStencilState.Default;
		
		base.Draw(gameTime);
	}
	
	private void DrawMesh(IMesh mesh)
	{
		VertexPositionColor[] vertices = mesh.Vertices;
		int[] indices = mesh.Indices;
		
		VertexBuffer vertexBuffer = new(GraphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.None);
		vertexBuffer.SetData(vertices);

		IndexBuffer indexBuffer = new(GraphicsDevice, IndexElementSize.ThirtyTwoBits, indices.Length, BufferUsage.None);
		indexBuffer.SetData(indices);
		
		GraphicsDevice.SetVertexBuffer(vertexBuffer);
		GraphicsDevice.Indices = indexBuffer;
		
		_effect.Parameters["WorldViewProjection"].SetValue(_camera.WorldViewProjection);
		_effect.CurrentTechnique.Passes[0].Apply();
		
		GraphicsDevice.DrawIndexedPrimitives(
			PrimitiveType.TriangleList,
			0,
			0,
			indexBuffer.IndexCount / 3);
	}
}