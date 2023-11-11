using Andavies.MonoGame.Inputs.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Andavies.MonoGame.Inputs;

/// <summary>
/// Static class that handles the main functions of listening for inputs.
/// <see cref="Update"/> must be called at the beginning of each frame in order to set the Previous/Current Input States
/// </summary>
public static class Input
{
	/// <summary>The position of the mouse on the screen during this frame</summary>
	public static Point CurrentMousePosition => CurrentMouseState.Position;
	
	private static KeyboardState? PreviousKeyboardState { get; set; }
	private static KeyboardState CurrentKeyboardState { get; set; }
	private static MouseState? PreviousMouseState { get; set; }
	private static MouseState CurrentMouseState { get; set; }

	/// <summary>Static constructor to initialize the current state</summary>
	static Input()
	{
		SetInputStates();
	}

	/// <summary>Call this at the beginning of each frame to update the Input States</summary>
	public static void Update()
	{
		SetInputStates();
	}

	/// <summary>Checks a given key to see if it was pressed during this last frame</summary>
	/// <param name="key">The key to check against</param>
	/// <returns>True if the given key was pressed this last frame. False otherwise</returns>
	public static bool WasKeyPressed(Keys key) =>
		(PreviousKeyboardState?.IsKeyUp(key) ?? false) && CurrentKeyboardState.IsKeyDown(key);
	
	/// <summary>Checks a given key to see if it was released during this last frame</summary>
	/// <param name="key">The key to check against</param>
	/// <returns>True if the given key was released this last frame. False otherwise</returns>
	public static bool WasKeyReleased(Keys key) =>
		(PreviousKeyboardState?.IsKeyDown(key) ?? false) && CurrentKeyboardState.IsKeyUp(key);

	/// <summary>Checks a given key to see if it is currently pressed down</summary>
	/// <param name="key">The key to check against</param>
	/// <returns>True if the given key is currently pressed down. False otherwise</returns>
	public static bool IsKeyDown(Keys key) => CurrentKeyboardState.IsKeyDown(key);

	/// <summary>Checks a given key to see if it is not currently pressed down</summary>
	/// <param name="key">The key to check against</param>
	/// <returns>True if the given key is NOT currently pressed down. False otherwise</returns>
	public static bool IsKeyUp(Keys key) => CurrentKeyboardState.IsKeyUp(key);
	
	/// <summary>Checks a given mouse button to see if it was pressed during this last frame</summary>
	/// <param name="mouseButton">The mouse button to check against</param>
	/// <returns>True if the given mouse button was pressed this last frame. False otherwise</returns>
	public static bool WasMousePressed(MouseButton mouseButton)
	{
		if (!TryGetButtonStates(mouseButton, out ButtonState previousButtonState, out ButtonState currentButtonState))
			return false;

		return previousButtonState == ButtonState.Released && currentButtonState == ButtonState.Pressed;
	}

	/// <summary>Checks a given mouse button to see if it was released during this last frame</summary>
	/// <param name="mouseButton">The mouse button to check against</param>
	/// <returns>True if the given mouse button was released this last frame. False otherwise</returns>
	public static bool WasMouseReleased(MouseButton mouseButton)
	{
		if (!TryGetButtonStates(mouseButton, out ButtonState previousButtonState, out ButtonState currentButtonState))
			return false;

		return previousButtonState == ButtonState.Pressed && currentButtonState == ButtonState.Released;
	}

	/// <summary>Checks a given mouse button to see if it is currently pressed down</summary>
	/// <param name="mouseButton">The mouse button to check against</param>
	/// <returns>True if the given mouse button is currently pressed. False otherwise</returns>
	public static bool IsMousePressed(MouseButton mouseButton)
	{
		if (!TryGetButtonState(mouseButton, CurrentMouseState, out ButtonState currentButtonState))
			return false;

		return currentButtonState == ButtonState.Pressed;
	}

	/// <summary>Checks a given mouse button to see if it is NOT currently pressed down</summary>
	/// <param name="mouseButton">The mouse button to check against</param>
	/// <returns>True if the given mouse button is NOT currently pressed. False otherwise</returns>
	public static bool IsMouseReleased(MouseButton mouseButton)
	{
		if (!TryGetButtonState(mouseButton, CurrentMouseState, out ButtonState currentButtonState))
			return false;

		return currentButtonState == ButtonState.Released;
	}

	private static void SetInputStates()
	{
		PreviousKeyboardState = CurrentKeyboardState;
		CurrentKeyboardState = Keyboard.GetState();

		PreviousMouseState = CurrentMouseState;
		CurrentMouseState = Mouse.GetState();
	}

	private static bool TryGetButtonState(MouseButton mouseButton, MouseState? mouseState, out ButtonState buttonState)
	{
		buttonState = default;

		if (!mouseState.HasValue)
			return false;

		buttonState = mouseButton switch
		{
			MouseButton.Left => mouseState.Value.LeftButton,
			MouseButton.Middle => mouseState.Value.MiddleButton,
			MouseButton.Right => mouseState.Value.RightButton,
			_ => throw new ArgumentOutOfRangeException(nameof(mouseButton), mouseButton, null)
		};

		return true;
	}

	private static bool TryGetButtonStates(MouseButton mouseButton, out ButtonState previousButtonState, out ButtonState currentButtonState)
	{
		previousButtonState = default;
		currentButtonState = default;

		return TryGetButtonState(mouseButton, PreviousMouseState, out previousButtonState) &&
		       TryGetButtonState(mouseButton, CurrentMouseState, out currentButtonState);
	}
}