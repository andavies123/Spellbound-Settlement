using Andavies.MonoGame.Utilities;
using FluentAssertions;

namespace Andavies.SpellboundSettlement.GameWorld.Test;

[TestClass]
public class WorldHelperTests
{
	#region WorldPositionToChunkPosition Tests

	public static IEnumerable<object[]> WorldPositionToChunkPositionData => new[]
	{
		new object[] {new Vector3Int(0, 0, 0), new Vector2Int(0, 0)},
		new object[] {new Vector3Int(-1, 0, -1), new Vector2Int(-1, -1)},
		new object[] {new Vector3Int(-1, 0, 0), new Vector2Int(-1, 0)},
		new object[] {new Vector3Int(0, 0, -1), new Vector2Int(0, -1)},
		new object[] {new Vector3Int(WorldHelper.ChunkSize.X, 0, WorldHelper.ChunkSize.Z), new Vector2Int(1, 1)}
	};

	[TestMethod]
	[DynamicData(nameof(WorldPositionToChunkPositionData))]
	public void WorldPositionToChunkPosition_GivesCorrectChunkPosition(Vector3Int worldPosition, Vector2Int expectedChunkPosition)
	{
		// Arrange / Act
		Vector2Int chunkPosition = WorldHelper.WorldPositionToChunkPosition(worldPosition);

		// Assert
		chunkPosition.Should().Be(expectedChunkPosition);
	}

	#endregion

	#region WorldPositionToTilePosition Tests

	public static IEnumerable<object[]> WorldPositionToTilePositionData => new[]
	{
		new object[] {new Vector3Int(0, 0, 0), new Vector3Int(0, 0, 0)},
		new object[] {new Vector3Int(5, 5, 5), new Vector3Int(5, 5, 5)},
		new object[] {new Vector3Int(-1, 0, -1), new Vector3Int(10, 0, 10)},
		new object[] {new Vector3Int(10, 10, 10), new Vector3Int(0, 0, 0)},
		new object[] {new Vector3Int(12, 5, 7), new Vector3Int(2, 5, 7)},
	};

	[TestMethod]
	[DynamicData(nameof(WorldPositionToTilePositionData))]
	public void WorldPositionToTilePosition_GivesCorrectTilePosition(Vector3Int worldPosition, Vector3Int expectedTilePosition)
	{
		// Arrange / Act
		Vector3Int tilePosition = WorldHelper.WorldPositionToTilePosition(worldPosition);
		
		// Assert
		tilePosition.Should().Be(expectedTilePosition);
	}

	#endregion
}