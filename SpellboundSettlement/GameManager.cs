using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.CameraObjects;
using SpellboundSettlement.Global;
using SpellboundSettlement.Inputs;
using SpellboundSettlement.Meshes;
using SpellboundSettlement.WorldObjects;

namespace SpellboundSettlement;

public class GameManager : Game
{
	private readonly GraphicsDeviceManager _graphics;
	private readonly GameplayInputManager _gameplayInput = new();
	private readonly Camera _camera = new();
	private readonly ICameraController _cameraController;
	
	private SpriteBatch _spriteBatch;
	private Effect _effect;
	
	// World
	private readonly World _world = new((0, 0), 5);
	private WorldMesh _worldMesh;
	
	// Update Times
	private DateTime _currentTime;
	private DateTime _previousTime;
	private TimeSpan _deltaTime;

	public GameManager()
	{
		_graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;

		_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
		_graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
		_graphics.IsFullScreen = false;
		_graphics.ApplyChanges();
		
		_cameraController = new WorldViewCameraController(_camera, _gameplayInput);
	}

	protected override void Initialize()
	{
		GameServices.AddService(this);
		GameServices.AddService<Game>(this);
		GameServices.AddService(_camera);
		GameServices.AddService(_graphics);
		GameServices.AddService(_gameplayInput);
		GameServices.AddService(_world);
		
		_effect = Content.Load<Effect>("TestShader");
		_previousTime = DateTime.Now;

		_cameraController.ResetCamera();
		_worldMesh = new WorldMesh(_world);

		base.Initialize();
	}

	protected override void LoadContent()
	{
		_spriteBatch = new SpriteBatch(GraphicsDevice);
	}

	protected override void Update(GameTime gameTime)
	{
		_currentTime = DateTime.Now;
		_deltaTime = _currentTime - _previousTime;
		
		_gameplayInput.UpdateInput();
		_cameraController.UpdateCamera((float)_deltaTime.TotalSeconds);

		base.Update(gameTime);
		_previousTime = _currentTime;
	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.CornflowerBlue);
		
		// Draw World
		foreach (ChunkMesh chunkMesh in _worldMesh.ChunkMeshes.Values)
			DrawMesh(chunkMesh);

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