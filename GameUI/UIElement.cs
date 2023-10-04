using GameUI.Styles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameUI;

public abstract class UIElement
{
	private bool _isMouseDown = false;
	
	protected UIElement(Point position, Point size, LayoutAnchor anchor)
	{
		Position = position;
		Size = size;
		Anchor = anchor;
	}

	/// <summary>
	/// Raised when the mouse first enters the bounds of this element
	/// </summary>
	public event Action? MouseEntered;
	
	/// <summary>
	/// Raised when the mouse first exists the bounds of this element
	/// </summary>
	public event Action? MouseExited;

	/// <summary>
	/// Raised when the mouse is first pressed inside the bounds of this element
	/// </summary>
	public event Action? MousePressed;

	/// <summary>
	/// Raised when the mouse is first released inside the bounds of this element
	/// </summary>
	public event Action? MouseReleased;

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
	/// Which part of the screen the UIElement will be anchored to. Affects positioning
	/// </summary>
	public LayoutAnchor Anchor { get; set; } = LayoutAnchor.MiddleCenter;

	/// <summary>
	/// True if the UI element is visible
	/// False if the UI element is not visible
	/// </summary>
	public bool IsVisible { get; set; } = true;
	
	/// <summary>
	/// The physical bounds of the UI Element used for drawing.
	/// This value is calculated internally using anchors, position, and size
	/// </summary>
	protected Rectangle Bounds { get; private set; }

	/// <summary>
	/// True if the mouse is inside the bounds of this element. Also known as hovering.
	/// False if the mouse is outside the bounds of this element
	/// </summary>
	protected bool IsMouseInside { get; set; } = false;
	
	/// <summary>
	/// True if the mouse was pressed inside the bounds of this element. The mouse can leave the bounds and stay pressed
	/// False if the mouse is released or was not pressed inside the bounds of this element. A mouse press outside then drag
	/// into the bounds should still result in false
	/// </summary>
	protected bool IsElementPressed { get; set; } = false;
	
	protected static Point CurrentMousePosition => Mouse.GetState().Position;
	protected static bool IsMousePressed => Mouse.GetState().LeftButton == ButtonState.Pressed;
	protected static bool IsMouseReleased => Mouse.GetState().LeftButton == ButtonState.Released;
	
	/// <summary>
	/// Calculates the bounds property using the position/size/anchor and the given screen size
	/// </summary>
	/// <param name="screenSize">The size of the screen. Used alongside the anchor</param>
	public void CalculateBounds(Point screenSize)
	{
		Point position = Anchor switch
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

	/// <summary>
	/// In charge of drawing the UIElement.
	/// The given SpriteBatch should have Begin() called before this method is called
	/// </summary>
	/// <param name="spriteBatch"></param>
	public abstract void Draw(SpriteBatch spriteBatch);

	/// <summary>
	/// Checks the mouse position to raise certain mouse events
	/// </summary>
	public virtual void Update()
	{
		CheckMouseEntered();
		CheckMouseExited();
		CheckMousePressed();
		CheckMouseReleased();
	}
	
	/// <summary>
	/// Checks to see if the mouse has entered the bounds of this element whether it is pressed or not
	/// </summary>
	protected virtual void CheckMouseEntered()
	{
		if (!IsMouseInside && Bounds.Contains(CurrentMousePosition))
		{
			IsMouseInside = true;
			MouseEntered?.Invoke();
		}
	}

	/// <summary>
	/// Checks to see if the mouse has exited the bounds of this element whether it is pressed or not
	/// </summary>
	protected virtual void CheckMouseExited()
	{
		if (IsMouseInside && !Bounds.Contains(CurrentMousePosition))
		{
			IsMouseInside = false;
			MouseExited?.Invoke();
		}
	}

	/// <summary>
	/// Checks to see if the mouse was pressed inside the bounds of this element
	/// </summary>
	protected virtual void CheckMousePressed()
	{
		if (!_isMouseDown && IsMousePressed)
		{
			_isMouseDown = true;
			if (Bounds.Contains(CurrentMousePosition))
			{
				MousePressed?.Invoke();
				IsElementPressed = true;
			}
		}
	}

	/// <summary>
	/// Checks to see if the mouse was released
	/// </summary>
	protected virtual void CheckMouseReleased()
	{
		if (_isMouseDown && IsMouseReleased)
		{
			_isMouseDown = false;
			IsElementPressed = false;
			if (Bounds.Contains(CurrentMousePosition))
				MouseReleased?.Invoke();
		}
	}
}