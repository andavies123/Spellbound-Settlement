using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.SpellboundSettlement.CameraObjects;

public class Camera
{
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
	public Matrix WorldMatrix { get; private set; }
	public Matrix ViewMatrix { get; private set; }
	public Matrix ProjectionMatrix { get; private set; }

	public void RecalculateWorldMatrix()
	{
		WorldMatrix = Matrix.CreateTranslation(WorldPosition);
	}

	public void RecalculateViewMatrix()
	{
		ViewMatrix = Matrix.CreateLookAt(Position, Target, Up);
	}

	public void RecalculateProjectionMatrix()
	{
		ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, NearClippingPlane, FarClippingPlane);
	}
	
	public Ray GetRayFromCamera(Viewport viewport, Vector2 screenPoint, float distance)
	{
		Vector3 nearPoint = new(screenPoint, 0);
		Vector3 farPoint = new(screenPoint, 1);

		Vector3 nearWorld = viewport.Unproject(nearPoint, ProjectionMatrix, ViewMatrix, Matrix.Identity);
		Vector3 farWorld = viewport.Unproject(farPoint, ProjectionMatrix, ViewMatrix, Matrix.Identity);

		Vector3 direction = Vector3.Normalize(farWorld - nearWorld);
		return new Ray(nearWorld, direction * distance);
	}
}