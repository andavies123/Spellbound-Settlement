﻿using System.Collections.Generic;
using Andavies.MonoGame.Meshes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Andavies.SpellboundSettlement.Meshes.WorldMeshConstants;

namespace Andavies.SpellboundSettlement.Meshes;

public class CubeMesh : IMesh
{
	private readonly PlaneMesh[] _faceMeshes;

	public CubeMesh(Vector3 vertexOffset, Color color)
	{
		_faceMeshes = new[]
		{
			new PlaneMesh(GetOffsetVertices(XPosVertices, vertexOffset), color),
			new PlaneMesh(GetOffsetVertices(XNegVertices, vertexOffset), color),
			new PlaneMesh(GetOffsetVertices(YPosVertices, vertexOffset), color),
			new PlaneMesh(GetOffsetVertices(YNegVertices, vertexOffset), color),
			new PlaneMesh(GetOffsetVertices(ZPosVertices, vertexOffset), color),
			new PlaneMesh(GetOffsetVertices(ZNegVertices, vertexOffset), color)
		};
		RecalculateMesh();
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