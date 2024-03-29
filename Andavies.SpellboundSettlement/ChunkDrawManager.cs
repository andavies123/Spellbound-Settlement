﻿using System;
using Andavies.MonoGame.Meshes;
using Andavies.SpellboundSettlement.CameraObjects;
using Andavies.SpellboundSettlement.GameWorld;
using Andavies.SpellboundSettlement.GameWorld.Tiles;
using Andavies.SpellboundSettlement.Globals;
using Andavies.SpellboundSettlement.Meshes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.SpellboundSettlement;

public class ChunkDrawManager : IChunkDrawManager
{
	private readonly IModelDrawManager _modelDrawManager;
	private readonly Camera _camera;
	
	public ChunkDrawManager(IModelDrawManager modelDrawManager, Camera camera)
	{
		_modelDrawManager = modelDrawManager ?? throw new ArgumentNullException(nameof(modelDrawManager));
		_camera = camera ?? throw new ArgumentNullException(nameof(camera));
	}

	public void DrawChunk(ChunkMesh chunkMesh)
	{
		// 1. Draw Terrain
		DrawTerrain(Global.GraphicsDeviceManager.GraphicsDevice, chunkMesh.TerrainMesh);
		
		// 2. Draw Models
		foreach ((ModelTile modelTile, WorldTile worldTile) in chunkMesh.TileModels.Values)
		{
			DrawWorldTile(modelTile, worldTile);
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
	
	private void DrawWorldTile(ModelTile modelTile, WorldTile worldTile)
	{
		const int tileCount = 10;
		Vector3 position = new(
			worldTile.ParentChunkPosition.X * tileCount + worldTile.TilePosition.X,
			0f * tileCount + worldTile.TilePosition.Y, // Currently the world is only 1 chunk high
			worldTile.ParentChunkPosition.Y * tileCount + worldTile.TilePosition.Z);
		
		_modelDrawManager.DrawModel(modelTile.Model, modelTile.ModelDetails, position, worldTile.Scale, worldTile.Rotation);
	}
}