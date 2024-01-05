using Microsoft.Xna.Framework;

namespace Andavies.MonoGame.Utilities;

public readonly struct Vector3Int
{
	public static readonly Vector3Int Zero = new(0);
	public static readonly Vector3Int One = new(1);
	
	public readonly int X;
	public readonly int Y;
	public readonly int Z;

	public Vector3Int(int x, int y, int z)
	{
		X = x;
		Y = y;
		Z = z;
	}

	public Vector3Int(int value) : this(value, value, value) { }

	public static Vector3Int operator +(Vector3Int a, Vector3Int b)
	{
		return new Vector3Int(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
	}

	public static Vector3 operator +(Vector3Int a, Vector3 b)
	{
		return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
	}

	public static Vector3Int operator -(Vector3Int a, Vector3Int b)
	{
		return new Vector3Int(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
	}

	public static Vector3Int operator *(Vector3Int a, int scale)
	{
		return new Vector3Int(a.X * scale, a.Y * scale, a.Z * scale);
	}

	public static bool operator ==(Vector3Int left, Vector3Int right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(Vector3Int left, Vector3Int right)
	{
		return !(left == right);
	}

	public override bool Equals(object? obj)
	{
		if (obj is not Vector3Int other)
			return false;

		return X == other.X && Y == other.Y && Z == other.Z;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(X, Y, Z);
	}

	public override string ToString()
	{
		return $"({X}, {Y}, {Z})";
	}
	
	public static explicit operator Vector3(Vector3Int vector)
	{
		return new Vector3(vector.X, vector.Y, vector.Z);
	}
}