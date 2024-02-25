namespace Andavies.MonoGame.Utilities.Test.MockClasses;

public class MockChangeTracker : ChangeTracker
{
	private int _testInt = 0;

	public int TestInt
	{
		get => _testInt;
		set => SetAndFlagChange(value, ref _testInt);
	}

	public int[] Test1DArray { get; } = {1, 2, 3, 4, 5};
	public int[,] Test2DArray { get; } = {{1, 2, 3, 4}, {1, 2, 3, 4}};
	public int[,,] Test3DArray { get; } = {{{1, 2, 3}, {1, 2, 3}, {1, 2, 3}}, {{1, 2, 3}, {1, 2, 3}, {1, 2, 3}}, {{1, 2, 3}, {1, 2, 3}, {1, 2, 3}}};

	public void Set1DArrayValue(int value, int index) => SetAndFlagChange(value, Test1DArray, index);
	public void Set2DArrayValue(int value, Vector2Int index) => SetAndFlagChange(value, Test2DArray, index);
	public void Set3DArrayValue(int value, Vector3Int index) => SetAndFlagChange(value, Test3DArray, index);
    
	public void SetIsChanged(bool value) => IsChanged = value;
}