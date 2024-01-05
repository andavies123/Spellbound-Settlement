using Andavies.MonoGame.Network.Extensions;
using Andavies.MonoGame.Utilities;
using Andavies.SpellboundSettlement.GameWorld;
using LiteNetLib.Utils;

namespace Andavies.SpellboundSettlement.NetworkMessages.Messages.World;

/// <summary>
/// A packet from the server to client to supply world chunk information
/// </summary>
public class WorldChunkResponsePacket : INetSerializable
{
	/// <summary>
	/// Chunk data at the requested position
	/// </summary>
	public Chunk? Chunk { get; set; }
	
	public void Serialize(NetDataWriter writer)
	{
		if (Chunk == null)
			return;
	
		writer.Put(Chunk.ChunkPosition);
		writer.Put(Chunk.TileCount);

		for (int x = 0; x < Chunk.WorldTiles.GetLength(0); x++)
		{
			for (int y = 0; y < Chunk.WorldTiles.GetLength(1); y++)
			{
				for (int z = 0; z < Chunk.WorldTiles.GetLength(2); z++)
				{
					WorldTile worldTile = Chunk.WorldTiles[x, y, z];
					writer.Put(worldTile.TileId);
					writer.Put(worldTile.TilePosition);
					writer.Put((int)worldTile.Rotation);
				}
			}
		}
	}

	public void Deserialize(NetDataReader reader)
	{
		Vector2Int chunkPosition = reader.GetVector2Int();
		Vector3Int tileCount = reader.GetVector3Int();
		WorldTile[,,] worldTiles = new WorldTile[tileCount.X, tileCount.Y, tileCount.Z];
		
		for (int x = 0; x < tileCount.X; x++)
		{
			for (int y = 0; y < tileCount.Y; y++)
			{
				for (int z = 0; z < tileCount.Z; z++)
				{
					int tileId = reader.GetInt();
					Vector3Int tilePosition = reader.GetVector3Int();
					Rotation rotation = (Rotation)reader.GetInt();
					worldTiles[x, y, z] = new WorldTile(tileId, chunkPosition, tilePosition)
					{
						Rotation = rotation
					};
				}
			}
		}
		
		Chunk = new Chunk(chunkPosition, worldTiles);
	}
}