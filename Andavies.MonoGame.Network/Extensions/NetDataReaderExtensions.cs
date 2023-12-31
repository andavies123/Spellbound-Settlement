using Andavies.MonoGame.Utilities;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;

namespace Andavies.MonoGame.Network.Extensions;

public static class NetDataReaderExtensions
{
	public static Vector2 GetVector2(this NetDataReader reader)
	{
		return new Vector2(reader.GetFloat(), reader.GetFloat());
	}

	public static Vector2Int GetVector2Int(this NetDataReader reader)
	{
		return new Vector2Int(reader.GetInt(), reader.GetInt());
	}

	public static Vector3Int GetVector3Int(this NetDataReader reader)
	{
		return new Vector3Int(reader.GetInt(), reader.GetInt(), reader.GetInt());
	}

	public static (int, int, int) GetIntTuple3(this NetDataReader reader)
	{
		return (reader.GetInt(), reader.GetInt(), reader.GetInt());
	}

	public static int[,,] GetInt3DArray(this NetDataReader reader, int length1, int length2, int length3)
	{
		int[,,] array = new int[length1, length2, length3];
		
		for (int x = 0; x < length1; x++)
		{
			for (int y = 0; y < length2; y++)
			{
				for (int z = 0; z < length3; z++)
				{
					array[x, y, z] = reader.GetInt();
				}
			}
		}
		
		return array;
	}

	public static List<Vector2> GetVector2List(this NetDataReader reader)
	{
		int length = reader.GetInt();
		List<Vector2> list = new();

		for (int i = 0; i < length; i++)
		{
			list.Add(reader.GetVector2());
		}

		return list;
	}

	public static List<Vector2Int> GetVector2IntList(this NetDataReader reader)
	{
		int length = reader.GetInt();
		List<Vector2Int> list = new();
		
		for (int i = 0; i < length; i++)
		{
			list.Add(reader.GetVector2Int());
		}

		return list;
	}
}