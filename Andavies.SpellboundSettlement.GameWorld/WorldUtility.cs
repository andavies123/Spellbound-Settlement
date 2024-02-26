using Andavies.MonoGame.Utilities;
using Microsoft.Xna.Framework;

namespace Andavies.SpellboundSettlement.GameWorld;

public static class WorldUtility
{
	public static List<(Vector3Int worldPosition, float direction)> GeneratePath(this World world, Vector3Int fromPosition, Vector3Int toPosition)
	{
		List<(Vector3Int, float)> path = new();
		Vector3Int currentPosition = fromPosition;
		float currentRotation = 0;

		while (currentPosition.X != toPosition.X || currentPosition.Z != toPosition.Z)
		{
			// Simple movement for now
			// Moves over to match X then moves over to match Z
			if (currentPosition.X > toPosition.X)
			{
				currentPosition = new Vector3Int(currentPosition.X - 1, currentPosition.Y, currentPosition.Z);
				currentRotation = MathHelper.Pi;
			}
			else if (currentPosition.X < toPosition.X)
			{
				currentPosition = new Vector3Int(currentPosition.X + 1, currentPosition.Y, currentPosition.Z);
				currentRotation = 0;
			}
			else if (currentPosition.Z > toPosition.Z)
			{
				currentPosition = new Vector3Int(currentPosition.X, currentPosition.Y, currentPosition.Z - 1);
				currentRotation = MathHelper.PiOver2;
			}
			else if (currentPosition.Z < toPosition.Z)
			{
				currentPosition = new Vector3Int(currentPosition.X, currentPosition.Y, currentPosition.Z + 1);
				currentRotation = MathHelper.PiOver2 * 3;
			}

			// Get the height
			if (!world.TryGetHeightAtPosition(currentPosition, out int? height) || height == null)
				height = 0;
			
			// Add the height of the current position
			currentPosition = new Vector3Int(currentPosition.X, height.Value + 1, currentPosition.Z);

			path.Add((currentPosition, currentRotation));
		}

		return path;
	}
	
	public static bool TryGetHeightAtPosition(this World world, Vector3Int worldPosition, out int? height)
	{
		height = null;

		Vector2Int chunkPosition = WorldHelper.WorldPositionToChunkPosition(worldPosition);
		Vector3Int tilePosition = WorldHelper.WorldPositionToTilePosition(worldPosition);

		if (!world.TryGetChunk(chunkPosition, out Chunk? chunk) || chunk == null)
			return false;

		height = chunk.GetHeightAtPosition(new Vector2Int(tilePosition.X, tilePosition.Z));
		return true;
	}
}