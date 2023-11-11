using Andavies.MonoGame.Inputs;
using Andavies.MonoGame.Inputs.Enums;
using Andavies.MonoGame.UI.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.MonoGame.UI.Core;

public abstract class UIElement : IUIElement
{
	private Rectangle _bounds = Rectangle.Empty;
	private bool _hasFocus; // Backing variable for HasFocus
	
	/// <summary>Use this constructor when the bounds are already defined as a Rectangle</summary>
	/// <param name="bounds">The bounds of this UI Element</param>
	protected UIElement(Rectangle bounds)
	{
		Bounds = bounds;
	}

	/// <summary>Use this constructor when a rectangle object has not already been created</summary>
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
	public event Action<IUIElement>? MouseClicked;
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

	public abstract void Draw(SpriteBatch spriteBatch);

	public virtual void Update(float deltaTimeSeconds)
	{
		if (!IsInteractable)
			return;
		
		CheckMousePosition();
		CheckMouseActions();
	}
	
	/// <summary>
	/// Add any logic that needs to occur when the bounds of this UI Element changed such as
	/// recalculating any children elements
	/// </summary>
	protected virtual void OnBoundsChanged() { }

	/// <summary>Checks for and raises mouse position events</summary>
	private void CheckMousePosition()
	{
		// Mouse Enter
		if (!IsElementHovered && Bounds.Contains(Input.CurrentMousePosition))
		{
			IsElementHovered = true;
			MouseEntered?.Invoke(this);
		}
		// Mouse Exit
		else if (IsElementHovered && !Bounds.Contains(Input.CurrentMousePosition))
		{
			IsElementHovered = false;
			MouseExited?.Invoke(this);
		}
	}

	/// <summary>Checks for and raises mouse press events</summary>
	private void CheckMouseActions()
	{
		// Mouse Pressed
		if (Input.WasMousePressed(MouseButton.Left) && Bounds.Contains(Input.CurrentMousePosition))
		{
			IsElementPressed = true;
			MousePressed?.Invoke(this);
		}
		// Mouse Released
		else if (Input.WasMouseReleased(MouseButton.Left))
		{
			if (Bounds.Contains(Input.CurrentMousePosition))
			{
				if (IsElementPressed)
					MouseClicked?.Invoke(this);
				MouseReleased?.Invoke(this);
			}

			IsElementPressed = false;
		}
	}
}