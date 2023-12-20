using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.MonoGame.Meshes;

public class PlaneMesh : IMesh
{
	private readonly Vector3[] _cornerVertices;
	private Color _color;
	
	public PlaneMesh(Vector3[] cornerVertices, Color color)
	{
		if (cornerVertices.Length != MeshConstants.VerticesPerFace)
			throw new ArgumentOutOfRangeException($"{nameof(cornerVertices)}", $"Should have a length of {MeshConstants.VerticesPerFace}");
		
		_cornerVertices = cornerVertices;
		_color = color;
		
		RecalculateMesh();
	}

	public Color Color
	{
		get => _color;
		set
		{
			_color = value;
			RecalculateMesh();
		}
	}
	
	// IMesh Implementation
	public bool IsVisible { get; set; } = true;
	public VertexPositionColor[] Vertices { get; } = new VertexPositionColor[MeshConstants.VerticesPerFace];
	public int[] Indices => MeshConstants.FaceIndices;

	public void RecalculateMesh()
	{
		for (int vertexIndex = 0; vertexIndex < _cornerVertices.Length; vertexIndex++)
			Vertices[vertexIndex] = new VertexPositionColor(_cornerVertices[vertexIndex], Color);
	}
}