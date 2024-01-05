using System;
using System.Collections.Generic;
using System.Linq;
using Andavies.MonoGame.Meshes;
using Andavies.MonoGame.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.SpellboundSettlement.Meshes;

public class TerrainMesh : IMesh
{
	private readonly CubeMesh[,,] _cubeMeshes;

	public TerrainMesh(Vector3Int terrainSize)
	{
		_cubeMeshes = new CubeMesh[terrainSize.X, terrainSize.Y, terrainSize.Z];
	}

	public void SetCubeMesh(Vector3Int position, CubeMesh cubeMesh)
	{
		_cubeMeshes[position.X, position.Y, position.Z] = cubeMesh;
	}

	public void SetTileColor(Vector3Int position, Color color)
	{
		_cubeMeshes[position.X, position.Y, position.Z].Color = color;
		RecalculateMesh();
	}

	public CubeMesh GetCubeMesh(Vector3Int position)
	{
		return _cubeMeshes[position.X, position.Y, position.Z];
	}

	public VertexPositionColor[] Vertices { get; private set; } = Array.Empty<VertexPositionColor>();
	public int[] Indices { get; private set; } = Array.Empty<int>();
	public bool IsVisible { get; set; }
	
	public void RecalculateMesh()
	{
		List<VertexPositionColor> vertices = new();
		List<int> indices = new();
		int triangleOffset = 0;
		
		foreach (CubeMesh cubeMesh in _cubeMeshes)
		{
			if (cubeMesh == null)
				continue;
			
			if (!cubeMesh.IsVisible)
				continue;
			
			vertices.AddRange(cubeMesh.Vertices);
			indices.AddRange(cubeMesh.Indices.Select(index => index + triangleOffset));
			triangleOffset += cubeMesh.Vertices.Length;
		}

		Vertices = vertices.ToArray();
		Indices = indices.ToArray();
	}
}