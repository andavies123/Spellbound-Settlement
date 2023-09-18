using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.Global;
using SpellboundSettlement.Inputs;
using SpellboundSettlement.Meshes;

namespace SpellboundSettlement;

public class GameManager : Game
{
	private readonly GraphicsDeviceManager _graphics;
	private readonly GameplayInputManager _gameplayInput = new();
	private readonly Camera _camera;
	
	private SpriteBatch _spriteBatch;
	private Effect _effect;
	
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
		
		_camera = new Camera(GraphicsDevice.Viewport.AspectRatio);
	}

	private ChunkMesh _chunkMesh;
	protected override void Initialize()
	{
		GameServices.AddService(this);
		GameServices.AddService(_camera);
		GameServices.AddService(_graphics);
		GameServices.AddService(_gameplayInput);
		
		_effect = Content.Load<Effect>("TestShader");
		_previousTime = DateTime.Now;
		_chunkMesh = new ChunkMesh(new Vector3(5, 5, 5), Vector3.Zero);

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

		if (_gameplayInput.MoveCameraInput != Vector2.Zero)
		{
			Vector2 cameraMovement = _gameplayInput.MoveCameraInput * (float)_deltaTime.TotalSeconds;
			_camera.CameraPosition = new Vector3(_camera.CameraPosition.X + cameraMovement.X, _camera.CameraPosition.Y, _camera.CameraPosition.Z + cameraMovement.Y);
			_camera.CameraTarget = new Vector3(_camera.CameraTarget.X + cameraMovement.X, _camera.CameraTarget.Y, _camera.CameraTarget.Z + cameraMovement.Y);
			_camera.RecalculateViewMatrix();
		}

		base.Update(gameTime);
		_previousTime = _currentTime;
	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.CornflowerBlue);
		
		DrawSquare();

		base.Draw(gameTime);
	}

	private void DrawSquare()
	{
		VertexPositionColor[] vertices = _chunkMesh.Vertices;
		int[] indices = _chunkMesh.Indices;

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