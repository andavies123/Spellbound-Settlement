using Andavies.MonoGame.Inputs.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Andavies.MonoGame.Inputs;

/// <summary>
/// Static class that handles the main functions of listening for inputs.
/// <see cref="Update"/> must be called at the beginning of each frame in order to set the Previous/Current Input States
/// </summary>
public class InputManager : IInputManager
{
	public InputManager()
	{
		SetInputStates();
	}
	
	public event Action? MouseMoved;
	public event Action? LeftMouseButtonPressed;
	public event Action? RightMouseButtonPressed;
	
	public Point CurrentMousePosition => CurrentMouseState.Position;
	public IReadOnlyList<Keys> KeysPressedThisFrame => CurrentKeyboardState.GetPressedKeys().Except(PreviousKeyboardState?.GetPressedKeys() ?? Array.Empty<Keys>()).ToList();
	
	private static KeyboardState? PreviousKeyboardState { get; set; }
	private static KeyboardState CurrentKeyboardState { get; set; }
	private static MouseState? PreviousMouseState { get; set; }
	private static MouseState CurrentMouseState { get; set; }

	public void Update()
	{
		SetInputStates();
		
		// Check for mouse movement
		if (!PreviousMouseState.HasValue || PreviousMouseState.Value.Position != CurrentMousePosition)
			MouseMoved?.Invoke();
		
		if (WasMousePressed(MouseButton.Left))
			LeftMouseButtonPressed?.Invoke();
		
		if (WasMousePressed(MouseButton.Right))
			RightMouseButtonPressed?.Invoke();
	}
	
	public bool WasKeyPressed(Keys key)
	{
		return (PreviousKeyboardState?.IsKeyUp(key) ?? false) && CurrentKeyboardState.IsKeyDown(key);
	}

	public bool WasKeyReleased(Keys key)
	{
		return (PreviousKeyboardState?.IsKeyDown(key) ?? false) && CurrentKeyboardState.IsKeyUp(key);
	}

	public bool IsKeyDown(Keys key)
	{
		return CurrentKeyboardState.IsKeyDown(key);
	}

	public bool IsKeyUp(Keys key)
	{
		return CurrentKeyboardState.IsKeyUp(key);
	}
	
	public bool WasMousePressed(MouseButton mouseButton)
	{
		if (!TryGetButtonStates(mouseButton, out ButtonState previousButtonState, out ButtonState currentButtonState))
			return false;

		return previousButtonState == ButtonState.Released && currentButtonState == ButtonState.Pressed;
	}
	
	public bool WasMouseReleased(MouseButton mouseButton)
	{
		if (!TryGetButtonStates(mouseButton, out ButtonState previousButtonState, out ButtonState currentButtonState))
			return false;

		return previousButtonState == ButtonState.Pressed && currentButtonState == ButtonState.Released;
	}

	public bool IsMouseDown(MouseButton mouseButton)
	{
		if (!TryGetButtonState(mouseButton, CurrentMouseState, out ButtonState currentButtonState))
			return false;

		return currentButtonState == ButtonState.Pressed;
	}

	public bool IsMouseUp(MouseButton mouseButton)
	{
		if (!TryGetButtonState(mouseButton, CurrentMouseState, out ButtonState currentButtonState))
			return false;

		return currentButtonState == ButtonState.Released;
	}

	private void SetInputStates()
	{
		PreviousKeyboardState = CurrentKeyboardState;
		CurrentKeyboardState = Keyboard.GetState();

		PreviousMouseState = CurrentMouseState;
		CurrentMouseState = Mouse.GetState();
	}

	private bool TryGetButtonState(MouseButton mouseButton, MouseState? mouseState, out ButtonState buttonState)
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

	private bool TryGetButtonStates(MouseButton mouseButton, out ButtonState previousButtonState, out ButtonState currentButtonState)
	{
		previousButtonState = default;
		currentButtonState = default;

		return TryGetButtonState(mouseButton, PreviousMouseState, out previousButtonState) &&
		       TryGetButtonState(mouseButton, CurrentMouseState, out currentButtonState);
	}
}