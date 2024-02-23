using Andavies.MonoGame.Utilities;

namespace Andavies.SpellboundSettlement.GameWorld;

public class Chunk
{
	public event Action<Chunk>? Updated;

	public WorldTile this[int x, int y, int z] => ChunkData.WorldTiles[x, y, z];
	public WorldTile this[Vector3Int index] => this[index.X, index.Y, index.Z];

	public ChunkData ChunkData { get; } = new();

	public void Update()
	{
		if (!ChunkData.IsChanged) 
			return;
		
		ChunkData.AcceptChanges();
		Updated?.Invoke(this);
	}
}