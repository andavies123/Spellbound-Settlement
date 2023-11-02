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

	/// <summary>
	/// The anchored position of this element on the screen.
	/// This value is not the exact position as it is affected by Layout Anchors.
	/// </summary>
	public Point Position { get; set; }

	/// <summary>The size of the UI element. X = width. Y = height</summary>
	public Point Size { get; set; }

	/// <summary>Which part of the screen the UIElement will be anchored to. Affects positioning</summary>
	public LayoutAnchor LayoutAnchor { get; set; } = LayoutAnchor.MiddleCenter;

	/// <summary>True if the UI element will be drawn. False if it won't be drawn</summary>
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
	protected Rectangle Bounds => new(Position, Size);

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

	// In charge of drawing the UIElement.
	// The given SpriteBatch should have Begin() called before this method is called
	public abstract void Draw(SpriteBatch spriteBatch);

	/// <summary>Checks the mouse position to raise certain mouse events</summary>
	public virtual void Update()
	{
		CheckMouseEntered();
		CheckMouseExited();
		CheckMousePressed();
		CheckMouseReleased();
	}
	
	/// <summary>Checks to see if the mouse has entered the bounds of this element whether it is pressed or not</summary>
	protected virtual void CheckMouseEntered()
	{
		if (!IsMouseInside && Bounds.Contains(CurrentMousePosition))
		{
			IsMouseInside = true;
			MouseEntered?.Invoke();
		}
	}
	
	/// <summary>Checks to see if the mouse has exited the bounds of this element whether it is pressed or not</summary>
	protected virtual void CheckMouseExited()
	{
		if (IsMouseInside && !Bounds.Contains(CurrentMousePosition))
		{
			IsMouseInside = false;
			MouseExited?.Invoke();
		}
	}

	/// <summary>Checks to see if the mouse was pressed inside the bounds of this element</summary>
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

	/// <summary>Checks to see if the mouse was released inside the bounds of this element</summary>
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