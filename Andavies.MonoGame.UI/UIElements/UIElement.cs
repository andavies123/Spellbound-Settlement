using Andavies.MonoGame.UI.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Andavies.MonoGame.UI.UIElements;

public abstract class UIElement
{
	private bool _isMouseDown = false;
	private bool _hasFocus = false; // Backing variable for HasFocus

	/// <summary>Raised when the mouse first enters the bounds of this element</summary>
	public event Action? MouseEntered;
	/// <summary>Raised when the mouse first exists the bounds of this element</summary>
	public event Action? MouseExited;
	/// <summary>Raised when the mouse is first pressed inside the bounds of this element</summary>
	public event Action? MousePressed;
	/// <summary>Raised when the mouse is first released inside the bounds of this element</summary>
	public event Action? MouseReleased;
	/// <summary>Raised when this UIElement has gained focus</summary>
	public event Action<UIElement>? ReceivedFocus;

	// The anchored position of this element on the screen.
	// This value is not the exact position as it is affected by Layout Anchors.
	public Point Position { get; set; }

	// The size of the UI element. X = width. Y = height
	public Point Size { get; set; }

	// Which part of the screen the UIElement will be anchored to. Affects positioning
	public LayoutAnchor LayoutAnchor { get; set; } = LayoutAnchor.MiddleCenter;

	// True if the UI element will be drawn. False if it won't be drawn
	public bool IsVisible { get; set; } = true;
	
	/// <summary>Whether or not this UIElement has focus or not</summary>
	public bool HasFocus
	{
		get => _hasFocus;
		set
		{
			_hasFocus = value;
			if (_hasFocus)
				ReceivedFocus?.Invoke(this);
		}
	}
	
	// The physical bounds of the UI Element used for drawing.
	// This value is calculated internally using anchors, position, and size
	protected Rectangle Bounds { get; private set; }

	// True if the mouse is inside the bounds of this element. Also known as hovering.
	// False if the mouse is outside the bounds of this element
	protected bool IsMouseInside { get; set; } = false;
	
	// True if the mouse was pressed inside the bounds of this element. The mouse can leave the bounds and stay pressed
	// False if the mouse is released or was not pressed inside the bounds of this element. A mouse press outside then drag
	// into the bounds should still result in false
	protected bool IsElementPressed { get; set; } = false;
	
	protected static Point CurrentMousePosition => Mouse.GetState().Position;
	protected static bool IsMousePressed => Mouse.GetState().LeftButton == ButtonState.Pressed;
	protected static bool IsMouseReleased => Mouse.GetState().LeftButton == ButtonState.Released;

	// Builder method for setting the layout anchor
	public UIElement SetLayoutAnchor(LayoutAnchor anchor)
	{
		LayoutAnchor = anchor;
		return this;
	}
	
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

	// In charge of drawing the UIElement.
	// The given SpriteBatch should have Begin() called before this method is called
	public abstract void Draw(SpriteBatch spriteBatch);

	// Checks the mouse position to raise certain mouse events
	public virtual void Update()
	{
		CheckMouseEntered();
		CheckMouseExited();
		CheckMousePressed();
		CheckMouseReleased();
	}
	
	// Checks to see if the mouse has entered the bounds of this element whether it is pressed or not
	protected virtual void CheckMouseEntered()
	{
		if (!IsMouseInside && Bounds.Contains(CurrentMousePosition))
		{
			IsMouseInside = true;
			MouseEntered?.Invoke();
		}
	}

	
	// Checks to see if the mouse has exited the bounds of this element whether it is pressed or not
	protected virtual void CheckMouseExited()
	{
		if (IsMouseInside && !Bounds.Contains(CurrentMousePosition))
		{
			IsMouseInside = false;
			MouseExited?.Invoke();
		}
	}

	// Checks to see if the mouse was pressed inside the bounds of this element
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

	// Checks to see if the mouse was released inside the bounds of this element
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