using Microsoft.Xna.Framework;

namespace Andavies.SpellboundSettlement;

public readonly struct ModelDetails
{
	public ModelDetails(string contentModelPath)
	{
		ContentModelPath = contentModelPath;
	}

	/// <summary>
	/// The path of the model related to the Content folder
	/// </summary>
	public string ContentModelPath { get; }
	
	/// <summary>
	/// Used to scale a model down to tile size.
	/// This will be the default scale before applying the display scale
	/// </summary>
	public float ModelScale { get; init; } = 1/32f;

	/// <summary>
	/// The offset of the model to center it on the tile
	/// </summary>
	public Vector3 PostScaleOffset { get; init; } = new(0.5f, 0.5f, 0.5f);
}