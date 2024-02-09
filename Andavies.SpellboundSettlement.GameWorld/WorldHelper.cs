using Andavies.MonoGame.Utilities;

namespace Andavies.SpellboundSettlement.GameWorld;

public static class WorldHelper
{
	/// <summary>
	/// The size of each chunk
	/// </summary>
	public static readonly Vector3Int ChunkSize = new(10); 
	
	/// <summary>
	/// Calculates and returns the position of the chunk
	/// </summary>
	/// <param name="worldPosition">The world position used to get the chunk position</param>
	/// <returns>The chunk position that corresponds with the given world position</returns>
	public static Vector2Int WorldPositionToChunkPosition(Vector3Int worldPosition)
	{
		return new Vector2Int(
			worldPosition.X < 0 ? (worldPosition.X - ChunkSize.Z + 1) / ChunkSize.Z : worldPosition.X / ChunkSize.Z,
			worldPosition.Z < 0 ? (worldPosition.Z - ChunkSize.Z + 1) / ChunkSize.Z : worldPosition.Z / ChunkSize.Z);
	}

	/// <summary>
	/// Calculates and returns the position of a tile inside of a chunk
	/// </summary>
	/// <param name="worldPosition">The world position used to get the tile position</param>
	/// <returns>The tile position inside of a chunk that corresponds with the given world position</returns>
	public static Vector3Int WorldPositionToTilePosition(Vector3Int worldPosition)
	{
		return new Vector3Int(
			worldPosition.X < 0 ? ChunkSize.X + worldPosition.X % ChunkSize.X : worldPosition.X % ChunkSize.X,
			worldPosition.Y < 0 ? ChunkSize.Y + worldPosition.Y % ChunkSize.Y : worldPosition.Y % ChunkSize.Y,
			worldPosition.Z < 0 ? ChunkSize.Z + worldPosition.Z % ChunkSize.Z : worldPosition.Z % ChunkSize.Z);
	}

	/// <summary>
	/// Calculates and returns the world position from a given chunk position and tile position
	/// </summary>
	/// <param name="chunkPosition">The chunk position used to get the world position</param>
	/// <param name="tilePosition">The tile position inside of a chunk used to get the world position</param>
	/// <returns>The world position that corresponds with the given chunk and tile position</returns>
	public static Vector3Int ChunkAndTilePositionToWorldPosition(Vector2Int chunkPosition, Vector3Int tilePosition)
	{
		return new Vector3Int(
			chunkPosition.X * ChunkSize.X + tilePosition.X,
			tilePosition.Y,
			chunkPosition.Y * ChunkSize.Z + tilePosition.Z);
	}
}