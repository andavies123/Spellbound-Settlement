using Microsoft.Xna.Framework;

namespace Andavies.SpellboundSettlement.GameWorld;

public enum Rotation
{
	Zero,
	Ninety,
	OneHundredEighty,
	TwoHundredSeventy
}

public static class RotationExtensions
{
	public static float ToRadians(this Rotation rotation)
	{
		return rotation switch
		{
			Rotation.Zero => 0f,
			Rotation.Ninety => MathHelper.PiOver2,
			Rotation.OneHundredEighty => MathHelper.Pi,
			Rotation.TwoHundredSeventy => MathHelper.PiOver2 * 3,
			_ => throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null)
		};
	}
}