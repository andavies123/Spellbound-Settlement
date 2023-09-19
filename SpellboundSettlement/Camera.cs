using Microsoft.Xna.Framework;

namespace SpellboundSettlement;

public class Camera
{
	private Matrix _worldMatrix;
	private Matrix _viewMatrix;
	private Matrix _projectionMatrix;

	public Camera(float aspectRatio)
	{
		AspectRatio = aspectRatio;
		RecalculateWorldMatrix(recalculateWvpMatrix: false);
		RecalculateViewMatrix(recalculateWvpMatrix: false);
		RecalculateProjectionMatrix(recalculateWvpMatrix: false);
		RecalculateWorldViewProjectionMatrix();
	}
	
	// World Matrix Properties
	public Vector3 WorldPosition { get; set; } = Vector3.Zero;
	
	// Camera Matrix Properties
	public Vector3 CameraPosition { get; set; } = new(0, 30, 20);
	public Vector3 CameraTarget { get; set; } = Vector3.Zero;
	public Vector3 CameraUp { get; set; } = Vector3.Up;
	
	// Projection Matrix Properties
	public float FieldOfView { get; set; } = MathHelper.PiOver4;
	public float AspectRatio { get; set; }
	public float NearClippingPlane { get; set; } = .01f;
	public float FarClippingPlane { get; set; } = 100f;
	
	public Matrix WorldViewProjection { get; set; }

	public void RecalculateWorldMatrix(bool recalculateWvpMatrix = true)
	{
		_worldMatrix = Matrix.CreateTranslation(WorldPosition);
		if (recalculateWvpMatrix)
			RecalculateWorldViewProjectionMatrix();
	}

	public void RecalculateViewMatrix(bool recalculateWvpMatrix = true)
	{
		_viewMatrix = Matrix.CreateLookAt(CameraPosition, CameraTarget, CameraUp);
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