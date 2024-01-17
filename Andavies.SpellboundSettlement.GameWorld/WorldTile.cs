using Andavies.MonoGame.Utilities;

namespace Andavies.SpellboundSettlement.GameWorld;

public class WorldTile
{
	public WorldTile(string tileId, Vector2Int parentChunkPosition, Vector3Int tilePosition)
	{
		TileId = tileId;
		ParentChunkPosition = parentChunkPosition;
		TilePosition = tilePosition;
	}
	
	public string TileId { get; set; }
	
	public Vector2Int ParentChunkPosition { get; set; }
	public Vector3Int TilePosition { get; }
	
	public Rotation Rotation { get; set; } = Rotation.Zero;
	public float Scale { get; set; } = 1f;
}