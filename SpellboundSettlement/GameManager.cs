using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.CameraObjects;
using SpellboundSettlement.Inputs;
using SpellboundSettlement.Meshes;
using SpellboundSettlement.UIStates;
using SpellboundSettlement.WorldObjects;

namespace SpellboundSettlement;

public class GameManager : Game
{
	private readonly GraphicsDeviceManager _graphics;
	private readonly IInputStateMachine _inputStateMachine;
	private readonly IUIStateMachine _uiStateMachine;
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
		IInputStateMachine inputStateMachine,
		IUIStateMachine uiStateMachine,
		ICameraController cameraController,
		GameplayInputManager gameplayInput,
		PauseMenuInputManager pauseMenuInput,
		Camera camera)
	{
		_inputStateMachine = inputStateMachine ?? throw new ArgumentNullException();
		_uiStateMachine = uiStateMachine ?? throw new ArgumentNullException();
		_cameraController = cameraController ?? throw new ArgumentNullException();
		_camera = camera ?? throw new ArgumentNullException();
		
		// Todo: Move this to a separate location that handles all input/UI states
		_inputStateMachine.ChangeInputManager(gameplayInput);

		// Todo: Move this logic to a better location that handles pausing/resuming the game
		gameplayInput.PauseGame.OnKeyUp += () => _inputStateMachine.ChangeInputManager(pauseMenuInput);
		pauseMenuInput.ExitMenu.OnKeyUp += () => _inputStateMachine.ChangeInputManager(gameplayInput);
		
		_graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;

		//_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
		_graphics.PreferredBackBufferWidth = 1920;
		//_graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
		_graphics.PreferredBackBufferHeight = 1080;
		
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

		base.Initialize();
	}

	protected override void LoadContent()
	{
		_spriteBatch = new SpriteBatch(GraphicsDevice);
		
		Texture = new Texture2D(GraphicsDevice, 1, 1);
		Color[] data = new Color[1 * 1];
		for (int i = 0; i < data.Length; i++)
			data[i] = Color.White;
		Texture.SetData(data);
		
		Font = Content.Load<SpriteFont>("TestFont");
		// Todo: Move
		_uiStateMachine.ChangeUIState(new GameplayUIState());
	}

	protected override void Update(GameTime gameTime)
	{
		_currentTime = DateTime.Now;
		_deltaTime = _currentTime - _previousTime;
		
		_inputStateMachine.Update();
		_cameraController.UpdateCamera((float)_deltaTime.TotalSeconds);
		_uiStateMachine.Update();

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
		
		_uiStateMachine.Draw(_spriteBatch);
		
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