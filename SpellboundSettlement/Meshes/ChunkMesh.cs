using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.WorldObjects;

namespace SpellboundSettlement.Meshes;

public class ChunkMesh : IMesh
{
	private readonly Chunk _chunk;
	private readonly Vector3 _chunkOffset;
	private readonly CubeMesh[,,] _cubeMeshes;
	
	public ChunkMesh(Chunk chunk, Vector3 chunkOffset)
	{
		_chunk = chunk;
		_chunkOffset = chunkOffset;

		_cubeMeshes = new CubeMesh[_chunk.TileCount.x, _chunk.TileCount.y, _chunk.TileCount.z];
		
		InitializeCubeMeshes();
		RecalculateMesh();
	}

	public bool IsVisible { get; set; }
	public VertexPositionColor[] Vertices { get; private set; }
	public int[] Indices { get; private set; }

	private void InitializeCubeMeshes()
	{
		for (int x = 0; x < _cubeMeshes.GetLength(0); x++)
		{
			for (int y = 0; y < _cubeMeshes.GetLength(1); y++)
			{
				for (int z = 0; z < _cubeMeshes.GetLength(2); z++)
				{
					_cubeMeshes[x, y, z] = new CubeMesh(_chunkOffset + new Vector3(x, y, z), WorldMeshConstants.HeightColors[y]);
					if (_chunk.Tiles[x, y, z] == 0)
						_cubeMeshes[x, y, z].IsVisible = false;
				}
			}
		}
	}

	private void RecalculateMesh()
	{
		List<VertexPositionColor> vertices = new();
		List<int> indices = new();
		int triangleOffset = 0;
		
		foreach (CubeMesh cubeMesh in _cubeMeshes)
		{
			if (!cubeMesh.IsVisible)
				continue;
			
			vertices.AddRange(cubeMesh.Vertices);
			indices.AddRange(cubeMesh.Indices.Select(index => index + triangleOffset));
			triangleOffset += cubeMesh.Vertices.Length;
		}

		Vertices = vertices.ToArray();
		Indices = indices.ToArray();
	}
}