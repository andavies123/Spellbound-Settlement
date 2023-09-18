using System;
using Microsoft.Xna.Framework;

namespace SpellboundSettlement.Global;

public static class GlobalRandom
{
	private static readonly Random Random = new();

	public static Color GetRandomColor() => new
	(
		Random.Next(256),
		Random.Next(256),
		Random.Next(256),
		Random.Next(256)
	);
}