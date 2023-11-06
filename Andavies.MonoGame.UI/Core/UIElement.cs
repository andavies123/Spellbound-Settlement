using Andavies.MonoGame.UI.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Andavies.MonoGame.UI.Core;

public abstract class UIElement : IUIElement
{
	private bool _isMouseDown = false;
	private bool _hasFocus = false; // Backing variable for HasFocus
	
	protected UIElement(Rectangle bounds)
	{
		Bounds = bounds;
	}

	protected UIElement(Point location, Point size)
	{
		Bounds = new Rectangle(location, size);
	}
	
	public event Action? MouseEntered;
	public event Action? MouseExited;
	public event Action? MousePressed;
	public event Action? MouseReleased;
	public event Action<IUIElement>? ReceivedFocus;
	
	public Rectangle Bounds { get; set; }
	public bool IsVisible { get; set; } = true;
	
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
		if (IsElementHovered || !Bounds.Contains(CurrentMousePosition)) 
			return;
		
		IsElementHovered = true;
		MouseEntered?.Invoke();
	}
	
	/// <summary>
	/// Checks to see if the mouse has exited the bounds of this element whether it is pressed or not
	/// </summary>
	protected virtual void CheckMouseExited()
	{
		if (!IsElementHovered || Bounds.Contains(CurrentMousePosition)) 
			return;
		
		IsElementHovered = false;
		MouseExited?.Invoke();
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
		
		MousePressed?.Invoke();
		IsElementPressed = true;
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
			MouseReleased?.Invoke();
	}
}