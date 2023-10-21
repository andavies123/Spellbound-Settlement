namespace Andavies.MonoGame.Meshes;

public static class MeshConstants
{
	// Vertex Counts
	public const int VerticesPerFace = 4;
	
	// Triangle Vertex Indices per face - 2 triangles
	public static readonly int[] FaceIndices = { 0, 2, 1, 0, 3, 2 };
}