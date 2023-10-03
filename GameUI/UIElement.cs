namespace GameUI;

public abstract class UIElement
{
	protected UIElement(
		(float width, float height) size,
		(float x, float y) position)
	{
		Size = size;
		Position = position;
	}

	/// <summary>
	/// True if the UI element is visible
	/// False if the UI element is not visible
	/// </summary>
	public bool IsVisible { get; set; } = true;
	
	/// <summary>
	/// The width and height of the UI element
	/// </summary>
	public (float width, float height) Size { get; set; }
	
	/// <summary>
	/// The position that the UI element will be displayed anchored from the top left corner
	/// </summary>
	public (float x, float y) Position { get; set; }
}