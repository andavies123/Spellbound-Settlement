using System;
using Andavies.MonoGame.Meshes;
using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.CameraObjects;
using SpellboundSettlement.Inputs;
using SpellboundSettlement.Meshes;
using SpellboundSettlement.UIStates;
using SpellboundSettlement.WorldObjects;

namespace SpellboundSettlement.GameStates;

public class GameplayGameState : GameState
{
	private readonly Camera _camera;
	private readonly World _world = new((0, 0), 5);

	private readonly GameplayUIState _gameplayGameplayUIState;
	
	private WorldMesh _worldMesh;
    
	public GameplayGameState(GameplayUIState gameplayUIState, GameplayInputManager inputManager, Camera camera)
	{
		_gameplayGameplayUIState = gameplayUIState;
		InputState = inputManager;
		_camera = camera;
		
		UIStates.Add(gameplayUIState);
	}
	
	public event Action PauseGame;

	public override GameplayInputManager InputState { get; }

	public override void Init()
	{
		base.Init();
		
		_worldMesh = new WorldMesh(_world);
	}

	public override void Start()
	{
		base.Start();
		
		UIStateMachine.ChangeUIState(_gameplayGameplayUIState);
		
		_gameplayGameplayUIState.PauseButton.MousePressed += RaisePauseGame;
		InputState.PauseGame.OnKeyUp += RaisePauseGame;
	}

	public override void Draw3D(GraphicsDevice graphicsDevice)
	{
		base.Draw3D(graphicsDevice);
		
		// Draw World
		foreach (ChunkMesh chunkMesh in _worldMesh.ChunkMeshes.Values)
			DrawMesh(graphicsDevice, chunkMesh);
	}
	
	public override void End()
	{
		base.End();
		
		_gameplayGameplayUIState.PauseButton.MousePressed -= RaisePauseGame;
		InputState.PauseGame.OnKeyUp -= RaisePauseGame;
	}

	private void RaisePauseGame()
	{
		PauseGame?.Invoke();
	}
	
	private void DrawMesh(GraphicsDevice graphicsDevice, IMesh mesh)
	{
		VertexPositionColor[] vertices = mesh.Vertices;
		int[] indices = mesh.Indices;
		
		VertexBuffer vertexBuffer = new(graphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.None);
		vertexBuffer.SetData(vertices);

		IndexBuffer indexBuffer = new(graphicsDevice, IndexElementSize.ThirtyTwoBits, indices.Length, BufferUsage.None);
		indexBuffer.SetData(indices);
		
		graphicsDevice.SetVertexBuffer(vertexBuffer);
		graphicsDevice.Indices = indexBuffer;
		
		GameManager.Effect.Parameters["WorldViewProjection"].SetValue(_camera.WorldViewProjection);
		GameManager.Effect.CurrentTechnique.Passes[0].Apply();
		
		graphicsDevice.DrawIndexedPrimitives(
			PrimitiveType.TriangleList,
			0,
			0,
			indexBuffer.IndexCount / 3);
	}
}