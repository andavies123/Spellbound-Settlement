using System;
using Microsoft.Xna.Framework;
using SpellboundSettlement.Noise;

namespace SpellboundSettlement.Globals;

public static class GlobalRandom
{
	public static readonly Random Random = new();
	public static readonly FastNoiseLite Noise = new();

	public static Color GetRandomColor() => new
	(
		Random.Next(256),
		Random.Next(256),
		Random.Next(256),
		Random.Next(256)
	);

	public static float GetPerlinNoise(int seed, float scale, (float x, float z) offset)
	{
		Noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
		Noise.SetSeed(seed);
		Noise.SetFrequency(scale);
		return Noise.GetNoise(offset.x, offset.z);
	}
}