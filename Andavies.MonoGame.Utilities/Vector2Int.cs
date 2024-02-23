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

	public Vector2Int(float x, float y)
	{
		X = (int) x;
		Y = (int) y;
	}

	public Vector2Int(int value) : this(value, value) { }

	public Vector2Int(Vector2 vector) : this(vector.X, vector.Y) { }

	public Vector3Int ToVector3IntNoY(int y = 0)
	{
		return new Vector3Int(X, y, Y);
	}

	public bool TryGetFromArray<T>(T[,] array, out T? value)
	{
		value = default;
		
		if (IsValidArrayIndex(array))
		{
			value = array[X, Y];
			return true;
		}

		return false;
	}

	public bool IsValidArrayIndex<T>(T[,] array)
	{
		return X >= 0 && X < array.GetLength(0) &&
		       Y >= 0 && Y < array.GetLength(1);
	}
	
	public float Distance(Vector2Int other) => Distance(this, other);

	public static float Distance(Vector2Int a, Vector2Int b)
	{
		return MathF.Sqrt(MathF.Pow(a.X - b.X, 2) + MathF.Pow(a.Y - b.Y, 2));
	}
	
	public static Vector2Int operator +(Vector2Int a, Vector2Int b)
	{
		return new Vector2Int(a.X + b.X, a.Y + b.Y);
	}

	public static Vector2 operator +(Vector2Int a, Vector2 b)
	{
		return new Vector2(a.X + b.X, a.Y + b.Y);
	}

	public static Vector2 operator +(Vector2 a, Vector2Int b)
	{
		return new Vector2(a.X + b.X, a.Y + b.Y);
	}

	public static Vector2Int operator -(Vector2Int a, Vector2Int b)
	{
		return new Vector2Int(a.X - b.X, a.Y - b.Y);
	}

	public static Vector2 operator -(Vector2Int a, Vector2 b)
	{
		return new Vector2(a.X - b.X, a.Y - b.Y);
	}

	public static Vector2 operator -(Vector2 a, Vector2Int b)
	{
		return new Vector2(a.X - b.X, a.Y - b.Y);
	}

	public static Vector2Int operator *(Vector2Int a, Vector2Int b)
	{
		return new Vector2Int(a.X * b.X, a.Y * b.Y);
	}

	public static Vector2 operator *(Vector2Int a, Vector2 b)
	{
		return new Vector2(a.X * b.X, a.Y * b.Y);
	}

	public static Vector2 operator *(Vector2 a, Vector2Int b)
	{
		return new Vector2(a.X * b.X, a.Y * b.Y);
	}

	public static Vector2Int operator *(Vector2Int a, int scale)
	{
		return new Vector2Int(a.X * scale, a.Y * scale);
	}

	public static Vector2 operator *(Vector2Int a, float scale)
	{
		return new Vector2(a.X * scale, a.Y * scale);
	}

	public static Vector2 operator /(Vector2Int a, Vector2 b)
	{
		if (b.X == 0 || b.Y == 0)
			throw new DivideByZeroException();
		
		return new Vector2(a.X / b.X, a.Y / b.Y);	
	}

	public static Vector2 operator /(Vector2 a, Vector2Int b)
	{
		if (b.X == 0 || b.Y == 0)
			throw new DivideByZeroException();
		
		return new Vector2(a.X / b.X, a.Y / b.Y);	
	}

	public static Vector2 operator /(Vector2Int a, Vector2Int b)
	{
		return a / (Vector2) b;
	}
	
	public static Vector2 operator /(Vector2Int a, float scale)
	{
		if (scale == 0f)
			throw new DivideByZeroException();
		
		return new Vector2(a.X / scale, a.Y / scale);
	}

	public static Vector2 operator /(Vector2Int a, int scale)
	{
		return a / (float) scale;
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

	public static explicit operator Vector2Int(Vector2 vector)
	{
		return new Vector2Int(vector);
	}
}