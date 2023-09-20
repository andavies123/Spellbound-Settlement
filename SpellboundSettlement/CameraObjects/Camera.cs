using Microsoft.Xna.Framework;

namespace SpellboundSettlement.CameraObjects;

public class Camera
{
	private Matrix _worldMatrix;
	private Matrix _viewMatrix;
	private Matrix _projectionMatrix;
	
	// World Matrix Properties
	public Vector3 WorldPosition { get; set; }
	
	// Camera Matrix Properties
	public Vector3 Position { get; set; }
	public Vector3 Target { get; set; }
	public Vector3 Up { get; set; }
	
	// Camera Rotation Matrix
	public float Yaw { get; set; } // Angle around Y axis in radians | 0° = facing -z | 90° = facing x | -90° = facing -x
	public float Pitch { get; set; } // Vertical angle in radians | 0° = straight forward | 90° = straight up | -90° = straight down
	public float Roll { get; set; } // Horizontal tilt in radians | 0° = leveled | 90° = to the right | -90° = to the left
	public Matrix CameraRotation => Matrix.CreateFromYawPitchRoll(Yaw, Pitch, Roll);
	public Matrix CameraRotationNoPitch => Matrix.CreateFromYawPitchRoll(Yaw, 0, Roll);
	
	// Projection Matrix Properties
	public float FieldOfView { get; set; }
	public float AspectRatio { get; set; }
	public float NearClippingPlane { get; set; }
	public float FarClippingPlane { get; set; }
	
	// Combined Matrix
	public Matrix WorldViewProjection { get; set; }

	public void RecalculateWorldMatrix(bool recalculateWvpMatrix = true)
	{
		_worldMatrix = Matrix.CreateTranslation(WorldPosition);
		if (recalculateWvpMatrix)
			RecalculateWorldViewProjectionMatrix();
	}

	public void RecalculateViewMatrix(bool recalculateWvpMatrix = true)
	{
		_viewMatrix = Matrix.CreateLookAt(Position, Target, Up);
		if (recalculateWvpMatrix) 
			RecalculateWorldViewProjectionMatrix();
	}

	public void RecalculateProjectionMatrix(bool recalculateWvpMatrix = true)
	{
		_projectionMatrix = Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, NearClippingPlane, FarClippingPlane);
		if (recalculateWvpMatrix) 
			RecalculateWorldViewProjectionMatrix();
	}

	public void RecalculateWorldViewProjectionMatrix() => WorldViewProjection = _worldMatrix * _viewMatrix * _projectionMatrix;
}