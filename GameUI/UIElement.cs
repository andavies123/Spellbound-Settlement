using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameUI;

public abstract class UIElement
{
	protected UIElement(Point position, Point size)
	{
		Position = position;
		Size = size;
	}
	
	/// <summary>
	/// The physical bounds of the UI Element used for drawing.
	/// This value is calculated internally using anchors, position, and size
	/// </summary>
	public Rectangle Bounds { get; private set; }

	/// <summary>
	/// The anchored position of this element on the screen.
	/// This value is not the exact position as it is affected by Layout Anchors.
	/// </summary>
	public Point Position { get; set; }

	/// <summary>
	/// The size of the UI element
	/// X = Width
	/// Y = Height
	/// </summary>
	public Point Size { get; set; }

	/// <summary>
	/// Which part of the UI Element will be used for displaying
	/// </summary>
	public LayoutAnchor LayoutAnchor { get; set; }

	/// <summary>
	/// True if the UI element is visible
	/// False if the UI element is not visible
	/// </summary>
	public bool IsVisible { get; set; } = true;
	
	/// <summary>
	/// The texture to use for the background combined with <see cref="BackgroundColor"/>
	/// </summary>
	public Texture2D? BackgroundTexture { get; set; }

	/// <summary>
	/// The color used for the background combined with <see cref="BackgroundTexture"/>
	/// </summary>
	public Color BackgroundColor { get; set; } = Color.Transparent;

	/// <summary>
	/// In charge of drawing the UIElement.
	/// The given SpriteBatch should have Begin() called before this method is called
	/// </summary>
	/// <param name="spriteBatch"></param>
	public abstract void Draw(SpriteBatch spriteBatch);

	/// <summary>
	/// Calculates the bounds property using the position/size/anchor and the given screen size
	/// </summary>
	/// <param name="screenSize">The size of the screen. Used alongside the anchor</param>
	public void CalculateBounds(Point screenSize)
	{
		Point position = LayoutAnchor switch
		{
			LayoutAnchor.TopLeft => Position,
			LayoutAnchor.TopCenter => new Point(screenSize.X/2 - Size.X/2 + Position.X, Position.Y),
			LayoutAnchor.TopRight => new Point(screenSize.X - Size.X + Position.X, Position.Y),
			LayoutAnchor.MiddleLeft => new Point(Position.X, screenSize.Y/2 - Size.Y/2 + Position.Y),
			LayoutAnchor.MiddleCenter => new Point(screenSize.X/2 - Size.X/2 + Position.X, screenSize.Y/2 - Size.Y/2 + Position.Y),
			LayoutAnchor.MiddleRight => new Point(screenSize.X - Size.X + Position.X, screenSize.Y/2 - Size.Y/2 + Position.Y),
			LayoutAnchor.BottomLeft => new Point(Position.X, screenSize.Y - Size.Y + Position.Y),
			LayoutAnchor.BottomCenter => new Point(screenSize.X/2 - Size.X/2 + Position.X, screenSize.Y - Size.Y + Position.Y),
			LayoutAnchor.BottomRight => new Point(screenSize.X - Size.X + Position.X, screenSize.Y - Size.Y + Position.Y),
			_ => Position
		};
		
		Bounds = new Rectangle(position, Size);
	}
}