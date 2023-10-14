using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpellboundSettlement.Inputs;

public class GameplayInputManager : IInputManager
{
	private const Keys MoveCameraForwardsKey = Keys.W;
	private const Keys MoveCameraBackwardsKey = Keys.S;
	private const Keys MoveCameraLeftKey = Keys.A;
	private const Keys MoveCameraRightKey = Keys.D;
	private const Keys RotateCameraKey = Keys.R;
	private const Keys RotateCamera90ClockwiseKey = Keys.Q;
	private const Keys RotateCamera90CounterClockwiseKey = Keys.E;

	private const Keys PauseGameKey = Keys.Escape;

	private bool _isMoveCameraActive = false;
	private int _previousScrollWheelValue = Mouse.GetState().ScrollWheelValue;

	public Vector2 MoveCameraInput { get; private set; }

	public readonly KeyAction RotateCamera90CW = new(RotateCamera90ClockwiseKey);
	public readonly KeyAction RotateCamera90CCW = new(RotateCamera90CounterClockwiseKey);
	public readonly KeyAction PauseGame = new(PauseGameKey);
	
	public event Action MoveCameraStarted;
	public event Action MoveCameraStopped;
	public event Action ZoomIn;
	public event Action ZoomOut;
	
	public void UpdateInput()
	{
		PauseGame.CheckKey();
		RotateCamera90CW.CheckKey();
		RotateCamera90CCW.CheckKey();
		
		CheckForceQuit();
		CheckMoveCameraInput();
		CheckZoomInput();
	}

	private void CheckForceQuit()
	{
		if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Keyboard.GetState().IsKeyDown(Keys.Escape))
			GameManager.QuitGame();
	}

	private void CheckMoveCameraInput()
	{
		float verticalMovement = 0f;
		float horizontalMovement = 0f;

		if (Keyboard.GetState().IsKeyDown(MoveCameraForwardsKey))
			verticalMovement += 1f;
		if (Keyboard.GetState().IsKeyDown(MoveCameraBackwardsKey))
			verticalMovement -= 1f;
		if (Keyboard.GetState().IsKeyDown(MoveCameraRightKey))
			horizontalMovement -= 1f;
		if (Keyboard.GetState().IsKeyDown(MoveCameraLeftKey))
			horizontalMovement += 1f;

		MoveCameraInput = new Vector2(horizontalMovement, verticalMovement);
		if (!_isMoveCameraActive && MoveCameraInput != Vector2.Zero)
		{
			_isMoveCameraActive = true;
			MoveCameraStarted?.Invoke();
		}
		else if (_isMoveCameraActive && MoveCameraInput == Vector2.Zero)
		{
			_isMoveCameraActive = false;
			MoveCameraStopped?.Invoke();
		}
	}

	private void CheckZoomInput()
	{
		// Zoom
		int currentScrollWheelValue = Mouse.GetState().ScrollWheelValue;
		if (_previousScrollWheelValue - currentScrollWheelValue > 0)
			ZoomOut?.Invoke();
		else if (_previousScrollWheelValue - currentScrollWheelValue < 0)
			ZoomIn?.Invoke();
		_previousScrollWheelValue = currentScrollWheelValue;
	}
}

public class KeyAction
{
	public KeyAction(Keys key) => Key = key;

	public bool IsKeyDown { get; private set; } = false;
	public Keys Key { get; set; }
	
	public event Action OnKeyDown;
	public event Action OnKeyUp;

	public void CheckKey()
	{
		if (!IsKeyDown && Keyboard.GetState().IsKeyDown(Key))
		{
			IsKeyDown = true;
			OnKeyDown?.Invoke();
		}
		else if (IsKeyDown && Keyboard.GetState().IsKeyUp(Key))
		{
			IsKeyDown = false;
			OnKeyUp?.Invoke();
		}
	}
}