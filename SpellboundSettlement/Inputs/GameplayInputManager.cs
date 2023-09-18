using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpellboundSettlement.Global;

namespace SpellboundSettlement.Inputs;

public class GameplayInputManager
{
	private const Keys MoveCameraForwardsKey = Keys.W;
	private const Keys MoveCameraBackwardsKey = Keys.S;
	private const Keys MoveCameraLeftKey = Keys.A;
	private const Keys MoveCameraRightKey = Keys.D;

	private const Keys PauseGameKey = Keys.Escape;

	private bool _isMoveCameraActive = false;
	private int _previousScrollWheelValue = Mouse.GetState().ScrollWheelValue;

	public Vector2 MoveCameraInput { get; private set; }
	
	public event Action MoveCameraInputStarted;
	public event Action MoveCameraInputStopped;
	public event Action ZoomIn;
	public event Action ZoomOut;
	public event Action PauseGame;
	
	public void UpdateInput()
	{
		CheckForceQuit();
		CheckPauseGameInput();
		CheckCameraMoveInput();
		CheckZoomInput();
	}

	private void CheckForceQuit()
	{
		if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Keyboard.GetState().IsKeyDown(Keys.Escape))
			GameServices.GetService<GameManager>().Exit();
	}

	private void CheckPauseGameInput()
	{
		if (Keyboard.GetState().IsKeyDown(PauseGameKey))
			PauseGame?.Invoke();
	}

	private void CheckCameraMoveInput()
	{
		float verticalMovement = 0f;
		float horizontalMovement = 0f;

		if (Keyboard.GetState().IsKeyDown(MoveCameraForwardsKey))
			verticalMovement += 1f;
		if (Keyboard.GetState().IsKeyDown(MoveCameraBackwardsKey))
			verticalMovement -= 1f;
		if (Keyboard.GetState().IsKeyDown(MoveCameraRightKey))
			horizontalMovement += 1f;
		if (Keyboard.GetState().IsKeyDown(MoveCameraLeftKey))
			horizontalMovement -= 1f;

		MoveCameraInput = new Vector2(horizontalMovement, verticalMovement);
		if (!_isMoveCameraActive && MoveCameraInput != Vector2.Zero)
		{
			_isMoveCameraActive = true;
			MoveCameraInputStarted?.Invoke();
		}
		else if (_isMoveCameraActive && MoveCameraInput == Vector2.Zero)
		{
			_isMoveCameraActive = false;
			MoveCameraInputStopped?.Invoke();
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