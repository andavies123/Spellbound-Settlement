using System.ComponentModel;
using Andavies.MonoGame.Utilities.Extensions;

namespace Andavies.MonoGame.Utilities;

public abstract class ChangeTracker : IChangeTracking
{
	public bool IsChanged { get; protected set; }

	public void AcceptChanges()
	{
		IsChanged = false;
	}

	protected void SetAndFlagChange<T>(T newValue, ref T oldValue)
	{
		if (Equals(newValue, oldValue))
			return;
		
		oldValue = newValue;
		IsChanged = true;
	}

	protected void SetAndFlagChange<T>(T newValue, T[] array, int arrayIndex)
	{
		if (!arrayIndex.IsValidArrayIndex(array))
			return;

		if (Equals(array[arrayIndex]))
			return;
		
		array[arrayIndex] = newValue;
		IsChanged = true;
	}

	protected void SetAndFlagChange<T>(T newValue, T[,] array, Vector2Int arrayIndex)
	{
		if (!arrayIndex.IsValidArrayIndex(array))
			return;

		if (Equals(array[arrayIndex.X, arrayIndex.Y]))
			return;
		
		array[arrayIndex.X, arrayIndex.Y] = newValue;
		IsChanged = true;
	}
	
	protected void SetAndFlagChange<T>(T newValue, T[,,] array, Vector3Int arrayIndex)
	{
		if (!arrayIndex.IsValidArrayIndex(array))
			return;

		if (Equals(array[arrayIndex.X, arrayIndex.Y, arrayIndex.Z]))
			return;
		
		array[arrayIndex.X, arrayIndex.Y, arrayIndex.Z] = newValue;
		IsChanged = true;
	}
}