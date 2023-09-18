using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static SpellboundSettlement.Meshes.CubeMeshConstants;

namespace SpellboundSettlement.Meshes;

public class CubeMesh
{
	private readonly CubeFaceMesh[] _faceMeshes;

	public readonly List<VertexPositionColor> Vertices = new();
	public readonly List<int> Indices = new();
	
	public CubeMesh(Vector3 vertexOffset)
	{
		_faceMeshes = new []
		{
			new CubeFaceMesh(GetOffsetVertices(XPosVertices, vertexOffset), XPosFaceColor),
			new CubeFaceMesh(GetOffsetVertices(XNegVertices, vertexOffset), XNegFaceColor),
			new CubeFaceMesh(GetOffsetVertices(YPosVertices, vertexOffset), YPosFaceColor),
			new CubeFaceMesh(GetOffsetVertices(YNegVertices, vertexOffset), YNegFaceColor),
			new CubeFaceMesh(GetOffsetVertices(ZPosVertices, vertexOffset), ZPosFaceColor),
			new CubeFaceMesh(GetOffsetVertices(ZNegVertices, vertexOffset), ZNegFaceColor)
		};
		RecalculateMesh();
	}

	private void RecalculateMesh()
	{
		Vertices.Clear();
		Indices.Clear();
		int triangleOffset = 0;
		
		foreach (CubeFaceMesh faceMesh in _faceMeshes)
		{
			if (!faceMesh.IsVisible)
				continue;
			
			Vertices.AddRange(faceMesh.VertexData);
			Indices.AddRange(GetOffsetIndices(triangleOffset));
			triangleOffset += VerticesPerFace;
		}
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

public static class CubeMeshConstants
{
	// Count Constants
	public const int VerticesPerFace = 4;
	
	// Face Array Indices
	public const int XPosIndex = 0;
	public const int XNegIndex = 1;
	public const int YPosIndex = 2;
	public const int YNegIndex = 3;
	public const int ZPosIndex = 4;
	public const int ZNegIndex = 5;
	
	// Vertices - Vertex(XYZ)
	public static readonly Vector3 Vertex000 = new(0, 0, 0);
	public static readonly Vector3 Vertex001 = new(0, 0, 1);
	public static readonly Vector3 Vertex010 = new(0, 1, 0);
	public static readonly Vector3 Vertex011 = new(0, 1, 1);
	public static readonly Vector3 Vertex100 = new(1, 0, 0);
	public static readonly Vector3 Vertex101 = new(1, 0, 1);
	public static readonly Vector3 Vertex110 = new(1, 1, 0);
	public static readonly Vector3 Vertex111 = new(1, 1, 1);
	
	// Vertex groups
	public static readonly Vector3[] XPosVertices = { Vertex110, Vertex111, Vertex101, Vertex100 }; // East
	public static readonly Vector3[] XNegVertices = { Vertex011, Vertex010, Vertex000, Vertex001 }; // West
	public static readonly Vector3[] YPosVertices = { Vertex011, Vertex111, Vertex110, Vertex010 }; // Up
	public static readonly Vector3[] YNegVertices = { Vertex000, Vertex100, Vertex101, Vertex001 }; // Down
	public static readonly Vector3[] ZPosVertices = { Vertex111, Vertex011, Vertex001, Vertex101 }; // North?
	public static readonly Vector3[] ZNegVertices = { Vertex010, Vertex110, Vertex100, Vertex000 }; // South?
	
	// Triangle Vertex Indices per face - 2 triangles
	public static readonly int[] FaceIndices = { 0, 2, 1, 0, 3, 2 };

	// Face colors
	public static readonly Color XPosFaceColor = Color.Aqua;
	public static readonly Color XNegFaceColor = Color.MidnightBlue;
	public static readonly Color YPosFaceColor = Color.White;
	public static readonly Color YNegFaceColor = Color.Black;
	public static readonly Color ZPosFaceColor = Color.Red;
	public static readonly Color ZNegFaceColor = Color.Orange;

	public static Vector3[] GetOffsetVertices(IEnumerable<Vector3> vertices, Vector3 offset) => vertices.Select(vertex => vertex + offset).ToArray();
	public static int[] GetOffsetIndices(int offset) => FaceIndices.Select(index => index + offset).ToArray();
}