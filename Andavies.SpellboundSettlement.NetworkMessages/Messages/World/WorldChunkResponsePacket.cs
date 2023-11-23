using Andavies.SpellboundSettlement.World;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;

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
	
		// Chunk Position
		writer.Put(Chunk.ChunkPosition.X);
		writer.Put(Chunk.ChunkPosition.Y);
		
		// World Offset
		writer.Put(Chunk.WorldOffset.X);
		writer.Put(Chunk.WorldOffset.Y);
		
		// Tile Count
		writer.Put(Chunk.TileCount.x);
		writer.Put(Chunk.TileCount.y);
		writer.Put(Chunk.TileCount.z);
		
		// Tiles
		for (int x = 0; x < Chunk.TileCount.x; x++)
		{
			for (int y = 0; y < Chunk.TileCount.y; y++)
			{
				for (int z = 0; z < Chunk.TileCount.z; z++)
				{
					writer.Put(Chunk.Tiles[x, y, z]);
				}
			}
		}
	}

	public void Deserialize(NetDataReader reader)
	{
		// Chunk Position
		float chunkPositionX = reader.GetFloat();
		float chunkPositionY = reader.GetFloat();
		Vector2 chunkPosition = new(chunkPositionX, chunkPositionY);
		
		// World Offset
		float worldOffsetX = reader.GetFloat();
		float worldOffsetY = reader.GetFloat();
		Vector2 worldOffset = new(worldOffsetX, worldOffsetY);
		
		// Tile Count
		int tileCountX = reader.GetInt();
		int tileCountY = reader.GetInt();
		int tileCountZ = reader.GetInt();
		
		Chunk = new Chunk(chunkPosition, worldOffset, (tileCountX, tileCountY, tileCountZ));
		
		// Tiles
		for (int x = 0; x < tileCountX; x++)
		{
			for (int y = 0; y < tileCountY; y++)
			{
				for (int z = 0; z < tileCountZ; z++)
				{
					Chunk.Tiles[x, y, z] = reader.GetInt();
				}
			}
		}

	}

	public override string ToString()
	{
		return $"{nameof(Chunk.ChunkPosition)}: {Chunk.ChunkPosition}\n" +
		       $"{nameof(Chunk.TileCount)}: {Chunk.TileCount}";
	}
}