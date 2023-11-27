using System;
using Andavies.SpellboundSettlement.Inputs;
using Autofac;
using Microsoft.Xna.Framework;

namespace Andavies.SpellboundSettlement.CameraObjects;

public class WorldViewCameraController : ICameraController
{
	private const int MaxZoomLevels = 9;
	private const int DefaultZoomLevel = 5;
	private static readonly float MinFieldOfView = MathHelper.ToRadians(30);
	private static readonly float MaxFieldOfView = MathHelper.ToRadians(110);
	
	private readonly Camera _camera;
	private readonly GameplayInputState _input;
	
	private bool _moveCamera = true;
	private int _currentZoomLevel = DefaultZoomLevel;
	
	public WorldViewCameraController(Camera camera, GameplayInputState input)
	{
		_camera = camera;
		_input = input;

		_input.MoveCameraStarted += OnMoveCameraStarted;
		_input.MoveCameraStopped += OnMoveCameraStopped;
		_input.RotateCamera90CW.OnKeyDown += OnRotateCameraInput;
		_input.ZoomIn += OnZoomInInput;
		_input.ZoomOut += OnZoomOutInput;
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
		
		_camera.FieldOfView = CalculateFieldOfView();
		_camera.AspectRatio = Program.Container.Resolve<Game>().GraphicsDevice.Viewport.AspectRatio;
		_camera.NearClippingPlane = 0.1f;
		_camera.FarClippingPlane = 1000f;
		
		SetZoomLevel(DefaultZoomLevel, recalculateMatrices: false);
		
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
		
		// Normalize only if the vector isn't zero as it has trouble normalizing a 0 vector
		if (movementVector != Vector3.Zero)
			movementVector.Normalize();

		// Apply delta time and movement speed after normalization or else it would always have length of 1
		_camera.Position += movementVector * deltaTime * MovementSpeed;
		
		Vector3 forward = new(
			(float) (Math.Sin(_camera.Yaw) * Math.Cos(_camera.Pitch)),
			(float) Math.Sin(_camera.Pitch),
			(float) (Math.Cos(_camera.Yaw) * Math.Cos(_camera.Pitch))
		);
		
		forward.Normalize();
		_camera.Target = _camera.Position + forward;
		
		_camera.RecalculateViewMatrix(recalculateWvpMatrix: true);
	}

	private void SetZoomLevel(int newZoomLevel, bool recalculateMatrices = true)
	{
		_currentZoomLevel = Math.Clamp(newZoomLevel, 1, MaxZoomLevels);
		_camera.FieldOfView = CalculateFieldOfView();
		if (recalculateMatrices)
			_camera.RecalculateProjectionMatrix(recalculateWvpMatrix: true);
	}

	private float CalculateFieldOfView() => (MaxFieldOfView - MinFieldOfView) / MaxZoomLevels * _currentZoomLevel;

	private void OnMoveCameraStarted() => _moveCamera = true;
	private void OnMoveCameraStopped() => _moveCamera = false;
	private void OnZoomInInput() => SetZoomLevel(_currentZoomLevel - 1);
	private void OnZoomOutInput() => SetZoomLevel(_currentZoomLevel + 1);

	private void OnRotateCameraInput()
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