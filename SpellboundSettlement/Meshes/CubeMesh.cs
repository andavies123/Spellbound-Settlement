using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static SpellboundSettlement.Meshes.WorldMeshConstants;

namespace SpellboundSettlement.Meshes;

public class CubeMesh : IMesh
{
	private readonly CubeFaceMesh[] _faceMeshes;
	
	public CubeMesh(Vector3 vertexOffset, Color color)
	{
		_faceMeshes = new []
		{
			new CubeFaceMesh(GetOffsetVertices(XPosVertices, vertexOffset), color),
			new CubeFaceMesh(GetOffsetVertices(XNegVertices, vertexOffset), color),
			new CubeFaceMesh(GetOffsetVertices(YPosVertices, vertexOffset), color),
			new CubeFaceMesh(GetOffsetVertices(YNegVertices, vertexOffset), color),
			new CubeFaceMesh(GetOffsetVertices(ZPosVertices, vertexOffset), color),
			new CubeFaceMesh(GetOffsetVertices(ZNegVertices, vertexOffset), color)
		};
		RecalculateMesh();
	}

	public VertexPositionColor[] Vertices { get; private set; }
	public int[] Indices { get; private set; }

	private void RecalculateMesh()
	{
		List<VertexPositionColor> vertices = new();
		List<int> indices = new();
		int triangleOffset = 0;
		
		foreach (CubeFaceMesh faceMesh in _faceMeshes)
		{
			if (!faceMesh.IsVisible)
				continue;
			
			vertices.AddRange(faceMesh.VertexData);
			indices.AddRange(GetOffsetIndices(triangleOffset));
			triangleOffset += VerticesPerFace;
		}

		Vertices = vertices.ToArray();
		Indices = indices.ToArray();
	}
}

public class CubeFaceMesh
{
	public CubeFaceMesh(Vector3[] vertices, Color color)
	{
		Color = color;

		if (vertices.Length != VerticesPerFace)
			throw new ArgumentOutOfRangeException($"{nameof(CubeFaceMesh)} Constructor", $"{nameof(vertices)} should have a length of {VerticesPerFace}");

		for (int vertexIndex = 0; vertexIndex < vertices.Length; vertexIndex++)
			VertexData[vertexIndex] = new VertexPositionColor(vertices[vertexIndex], Color);
	}

	public bool IsVisible { get; set; } = true;
	public Color Color { get; set; }
	public VertexPositionColor[] VertexData { get; set; } = new VertexPositionColor[VerticesPerFace];
}