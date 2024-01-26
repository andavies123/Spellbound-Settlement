using Andavies.MonoGame.Network.Extensions;
using Andavies.MonoGame.Utilities;
using LiteNetLib.Utils;

namespace Andavies.SpellboundSettlement.GameWorld;

public class WorldTile : INetSerializable
{
	public string TileId { get; set; } = string.Empty;
	
	public Vector2Int ParentChunkPosition { get; set; }
	public Vector3Int TilePosition { get; set; }

	public float Rotation { get; set; } = 0f;
	public float Scale { get; set; } = 1f;
	
	public void Serialize(NetDataWriter writer)
	{
		writer.Put(TileId);
		
		writer.Put(ParentChunkPosition);
		writer.Put(TilePosition);
		
		writer.Put(Rotation);
		writer.Put(Scale);
	}

	public void Deserialize(NetDataReader reader)
	{
		TileId = reader.GetString();

		ParentChunkPosition = reader.GetVector2Int();
		TilePosition = reader.GetVector3Int();

		Rotation = reader.GetFloat();
		Scale = reader.GetFloat();
	}
}