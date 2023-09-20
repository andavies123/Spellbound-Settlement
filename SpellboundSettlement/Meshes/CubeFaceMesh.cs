using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpellboundSettlement.Meshes;

public class CubeFaceMesh
{
	public CubeFaceMesh(Vector3[] vertices, Color color)
	{
		Color = color;

		if (vertices.Length != WorldMeshConstants.VerticesPerFace)
			throw new ArgumentOutOfRangeException($"{nameof(CubeFaceMesh)} Constructor", $"{nameof(vertices)} should have a length of {WorldMeshConstants.VerticesPerFace}");

		for (int vertexIndex = 0; vertexIndex < vertices.Length; vertexIndex++)
			VertexData[vertexIndex] = new VertexPositionColor(vertices[vertexIndex], Color);
	}

	public bool IsVisible { get; set; } = true;
	public Color Color { get; set; }
	public VertexPositionColor[] VertexData { get; set; } = new VertexPositionColor[WorldMeshConstants.VerticesPerFace];
}