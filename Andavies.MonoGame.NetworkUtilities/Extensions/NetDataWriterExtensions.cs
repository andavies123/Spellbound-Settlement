using LiteNetLib.Utils;
using Microsoft.Xna.Framework;

namespace Andavies.MonoGame.NetworkUtilities.Extensions;

public static class NetDataWriterExtensions
{
	public static void Put(this NetDataWriter writer, Vector2 value)
	{
		writer.Put(value.X);
		writer.Put(value.Y);
	}

	public static void Put(this NetDataWriter writer, (int x, int y, int z) value)
	{
		writer.Put(value.x);
		writer.Put(value.y);
		writer.Put(value.z);
	}

	public static void Put(this NetDataWriter writer, int[,,] value)
	{
		for (int x = 0; x < value.GetLength(0); x++)
		{
			for (int y = 0; y < value.GetLength(1); y++)
			{
				for (int z = 0; z < value.GetLength(2); z++)
				{
					writer.Put(value[x, y, z]);
				}
			}
		}
	}
}