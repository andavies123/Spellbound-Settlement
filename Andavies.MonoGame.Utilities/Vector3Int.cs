using Microsoft.Xna.Framework;

namespace Andavies.MonoGame.Utilities;

public readonly struct Vector3Int
{
	public static readonly Vector3Int Zero = new(0);
	public static readonly Vector3Int One = new(1);
	public static readonly Vector3Int East = new(1, 0, 0);
	public static readonly Vector3Int West = new(-1, 0, 0);
	public static readonly Vector3Int Up = new(0, 1, 0);
	public static readonly Vector3Int Down = new(0, -1, 0);
	public static readonly Vector3Int South = new(0, 0, 1);
	public static readonly Vector3Int North = new(0, 0, -1);
	
	public readonly int X;
	public readonly int Y;
	public readonly int Z;

	public Vector3Int(int x, int y, int z)
	{
		X = x;
		Y = y;
		Z = z;
	}

	public Vector3Int(float x, float y, float z)
	{
		X = (int) x;
		Y = (int) y;
		Z = (int) z;
	}

	public Vector3Int(int value) : this(value, value, value) { }

	public Vector3Int(Vector3 vector3) : this(vector3.X, vector3.Y, vector3.Z) { }

	public void Deconstruct(out int x, out int y, out int z)
	{
		x = X;
		y = Y;
		z = Z;
	}

	public bool IsValidArrayIndex<T>(T[,,] array)
	{
		return X >= 0 && X < array.GetLength(0) &&
		       Y >= 0 && Y < array.GetLength(1) &&
		       Z >= 0 && Z < array.GetLength(2);
	}

	public float Distance(Vector3Int other) => Distance(this, other);

	public static float Distance(Vector3Int a, Vector3Int b)
	{
		return MathF.Sqrt(MathF.Pow(a.X - b.X, 2) + MathF.Pow(a.Y - b.Y, 2) + MathF.Pow(a.Z - b.Z, 2));
	}

	public static Vector3Int operator +(Vector3Int a, Vector3Int b)
	{
		return new Vector3Int(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
	}

	public static Vector3 operator +(Vector3Int a, Vector3 b)
	{
		return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
	}

	public static Vector3 operator +(Vector3 a, Vector3Int b)
	{
		return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
	}

	public static Vector3Int operator -(Vector3Int a, Vector3Int b)
	{
		return new Vector3Int(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
	}

	public static Vector3 operator -(Vector3Int a, Vector3 b)
	{
		return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
	}

	public static Vector3 operator -(Vector3 a, Vector3Int b)
	{
		return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
	}

	public static Vector3Int operator *(Vector3Int a, Vector3Int b)
	{
		return new Vector3Int(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
	}

	public static Vector3 operator *(Vector3Int a, Vector3 b)
	{
		return new Vector3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
	}

	public static Vector3 operator *(Vector3 a, Vector3Int b)
	{
		return new Vector3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
	}

	public static Vector3Int operator *(Vector3Int a, int scale)
	{
		return new Vector3Int(a.X * scale, a.Y * scale, a.Z * scale);
	}

	public static Vector3 operator *(Vector3Int a, float scale)
	{
		return new Vector3(a.X * scale, a.Y * scale, a.Z * scale);
	}

	public static Vector3 operator /(Vector3Int a, Vector3 b)
	{
		if (b.X == 0 || b.Y == 0 || b.Z == 0)
			throw new DivideByZeroException();
		
		return new Vector3(a.X / b.X, a.Y / b.Y, a.Z / b.Z);	
	}

	public static Vector3 operator /(Vector3 a, Vector3Int b)
	{
		if (b.X == 0 || b.Y == 0 || b.Z == 0)
			throw new DivideByZeroException();
		
		return new Vector3(a.X / b.X, a.Y / b.Y, a.Z / b.Z);	
	}

	public static Vector3 operator /(Vector3Int a, Vector3Int b)
	{
		return a / (Vector3) b;
	}
	
	public static Vector3 operator /(Vector3Int a, float scale)
	{
		if (scale == 0f)
			throw new DivideByZeroException();
		
		return new Vector3(a.X / scale, a.Y / scale, a.Z / scale);
	}

	public static Vector3 operator /(Vector3Int a, int scale)
	{
		return a / (float) scale;
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

	public static explicit operator Vector3Int(Vector3 vector)
	{
		return new Vector3Int(vector);
	}
}