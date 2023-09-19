using Microsoft.Xna.Framework.Graphics;

namespace SpellboundSettlement.Meshes;

/// <summary>
/// Interface to describe everything necessary to display a mesh
/// </summary>
public interface IMesh
{
	/// <summary>
	/// True if the mesh is visible and should be drawn
	/// False if the mesh is not visible and should not be drawn
	/// </summary>
	bool IsVisible { get; set; }
	
	/// <summary>
	/// Array of vertices that describe the mesh
	/// </summary>
	VertexPositionColor[] Vertices { get; }
	
	/// <summary>
	/// Array of vertex indices that describe the mesh.
	/// Each index should point to a vertex in the Vertices array.
	/// Array should contain a multiple of 3 indices (3 indices per triangle)
	/// </summary>
	int[] Indices { get; }
}