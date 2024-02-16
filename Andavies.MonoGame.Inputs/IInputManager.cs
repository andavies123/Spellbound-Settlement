using Andavies.MonoGame.Inputs.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Andavies.MonoGame.Inputs;

public interface IInputManager
{
	/// <summary>
	/// Raised when the mouse moved during this last frame
	/// </summary>
	event Action? MouseMoved;
	
	/// <summary>
	/// Raised when the left mouse button was pressed during the last frame
	/// </summary>
	event Action? LeftMouseButtonPressed;
	
	/// <summary>
	/// Raised when the right mouse button was pressed during the last frame
	/// </summary>
	event Action? RightMouseButtonPressed;

	/// <summary>
	/// The position of the mouse on screen during this frame
	/// </summary>
	Point CurrentMousePosition { get; }
	
	/// <summary>
	/// A list of keys that were pressed this frame. This will not return all keys currently pressed, only new ones
	/// </summary>
	IReadOnlyList<Keys> KeysPressedThisFrame { get; }

	/// <summary>Checks a given key to see if it was pressed during this last frame</summary>
	/// <param name="key">The key to check against</param>
	/// <returns>True if the given key was pressed this last frame. False otherwise</returns>
	bool WasKeyPressed(Keys key);
	
	/// <summary>Checks a given key to see if it was released during this last frame</summary>
	/// <param name="key">The key to check against</param>
	/// <returns>True if the given key was released this last frame. False otherwise</returns>
	bool WasKeyReleased(Keys key);

	/// <summary>Checks a given key to see if it is currently pressed down</summary>
	/// <param name="key">The key to check against</param>
	/// <returns>True if the given key is currently pressed down. False otherwise</returns>
	bool IsKeyDown(Keys key);

	/// <summary>Checks a given key to see if it is not currently pressed down</summary>
	/// <param name="key">The key to check against</param>
	/// <returns>True if the given key is NOT currently pressed down. False otherwise</returns>
	bool IsKeyUp(Keys key);

	/// <summary>Checks a given mouse button to see if it was pressed during this last frame</summary>
	/// <param name="mouseButton">The mouse button to check against</param>
	/// <returns>True if the given mouse button was pressed this last frame. False otherwise</returns>
	bool WasMousePressed(MouseButton mouseButton);

	/// <summary>Checks a given mouse button to see if it was released during this last frame</summary>
	/// <param name="mouseButton">The mouse button to check against</param>
	/// <returns>True if the given mouse button was released this last frame. False otherwise</returns>
	bool WasMouseReleased(MouseButton mouseButton);
	
	/// <summary>Checks a given mouse button to see if it is currently pressed down</summary>
	/// <param name="mouseButton">The mouse button to check against</param>
	/// <returns>True if the given mouse button is currently pressed. False otherwise</returns>
	bool IsMouseDown(MouseButton mouseButton);

	/// <summary>Checks a given mouse button to see if it is NOT currently pressed down</summary>
	/// <param name="mouseButton">The mouse button to check against</param>
	/// <returns>True if the given mouse button is NOT currently pressed. False otherwise</returns>
	bool IsMouseUp(MouseButton mouseButton);
}