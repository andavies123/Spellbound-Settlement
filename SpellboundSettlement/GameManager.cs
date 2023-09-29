using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.CameraObjects;
using SpellboundSettlement.Inputs;
using SpellboundSettlement.Meshes;
using SpellboundSettlement.WorldObjects;

namespace SpellboundSettlement;

public class GameManager : Game
{
	private readonly GraphicsDeviceManager _graphics;
	private readonly IInputStateMachine _inputStateMachine;
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

	public GameManager(
		IInputStateMachine inputStateMachine,
		ICameraController cameraController,
		GameplayInputManager gameplayInput,
		PauseMenuInputManager pauseMenuInput,
		Camera camera)
	{
		_inputStateMachine = inputStateMachine ?? throw new ArgumentNullException();
		_cameraController = cameraController ?? throw new ArgumentNullException();
		_camera = camera ?? throw new ArgumentNullException();
		
		_inputStateMachine.ChangeInputManager(gameplayInput);

		// Todo: Move this logic to a better location that handles pausing/resuming the game
		gameplayInput.PauseGame += () => _inputStateMachine.ChangeInputManager(pauseMenuInput);
		pauseMenuInput.ExitMenu += () => _inputStateMachine.ChangeInputManager(gameplayInput);
		
		_graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;

		_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
		_graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
		_graphics.IsFullScreen = false;
		_graphics.ApplyChanges();
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
	}

	protected override void Update(GameTime gameTime)
	{
		_currentTime = DateTime.Now;
		_deltaTime = _currentTime - _previousTime;
		
		_inputStateMachine.Update();
		_cameraController.UpdateCamera((float)_deltaTime.TotalSeconds);

		base.Update(gameTime);
		_previousTime = _currentTime;
	}

	private readonly Stopwatch _drawStopwatch = new();
	private readonly Queue<long> _latestDrawTimes = new();
    
	protected override void Draw(GameTime gameTime)
	{
		#region Draw Timers (Remove when Necessary)

		_drawStopwatch.Restart();

		vertexCount = 0;
		indexCount = 0;

		#endregion
		
		GraphicsDevice.Clear(Color.CornflowerBlue);
		
		// Draw World
		foreach (ChunkMesh chunkMesh in _worldMesh.ChunkMeshes.Values)
			DrawMesh(chunkMesh);
		
		base.Draw(gameTime);

		#region Draw Timers (Remove when Necessary)

		_drawStopwatch.Stop();
		_latestDrawTimes.Enqueue(_drawStopwatch.ElapsedMilliseconds);
		if (_latestDrawTimes.Count > 240)
			_latestDrawTimes.Dequeue();
		Console.WriteLine($"{vertexCount} - {indexCount} - {_latestDrawTimes.Count} - {_latestDrawTimes.Average()}");

		#endregion
	}

	private int vertexCount = 0;
	private int indexCount = 0;
	
	// Before = 1,074,360 vertices & 1,611,540 indices & avg 26 sec
	// After = 146,960 vertices & 220,440 indices & avg 4 sec
	
	private void DrawMesh(IMesh mesh)
	{
		VertexPositionColor[] vertices = mesh.Vertices;
		int[] indices = mesh.Indices;
		
		VertexBuffer vertexBuffer = new(GraphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.None);
		vertexBuffer.SetData(vertices);

		IndexBuffer indexBuffer = new(GraphicsDevice, IndexElementSize.ThirtyTwoBits, indices.Length, BufferUsage.None);
		indexBuffer.SetData(indices);

		vertexCount += vertices.Length;
		indexCount += indices.Length;
		
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