using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.MonoGame.UI.Interfaces;

public interface IUIElement
{
	/// <summary>
	/// Raised when the mouse first enters the bounds of this element
	/// </summary>
	event Action<IUIElement>? MouseEntered;
	
	/// <summary>
	/// Raised when the mouse first exists the bounds of this element
	/// </summary>
	event Action<IUIElement>? MouseExited;
	
	/// <summary>
	/// Raised when the mouse is first pressed inside the bounds of this element
	/// </summary>
	event Action<IUIElement>? MousePressed;
	
	/// <summary>
	/// Raised when the mouse is first released inside the bounds of this element
	/// </summary>
	event Action<IUIElement>? MouseReleased;
	
	/// <summary>
	/// Raised when this UIElement has gained focus
	/// </summary>
	event Action<IUIElement>? ReceivedFocus;
	
	/// <summary>
	/// The rectangular bounds that defines the area of this element.
	/// Usually set by an element's parent such as a layout group
	/// </summary>
	Rectangle Bounds { get; set; }

	/// <summary>
	/// References the width of this UI Element's bounds.
	/// Points to Bounds.Width
	/// </summary>
	int Width { get; }
	
	/// <summary>
	/// References the height of this UI Element's bounds.
	/// Points to Bounds.Height
	/// </summary>
	int Height { get; }

	/// <summary>
	/// Whether or not this UI Element should be drawn
	/// True = Should be drawn
	/// False = Should not be drawn
	/// </summary>
	bool IsVisible { get; set; }
	
	/// <summary>
	/// Whether or not this UI Element has focus from the user
	/// True = Has Focus
	/// False = Does not have focus
	/// </summary>
	bool HasFocus { get; set; }

	/// <summary>
	/// Used to update any elements. Can be used for animation or anything that requires logic updates
	/// </summary>
	/// <param name="deltaTimeSeconds">Time that has passed for this frame</param>
	void Update(float deltaTimeSeconds);

	/// <summary>
	/// Used to draw the UIElement or even call draw on a UIElements children components
	/// </summary>
	/// <param name="spriteBatch">Used to draw 2D Graphics</param>
	void Draw(SpriteBatch spriteBatch);
}