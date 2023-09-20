using System;
using Microsoft.Xna.Framework;
using SpellboundSettlement.Global;
using SpellboundSettlement.Inputs;

namespace SpellboundSettlement.CameraObjects;

public class WorldViewCameraController : ICameraController
{
	private readonly Camera _camera;
	private readonly GameplayInputManager _input;

	private bool _moveCamera = true;
	
	public WorldViewCameraController(Camera camera, GameplayInputManager input)
	{
		_camera = camera;
		_input = input;

		_input.MoveCameraStarted += OnMoveCameraStarted;
		_input.MoveCameraStopped += OnMoveCameraStopped;
		_input.RotateCamera += OnRotateCameraInputPressed;
	}

	public float MovementSpeed { get; set; } = 20;

	public void ResetCamera()
	{
		_camera.WorldPosition = Vector3.Zero;
		
		_camera.Position = new Vector3(0, 50, 0);
		_camera.Target = Vector3.Zero;
		_camera.Up = Vector3.Up;
		_camera.Yaw = MathHelper.ToRadians(180);
		_camera.Pitch = MathHelper.ToRadians(-60);
		_camera.Roll = MathHelper.ToRadians(0);
		
		_camera.FieldOfView = MathHelper.PiOver4;
		_camera.AspectRatio = GameServices.GetService<Game>().GraphicsDevice.Viewport.AspectRatio;
		_camera.NearClippingPlane = 0.1f;
		_camera.FarClippingPlane = 1000f;
		
		_camera.RecalculateWorldMatrix(recalculateWvpMatrix: false);
		_camera.RecalculateViewMatrix(recalculateWvpMatrix: false);
		_camera.RecalculateProjectionMatrix(recalculateWvpMatrix: false);
		_camera.RecalculateWorldViewProjectionMatrix();
	}

	public void UpdateCamera(float deltaTime)
	{
		if (!_moveCamera)
			return;

		Vector2 moveCameraInput = _input.MoveCameraInput;
		
		Vector3 cameraForward = _camera.CameraRotationNoPitch.Backward * moveCameraInput.Y;
		Vector3 cameraHorizontal = _camera.CameraRotation.Right * moveCameraInput.X;

		Vector3 movementVector = cameraForward + cameraHorizontal;
		
		if (movementVector != Vector3.Zero)
			movementVector.Normalize();
		
		_camera.Position += movementVector * deltaTime * MovementSpeed;
		
		Vector3 forward = new(
			(float) (Math.Sin(_camera.Yaw) * Math.Cos(_camera.Pitch)),
			(float) Math.Sin(_camera.Pitch),
			(float) (Math.Cos(_camera.Yaw) * Math.Cos(_camera.Pitch))
		);
		
		forward.Normalize();
		_camera.Target = _camera.Position + forward;
		
		_camera.RecalculateViewMatrix();
	}

	private void OnMoveCameraStarted() => _moveCamera = true;
	private void OnMoveCameraStopped() => _moveCamera = false;

	private void OnRotateCameraInputPressed()
	{
		_camera.Yaw += MathHelper.ToRadians(90);
		_camera.Yaw %= MathHelper.TwoPi;
		
		Vector3 forward = new(
			(float) (Math.Sin(_camera.Yaw) * Math.Cos(_camera.Pitch)),
			(float) Math.Sin(_camera.Pitch),
			(float) (Math.Cos(_camera.Yaw) * Math.Cos(_camera.Pitch))
		);
		
		forward.Normalize();
		_camera.Target = _camera.Position + forward;
		_camera.RecalculateViewMatrix();
	}
}

public interface ICameraController
{
	/// <summary>
	/// How fast the camera moves
	/// </summary>
	float MovementSpeed { get; set; }

	/// <summary>
	/// Resets all the necessary camera properties to defaults
	/// </summary>
	void ResetCamera();
	
	/// <summary>
	/// Call this every frame to update the camera's values
	/// </summary>
	/// <param name="deltaTime">The time in seconds that has passed since the last frame</param>
	void UpdateCamera(float deltaTime);
}