using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace SpellboundSettlement.Meshes;

public static class ChunkMeshConstants
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
	public static readonly Vector3[] ZPosVertices = { Vertex111, Vertex011, Vertex001, Vertex101 }; // South
	public static readonly Vector3[] ZNegVertices = { Vertex010, Vertex110, Vertex100, Vertex000 }; // North
	
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