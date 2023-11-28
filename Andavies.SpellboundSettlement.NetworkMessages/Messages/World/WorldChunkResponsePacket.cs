using Andavies.MonoGame.NetworkUtilities.Extensions;
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
	
		writer.Put(Chunk.ChunkPosition);
		writer.Put(Chunk.WorldOffset);
		writer.Put(Chunk.TileCount);
		writer.Put(Chunk.Tiles);
	}

	public void Deserialize(NetDataReader reader)
	{
		Vector2 chunkPosition = reader.GetVector2();
		Vector2 worldOffset = reader.GetVector2();
		(int x, int y, int z) tileCount = reader.GetIntTuple3();
		int[,,] tiles = reader.GetInt3DArray(tileCount.x, tileCount.y, tileCount.z);
		
		Chunk = new Chunk(chunkPosition, worldOffset, tiles);
	}
}