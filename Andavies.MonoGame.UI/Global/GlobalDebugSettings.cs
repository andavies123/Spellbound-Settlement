using Microsoft.Xna.Framework;

namespace Andavies.MonoGame.UI.Global;

/// <summary>
/// Static class containing settings related to debugging for UI objects
/// </summary>
public static class GlobalDebugSettings
{
	/// <summary>
	/// Setting to toggle whether to draw the bounds of each UIElement
	/// </summary>
	public static bool DrawDebugBounds { get; set; } = false;

	/// <summary>
	/// The color the debug bounds will be drawn with
	/// </summary>
	public static Color DebugBoundsColor { get; set; } = Color.Red;

	/// <summary>
	/// The width of the lines the debug bounds will be drawn with
	/// </summary>
	public static int DebugBoundsThickness { get; set; } = 1;
}