using Microsoft.Xna.Framework;

namespace Andavies.SpellboundSettlement.GameWorld;

public interface ITileDetails
{
	/// <summary>
	/// Unique Id for this tile. Should not be shared with any other tile
	/// </summary>
	int TileId { get; }
	
	/// <summary>
	/// The name that will be displayed for this tile
	/// </summary>
	string DisplayName { get; }
	
	/// <summary>
	/// A short description for this tile
	/// </summary>
	string Description { get; }
}

public readonly struct NonVisibleTileDetails : ITileDetails
{
	public NonVisibleTileDetails(int tileId, string displayName, string description)
	{
		TileId = tileId;
		DisplayName = displayName;
		Description = description;
	}
	
	public int TileId { get; }
	public string DisplayName { get; }
	public string Description { get; }
}

public readonly struct TerrainTileDetails : ITileDetails
{
	public TerrainTileDetails(int tileId, string displayName, string description)
	{
		TileId = tileId;
		DisplayName = displayName;
		Description = description;
	}
	
	public int TileId { get; }
	public string DisplayName { get; }
	public string Description { get; }
}

public readonly struct ModelTileDetails : ITileDetails
{
	public ModelTileDetails(int tileId, string displayName, string description, string contentModelPath, float modelScale)
	{
		TileId = tileId;
		DisplayName = displayName;
		Description = description;
		ContentModelPath = contentModelPath;
		ModelScale = modelScale;
	}
	
	public int TileId { get; }
	public string DisplayName { get; }
	public string Description { get; }
	public string ContentModelPath { get; }
	public float ModelScale { get; }
	public Vector3 PostScaleOffset { get; } = new(.5f);
}