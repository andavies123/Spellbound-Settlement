﻿using System;
using Andavies.MonoGame.Meshes;
using Andavies.SpellboundSettlement.CameraObjects;
using Andavies.SpellboundSettlement.GameWorld;
using Andavies.SpellboundSettlement.Globals;
using Andavies.SpellboundSettlement.Meshes;
using Andavies.SpellboundSettlement.Repositories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.SpellboundSettlement;

public class ChunkDrawManager : IChunkDrawManager
{
	private readonly IModelRepository _modelRepository;
	private readonly Camera _camera;
	
	public ChunkDrawManager(IModelRepository modelRepository, Camera camera)
	{
		_modelRepository = modelRepository ?? throw new ArgumentNullException(nameof(modelRepository));
		_camera = camera ?? throw new ArgumentNullException(nameof(camera));
	}

	public void DrawChunk(ChunkMesh chunkMesh)
	{
		// 1. Draw Terrain
		DrawTerrain(Global.GraphicsDeviceManager.GraphicsDevice, chunkMesh.TerrainMesh);
		
		// 2. Draw Models
		foreach ((ModelTileDetails modelTileDetails, WorldTile worldTile) in chunkMesh.TileModels.Values)
		{
			DrawWorldTile(modelTileDetails, worldTile);
		}
	}
	
	private void DrawTerrain(GraphicsDevice graphicsDevice, IMesh mesh)
	{
		if (mesh.Vertices.Length == 0 || mesh.Indices.Length == 0)
			return;
		
		VertexPositionColor[] vertices = mesh.Vertices;
		int[] indices = mesh.Indices;
		
		VertexBuffer vertexBuffer = new(graphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.None);
		vertexBuffer.SetData(vertices);

		IndexBuffer indexBuffer = new(graphicsDevice, IndexElementSize.ThirtyTwoBits, indices.Length, BufferUsage.None);
		indexBuffer.SetData(indices);
		
		graphicsDevice.SetVertexBuffer(vertexBuffer);
		graphicsDevice.Indices = indexBuffer;
		
		GameManager.Effect.Parameters["WorldMatrix"].SetValue(_camera.WorldMatrix);
		GameManager.Effect.Parameters["ViewMatrix"].SetValue(_camera.ViewMatrix);
		GameManager.Effect.Parameters["ProjectionMatrix"].SetValue(_camera.ProjectionMatrix);
		GameManager.Effect.CurrentTechnique.Passes[0].Apply();
		
		graphicsDevice.DrawIndexedPrimitives(
			PrimitiveType.TriangleList,
			0,
			0,
			indexBuffer.IndexCount / 3);
	}
	
	private void DrawWorldTile(ModelTileDetails modelTileDetails, WorldTile worldTile)
	{
		const int tileCount = 10;
		Vector3 position = new(
			worldTile.ParentChunkPosition.X * tileCount + worldTile.TilePosition.X, 
			0f * tileCount + worldTile.TilePosition.Y, // Currently the world is only 1 chunk high
			worldTile.ParentChunkPosition.Y * tileCount + worldTile.TilePosition.Z);

		if (!_modelRepository.TryGetModel(modelTileDetails.ContentModelPath, out Model model) || model == null)
		{
			return;
		}
		
		foreach (ModelMesh modelMesh in model.Meshes)
		{
			foreach (var effect1 in modelMesh.Effects)
			{
				BasicEffect effect = (BasicEffect) effect1;
				effect.EnableDefaultLighting();
				
				effect.View = _camera.ViewMatrix;
				effect.Projection = _camera.ProjectionMatrix;
		
				Matrix rotationMatrix = Matrix.CreateRotationY(RotationToRadians(worldTile.Rotation));
				Matrix translationMatrix = Matrix.CreateTranslation(modelTileDetails.PostScaleOffset + position);
				Matrix scaleMatrix = Matrix.CreateScale(modelTileDetails.ModelScale * worldTile.Scale);
				
				effect.World = scaleMatrix * rotationMatrix * translationMatrix; // Translation needs to be last
			}
			
			modelMesh.Draw();
		}
	}

	private static float RotationToRadians(Rotation rotation)
	{
		return rotation switch
		{
			Rotation.Zero => 0f,
			Rotation.Ninety => MathHelper.PiOver2,
			Rotation.OneHundredEighty => MathHelper.Pi,
			Rotation.TwoHundredSeventy => MathHelper.PiOver2 * 3,
			_ => throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null)
		};
	}
}