using Andavies.MonoGame.Utilities.Noise;
using Microsoft.Xna.Framework;

namespace Andavies.MonoGame.Utilities;

public static class RandomUtility
{
	public static readonly Random Random = new();
	public static readonly FastNoiseLite Noise = new();
	
	public static Color GetRandomColor(bool randomizeAlpha) => new (
		Random.Next(256), 
		Random.Next(256), 
		Random.Next(256),
		randomizeAlpha ? Random.Next(256) : 255);

	public static float GetPerlinNoise(int seed, float scale, (float x, float z) offset)
	{
		Noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
		Noise.SetSeed(seed);
		Noise.SetFrequency(scale);
		return Noise.GetNoise(offset.x, offset.z);
	}
}