using Andavies.MonoGame.Utilities;
using Microsoft.Xna.Framework;

namespace Andavies.SpellboundSettlement.Meshes;

public class ChunkMeshCollider
{
	public ChunkMeshCollider(Vector3Int chunkPosition, Vector3Int tileCount)
	{
		ChunkCollider = new BoxCollider((Vector3)chunkPosition, (Vector3)tileCount);
		TileColliders = new BoxCollider[tileCount.X, tileCount.Y, tileCount.Z];

		for (int x = 0; x < TileColliders.GetLength(0); x++)
		{
			for (int y = 0; y < TileColliders.GetLength(1); y++)
			{
				for (int z = 0; z < TileColliders.GetLength(2); z++)
				{
					TileColliders[x, y, z] = new BoxCollider(chunkPosition + new Vector3(x, y, z), Vector3.One);
				}
			}
		}
	}
	
	public BoxCollider ChunkCollider { get; set; }
	public BoxCollider[,,] TileColliders { get; set; }
}