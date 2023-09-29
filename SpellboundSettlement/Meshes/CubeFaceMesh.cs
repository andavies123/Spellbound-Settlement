using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpellboundSettlement.Meshes;

public class CubeFaceMesh : IMesh
{
	public CubeFaceMesh(Vector3[] vertices, Color color)
	{
		Color = color;

		if (vertices.Length != WorldMeshConstants.VerticesPerFace)
			throw new ArgumentOutOfRangeException($"{nameof(CubeFaceMesh)} Constructor", $"{nameof(vertices)} should have a length of {WorldMeshConstants.VerticesPerFace}");

		for (int vertexIndex = 0; vertexIndex < vertices.Length; vertexIndex++)
			Vertices[vertexIndex] = new VertexPositionColor(vertices[vertexIndex], Color);
	}


	public Color Color { get; set; }
	
	#region IMesh Implementation
	
	public bool IsVisible { get; set; } = true;
	public int[] Indices => WorldMeshConstants.FaceIndices;
	public VertexPositionColor[] Vertices { get; } = new VertexPositionColor[WorldMeshConstants.VerticesPerFace];
	
	// Unused at the current moment since faces don't move
	public void RecalculateMesh() { }
	
	#endregion
}