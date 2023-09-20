namespace SpellboundSettlement.CameraObjects;

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