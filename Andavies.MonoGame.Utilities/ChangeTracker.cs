using System.ComponentModel;

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
		if (!Equals(newValue, oldValue))
		{
			oldValue = newValue;
			IsChanged = true;
		}
	}
	
	protected void SetAndFlagChange<T>(T newValue, T[,,] array, Vector3Int arrayIndex)
	{
		if (arrayIndex.IsValidArrayIndex(array))
		{
			array[arrayIndex.X, arrayIndex.Y, arrayIndex.Z] = newValue;
			IsChanged = true;
		}
	}
}