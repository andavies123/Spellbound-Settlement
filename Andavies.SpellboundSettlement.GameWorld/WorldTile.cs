using Andavies.MonoGame.Utilities;

namespace Andavies.SpellboundSettlement.GameWorld;

public class WorldTile
{
	public WorldTile(int tileId, Vector2Int parentChunkPosition, Vector3Int tilePosition)
	{
		TileId = tileId;
		ParentChunkPosition = parentChunkPosition;
		TilePosition = tilePosition;
	}
	
	public int TileId { get; set; } = 0;
	public Vector2Int ParentChunkPosition { get; set; }
	public Vector3Int TilePosition { get; }
	
	public Rotation Rotation { get; set; } = Rotation.Zero;
}