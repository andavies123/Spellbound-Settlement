using System.Collections.Concurrent;
using System.Collections.Generic;
using Andavies.MonoGame.Utilities;
using Andavies.SpellboundSettlement.GameWorld;
using Andavies.SpellboundSettlement.GameWorld.Tiles;

namespace Andavies.SpellboundSettlement.Meshes;

public class ChunkMesh
{
	private readonly ConcurrentDictionary<Vector3Int, (ModelTile, WorldTile)> _tileModels = new ();

	public ChunkMesh(Chunk chunk)
	{
		Chunk = chunk;
		TerrainMesh = new TerrainMesh(chunk.TileCount);
		ChunkMeshCollider = new ChunkMeshCollider(new Vector3Int(chunk.ChunkPosition.X * 10, 0, chunk.ChunkPosition.Y * 10), chunk.TileCount);
	}

	public Chunk Chunk { get; }
	public TerrainMesh TerrainMesh { get; }
	public ChunkMeshCollider ChunkMeshCollider { get; }
	
	public IReadOnlyDictionary<Vector3Int, (ModelTile, WorldTile)> TileModels => _tileModels;

	public void SetTileModel(Vector3Int position, ModelTile modelTile, WorldTile worldTile)
	{
		_tileModels[position] = (modelTile, worldTile);
	}

	public void SetTileMesh(Vector3Int position, CubeMesh cubeMesh)
	{
		TerrainMesh.SetCubeMesh(position, cubeMesh);
	}
}