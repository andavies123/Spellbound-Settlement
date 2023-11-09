using Andavies.MonoGame.UI.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Andavies.MonoGame.UI.Core;

public abstract class UIElement : IUIElement
{
	private Rectangle _bounds = Rectangle.Empty;
	private bool _isMouseDown = false;
	private bool _hasFocus = false; // Backing variable for HasFocus
	
	/// <summary>
	/// Use this constructor when the bounds are already defined as a Rectangle
	/// </summary>
	/// <param name="bounds">The bounds of this UI Element</param>
	protected UIElement(Rectangle bounds)
	{
		Bounds = bounds;
	}

	/// <summary>
	/// Use this constructor when a rectangle object has not already been created
	/// </summary>
	/// <param name="location">The location/position of the top left corner of the bounds</param>
	/// <param name="size">The size (width/height) of the bounds</param>
	protected UIElement(Point location, Point size) : 
		this(new Rectangle(location, size)) { }

	/// <summary>
	/// Constructor that will set the bounds location to (0, 0)
	/// Use this constructor when a parent layout group will have control over positions
	/// </summary>
	/// <param name="size">The size (width/height) of the bounds</param>
	protected UIElement(Point size) : 
		this(new Rectangle(new Point(0, 0), size)) { }
	
	public event Action<IUIElement>? MouseEntered;
	public event Action<IUIElement>? MouseExited;
	public event Action<IUIElement>? MousePressed;
	public event Action<IUIElement>? MouseReleased;
	public event Action<IUIElement>? ReceivedFocus;

	public Rectangle Bounds
	{
		get => _bounds;
		set
		{
			bool changed = _bounds != value;
			_bounds = value;
			if (changed)
				OnBoundsChanged();
		}
	}
	public int Width => Bounds.Width;
	public int Height => Bounds.Height;
	public bool IsVisible { get; set; } = true;
	public bool IsInteractable { get; set; } = true;
	
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
	
	/// <summary>
	/// Whether or not the mouse is currently being hovered over this UI Element
    /// True = Mouse is currently hovered
    /// False = Mouse is not currently hovered
	/// </summary>
	protected bool IsElementHovered { get; set; }
	
	/// <summary>
	/// Whether or not the mouse was pressed inside the bounds of this UI Element
	/// True = Mouse was pressed inside the bounds
	/// False = Mouse was not pressed inside the bounds or the mouse was released after being pressed in the bounds
	/// </summary>
	protected bool IsElementPressed { get; set; } = false;
	
	//Todo: Find someplace else for these static properties
	protected static Point CurrentMousePosition => Mouse.GetState().Position;
	protected static bool IsMousePressed => Mouse.GetState().LeftButton == ButtonState.Pressed;
	protected static bool IsMouseReleased => Mouse.GetState().LeftButton == ButtonState.Released;

	public abstract void Draw(SpriteBatch spriteBatch);

	public virtual void Update(float deltaTimeSeconds)
	{
		if (!IsInteractable)
			return;
		
		CheckMouseEntered();
		CheckMouseExited();
		CheckMousePressed();
		CheckMouseReleased();
	}
	
	/// <summary>
	/// Add any logic that needs to occur when the bounds of this UI Element changed such as
	/// recalculating any children elements
	/// </summary>
	protected virtual void OnBoundsChanged() { }
	
	/// <summary>
	/// Checks to see if the mouse has entered the bounds of this element whether it is pressed or not
	/// </summary>
	protected virtual void CheckMouseEntered()
	{
		if (IsElementHovered || !Bounds.Contains(CurrentMousePosition)) 
			return;
		
		IsElementHovered = true;
		MouseEntered?.Invoke(this);
	}
	
	/// <summary>
	/// Checks to see if the mouse has exited the bounds of this element whether it is pressed or not
	/// </summary>
	protected virtual void CheckMouseExited()
	{
		if (!IsElementHovered || Bounds.Contains(CurrentMousePosition)) 
			return;
		
		IsElementHovered = false;
		MouseExited?.Invoke(this);
	}

	/// <summary>
	/// Checks to see if the mouse was pressed inside the bounds of this element
	/// </summary>
	protected virtual void CheckMousePressed()
	{
		if (_isMouseDown || !IsMousePressed) 
			return;
		
		_isMouseDown = true;

		if (!Bounds.Contains(CurrentMousePosition)) 
			return;
		
		IsElementPressed = true;
		MousePressed?.Invoke(this);
	}

	/// <summary>
	/// Checks to see if the mouse was released inside the bounds of this element
	/// </summary>
	protected virtual void CheckMouseReleased()
	{
		if (!_isMouseDown || !IsMouseReleased) 
			return;
		
		_isMouseDown = false;
		IsElementPressed = false;
		if (Bounds.Contains(CurrentMousePosition))
			MouseReleased?.Invoke(this);
	}
}