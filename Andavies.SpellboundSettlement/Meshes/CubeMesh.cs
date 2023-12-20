using System.Collections.Generic;
using Andavies.MonoGame.Meshes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Andavies.SpellboundSettlement.Meshes.WorldMeshConstants;

namespace Andavies.SpellboundSettlement.Meshes;

public class CubeMesh : IMesh
{
	private readonly PlaneMesh[] _faceMeshes;
	private Color _color;

	public CubeMesh(Vector3 vertexOffset, Color color)
	{
		_color = color;
		_faceMeshes = new[]
		{
			new PlaneMesh(GetOffsetVertices(XPosVertices, vertexOffset), Color),
			new PlaneMesh(GetOffsetVertices(XNegVertices, vertexOffset), Color),
			new PlaneMesh(GetOffsetVertices(YPosVertices, vertexOffset), Color),
			new PlaneMesh(GetOffsetVertices(YNegVertices, vertexOffset), Color),
			new PlaneMesh(GetOffsetVertices(ZPosVertices, vertexOffset), Color),
			new PlaneMesh(GetOffsetVertices(ZNegVertices, vertexOffset), Color)
		};
		RecalculateMesh();
	}

	public Color Color
	{
		get => _color;
		set
		{
			_color = value;
			foreach (PlaneMesh faceMesh in _faceMeshes)
			{
				faceMesh.Color = _color;
			}
			RecalculateMesh();
		}
	}

	public void SetFaceVisibility(WorldDirection worldDirection, bool visibility) =>
		_faceMeshes[(int) worldDirection].IsVisible = visibility;
	
	#region IMesh Implementation
	
	public bool IsVisible { get; set; } = true;
	public VertexPositionColor[] Vertices { get; private set; }
	public int[] Indices { get; private set; }

	public void RecalculateMesh()
	{
		List<VertexPositionColor> vertices = new();
		List<int> indices = new();
		int triangleOffset = 0;
		
		foreach (PlaneMesh faceMesh in _faceMeshes)
		{
			if (!faceMesh.IsVisible)
				continue;
			
			vertices.AddRange(faceMesh.Vertices);
			indices.AddRange(GetOffsetIndices(triangleOffset));
			triangleOffset += VerticesPerFace;
		}

		Vertices = vertices.ToArray();
		Indices = indices.ToArray();
	}
	
	#endregion
}