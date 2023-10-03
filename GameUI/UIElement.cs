using Microsoft.Xna.Framework;

namespace GameUI;

public abstract class UIElement
{
	protected UIElement(
		Rectangle bounds,
		LayoutAnchor elementAnchor,
		LayoutAnchor screenAnchor)
	{
		Bounds = bounds;
		ElementAnchor = elementAnchor;
		ScreenAnchor = screenAnchor;
	}
	
	/// <summary>
	/// The physical bounds of the UI Element.
	/// Contains the position and size
	/// </summary>
	public Rectangle Bounds { get; set; }
	
	/// <summary>
	/// Which part of the UI Element will be used for displaying
	/// </summary>
	public LayoutAnchor ElementAnchor { get; set; }
	
	/// <summary>
	/// Which section of the screen will be used for displaying
	/// </summary>
	public LayoutAnchor ScreenAnchor { get; set; }

	/// <summary>
	/// True if the UI element is visible
	/// False if the UI element is not visible
	/// </summary>
	public bool IsVisible { get; set; } = true;

}