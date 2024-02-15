using Microsoft.Xna.Framework;

namespace Andavies.SpellboundSettlement.CameraObjects;

public interface ICameraController : IUpdateable
{
	/// <summary>
	/// How fast the camera moves
	/// </summary>
	float MovementSpeed { get; set; }

	/// <summary>
	/// Resets all the necessary camera properties to defaults
	/// </summary>
	void ResetCamera();
}