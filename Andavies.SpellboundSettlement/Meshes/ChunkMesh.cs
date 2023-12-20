using System.Collections.Generic;
using System.Linq;
using Andavies.MonoGame.Meshes;
using Andavies.SpellboundSettlement.GameWorld;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.SpellboundSettlement.Meshes;

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

		Collider = new BoundingBox(_chunkOffset, _chunkOffset + new Vector3(_chunk.TileCount.x, _chunk.TileCount.y, _chunk.TileCount.z));
		TileColliders = new BoundingBox[_chunk.TileCount.x, _chunk.TileCount.y, _chunk.TileCount.z];
		
		InitializeCubeMeshes();
		RecalculateMesh();
	}

	public bool IsVisible { get; set; }
	public VertexPositionColor[] Vertices { get; private set; }
	public int[] Indices { get; private set; }
	
	// Colliders
	public BoundingBox Collider { get; set; }
	public BoundingBox[,,] TileColliders { get; set; }

	public CubeMesh[,,] TileMeshes => _cubeMeshes;
	public Vector2 ChunkPosition => _chunk.ChunkPosition;

	public void SetTileColor(int x, int y, int z, Color color)
	{
		_cubeMeshes[x, y, z].Color = color;
		RecalculateMesh();
	}

	private void InitializeCubeMeshes()
	{
		for (int x = 0; x < _cubeMeshes.GetLength(0); x++)
		{
			for (int y = 0; y < _cubeMeshes.GetLength(1); y++)
			{
				for (int z = 0; z < _cubeMeshes.GetLength(2); z++)
				{
					Vector3 tilePosition = _chunkOffset + new Vector3(x, y, z);
					_cubeMeshes[x, y, z] = new CubeMesh(tilePosition, WorldMeshConstants.HeightColors[y]);
					TileColliders[x, y, z] = new BoundingBox(tilePosition, tilePosition + WorldMeshConstants.Vertex111);
					
					// Check to see if the whole cube is visible
					if (_chunk.Tiles[x, y, z] == 0)
					{
						_cubeMeshes[x, y, z].IsVisible = false;
						continue;
					}
					
					// Check each cubes individual faces for visibility
					if (x + 1 <= _chunk.Tiles.GetLength((int)WorldDimension.X) - 1 && _chunk.Tiles[x + 1, y, z] != 0)
						_cubeMeshes[x, y, z].SetFaceVisibility(WorldDirection.XPositive, false);
					if (x - 1 >= 0 && _chunk.Tiles[x - 1, y, z] != 0)
						_cubeMeshes[x, y, z].SetFaceVisibility(WorldDirection.XNegative, false);
					if (y + 1 <= _chunk.Tiles.GetLength((int)WorldDimension.Y) - 1 && _chunk.Tiles[x, y + 1, z] != 0) 
						_cubeMeshes[x, y, z].SetFaceVisibility(WorldDirection.YPositive, false);
					if (y - 1 >= 0 && _chunk.Tiles[x, y - 1, z] != 0) 
						_cubeMeshes[x, y, z].SetFaceVisibility(WorldDirection.YNegative, false);
					if (z + 1 <= _chunk.Tiles.GetLength((int)WorldDimension.Z) - 1 && _chunk.Tiles[x, y, z + 1] != 0) 
						_cubeMeshes[x, y, z].SetFaceVisibility(WorldDirection.ZPositive, false);
					if (z - 1 >= 0 && _chunk.Tiles[x, y, z - 1] != 0) 
						_cubeMeshes[x, y, z].SetFaceVisibility(WorldDirection.ZNegative, false);
					
					// Recalculate mesh
					_cubeMeshes[x, y, z].RecalculateMesh();
				}
			}
		}
	}

	public void RecalculateMesh()
	{
		List<VertexPositionColor> vertices = new();
		List<int> indices = new();
		int triangleOffset = 0;
		
		foreach (CubeMesh cubeMesh in _cubeMeshes)
		{
			if (cubeMesh == null)
				continue;
			
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