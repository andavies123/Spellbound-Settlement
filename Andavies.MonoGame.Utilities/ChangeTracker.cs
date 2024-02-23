using System.ComponentModel;

namespace Andavies.MonoGame.Utilities;

public abstract class ChangeTracker : IChangeTracking
{
	public bool IsChanged { get; private set; }

	public void AcceptChanges()
	{
		IsChanged = false;
	}

	protected void SetAndFlagChanged<T>(T newValue, ref T oldValue)
	{
		if (!Equals(newValue, oldValue))
		{
			oldValue = newValue;
			IsChanged = true;
		}
	}
	
	protected void SetAndFlagChanged<T>(T newValue, T[,,] array, Vector3Int arrayIndex)
	{
		if (arrayIndex.IsValidArrayIndex(array))
		{
			array[arrayIndex.X, arrayIndex.Y, arrayIndex.Z] = newValue;
			IsChanged = true;
		}
	}
}