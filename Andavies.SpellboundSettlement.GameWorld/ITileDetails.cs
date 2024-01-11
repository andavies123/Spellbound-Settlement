using System.ComponentModel;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

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

[Serializable]
public struct NonVisibleTileDetails : ITileDetails
{
	public NonVisibleTileDetails() { }

	// ITileDetails Properties
	
	public int TileId { get; set; } = -1;
	public string DisplayName { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
}

[Serializable]
public struct TerrainTileDetails : ITileDetails
{
	public TerrainTileDetails() { }
    
	// ITileDetails Properties
	
	public int TileId { get; set; } = -1;
	public string DisplayName { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
}

[Serializable]
public struct ModelTileDetails : ITileDetails
{
	[JsonConstructor]
	public ModelTileDetails(Vector3? postScaleOffset)
	{
		PostScaleOffset = postScaleOffset ?? new Vector3(.5f);
	}

	// ITileDetails Properties
	
	public int TileId { get; set; } = -1;
	public string DisplayName { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	
	// ModelTileDetails Properties
	
	/// <summary>
	/// The path of the model related to the Content folder
	/// </summary>
	public string ContentModelPath { get; set; } = string.Empty;
	
	/// <summary>
	/// Used to scale a model down to tile size.
	/// This will be the default scale before applying the display scale
	/// </summary>
	public float ModelScale { get; set; } = 1f;
	
	/// <summary>
	/// The min display scale to be scaled after the ModelScale is added
	/// This would be used for tiles that can be given a scale range when added to the world
	/// If no scale range is necessary, make this the same as the max display scale
	/// </summary>
	public float MinDisplayScale { get; set; } = 1f;
	
	/// <summary>
	/// The max display scale to be scaled after the ModelScale is added
	/// This would be used for tiles that can be given a scale range when added to the world
	/// If no scale range is necessary, make this the same as the min display scale
	/// </summary>
	public float MaxDisplayScale { get; set; } = 1f;

	/// <summary>
	/// The offset of the model to center it on the tile
	/// </summary>
	public Vector3 PostScaleOffset { get; set; }
}