using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.MonoGame.Meshes;

public class PlaneMesh : IMesh
{
	public PlaneMesh(Vector3[] vertices, Color color)
	{
		Color = color;
		
		if (vertices.Length != MeshConstants.VerticesPerFace)
			throw new ArgumentOutOfRangeException(
				$"{nameof(vertices)}", 
				$"Should have a length of {MeshConstants.VerticesPerFace}");

		for (int vertexIndex = 0; vertexIndex < vertices.Length; vertexIndex++)
			Vertices[vertexIndex] = new VertexPositionColor(vertices[vertexIndex], Color);
	}
	
	public Color Color { get; set; }
	
	// IMesh Implementation
	
	public bool IsVisible { get; set; } = true;
	public VertexPositionColor[] Vertices { get; } = new VertexPositionColor[MeshConstants.VerticesPerFace];
	public int[] Indices => MeshConstants.FaceIndices;
	
	public void RecalculateMesh()
	{
		throw new NotImplementedException();
	}
}