using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpellboundSettlement.Meshes;

public class ChunkMesh : IMesh
{
	private readonly int _chunkSizeX;
	private readonly int _chunkSizeY;
	private readonly int _chunkSizeZ;
	private readonly Vector3 _chunkOffset;

	private readonly CubeMesh[,,] _cubeMeshes;
	
	public ChunkMesh(Vector3 chunkSize, Vector3 chunkOffset)
	{
		_chunkSizeX = (int)chunkSize.X;
		_chunkSizeY = (int)chunkSize.Y;
		_chunkSizeZ = (int)chunkSize.Z;
		_chunkOffset = chunkOffset;

		_cubeMeshes = new CubeMesh[_chunkSizeX, _chunkSizeY, _chunkSizeZ];
		
		InitializeCubeMeshes();
		RecalculateMesh();
	}

	public VertexPositionColor[] Vertices { get; private set; }
	public int[] Indices { get; private set; }
	public (int x, int y, int z) ChunkSize => (_chunkSizeX, _chunkSizeY, _chunkSizeZ);

	private void InitializeCubeMeshes()
	{
		for (int x = 0; x < _cubeMeshes.GetLength(0); x++)
		{
			for (int y = 0; y < _cubeMeshes.GetLength(1); y++)
			{
				for (int z = 0; z < _cubeMeshes.GetLength(2); z++)
				{
					_cubeMeshes[x, y, z] = new CubeMesh(_chunkOffset + new Vector3(x, y, z));
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
			vertices.AddRange(cubeMesh.Vertices);
			indices.AddRange(cubeMesh.Indices.Select(index => index + triangleOffset));
			triangleOffset += cubeMesh.Vertices.Length;
		}

		Vertices = vertices.ToArray();
		Indices = indices.ToArray();
	}
}