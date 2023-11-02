using Microsoft.Xna.Framework.Input;

namespace Andavies.MonoGame.Input;

/// <summary>
/// Static class that handles the main functions of listening for inputs.
/// <see cref="Update"/> must be called at the beginning of each frame in order to set the Previous/Current Input States
/// </summary>
public static class Input
{
	private static KeyboardState? PreviousKeyboardState { get; set; }
	private static KeyboardState? CurrentKeyboardState { get; set; }

	/// <summary>Call this at the beginning of each frame to update the Input States</summary>
	public static void Update()
	{
		PreviousKeyboardState = CurrentKeyboardState;
		CurrentKeyboardState = Keyboard.GetState();
	}

	/// <summary>Returns whether or not a key was pressed in this frame</summary>
	public static bool WasKeyPressed(Keys key) =>
		(PreviousKeyboardState?.IsKeyUp(key) ?? false) && (CurrentKeyboardState?.IsKeyDown(key) ?? false);
	
	/// <summary>Returns whether or not a key was released in this frame</summary>
	public static bool WasKeyReleased(Keys key) =>
		(PreviousKeyboardState?.IsKeyDown(key) ?? false) && (CurrentKeyboardState?.IsKeyUp(key) ?? false);

	/// <summary>Returns whether or not a key is currently pressed down</summary>
	public static bool IsKeyDown(Keys key) =>
		CurrentKeyboardState?.IsKeyDown(key) ?? false;

	/// <summary>Returns whether or not a key is currently released (not pressed)</summary>
	public static bool IsKeyUp(Keys key) =>
		CurrentKeyboardState?.IsKeyUp(key) ?? false;
}