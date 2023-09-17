using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpellboundSettlement.Inputs;

public class GameplayInputManager
{
	private const Keys MoveCameraForwardsKey = Keys.W;
	private const Keys MoveCameraBackwardsKey = Keys.S;
	private const Keys MoveCameraLeftKey = Keys.A;
	private const Keys MoveCameraRightKey = Keys.D;

	private const Keys PauseGameKey = Keys.Escape;

	private bool _isMoveCameraActive = false;

	public Vector2 MoveCameraInput { get; private set; }
	
	public event Action MoveCameraInputStarted;
	public event Action MoveCameraInputStopped;
	public event Action PauseGameInputPressed;
	
	public void UpdateInput()
	{
		CheckPauseGameInput();
		CheckCameraMoveInput();
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

	private void CheckPauseGameInput()
	{
		if (Keyboard.GetState().IsKeyDown(PauseGameKey))
			PauseGameInputPressed?.Invoke();
	}
}