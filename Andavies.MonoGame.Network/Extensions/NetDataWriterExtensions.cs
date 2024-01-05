using Andavies.MonoGame.Utilities;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;

namespace Andavies.MonoGame.Network.Extensions;

public static class NetDataWriterExtensions
{
	public static void Put(this NetDataWriter writer, Vector2 value)
	{
		writer.Put(value.X);
		writer.Put(value.Y);
	}

	public static void Put(this NetDataWriter writer, Vector2Int value)
	{
		writer.Put(value.X);
		writer.Put(value.Y);
	}

	public static void Put(this NetDataWriter writer, Vector3Int value)
	{
		writer.Put(value.X);
		writer.Put(value.Y);
		writer.Put(value.Z);
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

	public static void Put(this NetDataWriter writer, List<Vector2> value)
	{
		writer.Put(value.Count);
		value.ForEach(writer.Put);
	}

	public static void Put(this NetDataWriter writer, List<Vector2Int> value)
	{
		writer.Put(value.Count);
		value.ForEach(writer.Put);
	}
}