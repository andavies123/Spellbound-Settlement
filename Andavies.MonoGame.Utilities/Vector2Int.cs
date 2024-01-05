using Microsoft.Xna.Framework;

namespace Andavies.MonoGame.Utilities;

public readonly struct Vector2Int
{
	public static readonly Vector2Int Zero = new(0);
	public static readonly Vector2Int One = new(1);
	
	public readonly int X;
	public readonly int Y;
	
	public Vector2Int(int x, int y)
	{
		X = x;
		Y = y;
	}

	public Vector2Int(int value) : this(value, value) { }

	public Vector3Int ToVector3IntNoY(int y = 0)
	{
		return new Vector3Int(X, y, Y);
	}
	
	public static Vector2Int operator +(Vector2Int a, Vector2Int b)
	{
		return new Vector2Int(a.X + b.X, a.Y + b.Y);
	}

	public static Vector2Int operator -(Vector2Int a, Vector2Int b)
	{
		return new Vector2Int(a.X - b.X, a.Y - b.Y);
	}

	public static Vector2Int operator *(Vector2Int a, int scale)
	{
		return new Vector2Int(a.X * scale, a.Y * scale);
	}

	public static bool operator ==(Vector2Int left, Vector2Int right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(Vector2Int left, Vector2Int right)
	{
		return !(left == right);
	}

	public override bool Equals(object? obj)
	{
		if (obj is not Vector2Int other)
			return false;

		return X == other.X && Y == other.Y;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(X, Y);
	}

	public override string ToString()
	{
		return $"({X}, {Y})";
	}
	
	public static explicit operator Vector2(Vector2Int vector)
	{
		return new Vector2(vector.X, vector.Y);
	}
}