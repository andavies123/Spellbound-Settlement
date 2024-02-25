using Andavies.MonoGame.Utilities.Test.MockClasses;

namespace Andavies.MonoGame.Utilities.Test;

[TestClass]
public class ChangeTrackerTests
{
	#region AcceptChanges Tests

	[DataTestMethod]
	[DataRow(true)]
	[DataRow(false)]
	public void AcceptChanges_SetsIsChangedToFalse(bool initialIsChangedValue)
	{
		// Arrange
		MockChangeTracker changeTracker = new();
		changeTracker.SetIsChanged(initialIsChangedValue);
		
		// Act
		changeTracker.AcceptChanges();
		
		// Assert
		changeTracker.IsChanged.Should().BeFalse();
	}

	#endregion

	#region SetAndFlagChange Tests

	#region General Overload Tests

	[TestMethod]
	public void SetAndFlagChange_General_SetsIsChangedToTrue_WhenGivingTwoNonEqualValues()
	{
		// Arrange
		MockChangeTracker changeTracker = new();
		
		// Act
		changeTracker.TestInt++;
		
		// Assert
		changeTracker.IsChanged.Should().BeTrue();
	}

	[TestMethod]
	public void SetAndFlagChange_General_UpdatesValue_WhenSettingToNewValue()
	{
		// Arrange
		MockChangeTracker changeTracker = new();
		
		// Act
		changeTracker.TestInt++;
		
		// Assert
		changeTracker.TestInt.Should().Be(1);
	}

	[TestMethod]
	public void SetAndFlagChange_General_DoesNotSetIsChangedToTrue_WhenNewAndOldValueAreEqual()
	{
		// Arrange
		MockChangeTracker changeTracker = new();

		// Act
		changeTracker.TestInt = changeTracker.TestInt;

		// Assert
		changeTracker.IsChanged.Should().BeFalse();
	}

	#endregion
	
	#region 1D Array Overload Tests

	[TestMethod]
	public void SetAndFlagChange_1DArray_SetsIsChangedToTrue_WhenSettingToNewValue()
	{
		// Arrange
		MockChangeTracker changeTracker = new();
		
		// Act
		changeTracker.Set1DArrayValue(changeTracker.Test1DArray[0] + 1, 0);
		
		// Assert
		changeTracker.IsChanged.Should().BeTrue();
	}
	
	[TestMethod]
	public void SetAndFlagChange_1DArray_UpdatesValue_WhenSettingToNewValue()
	{
		// Arrange
		MockChangeTracker changeTracker = new();
		
		// Act
		changeTracker.Set1DArrayValue(changeTracker.Test1DArray[0] + 1, 0);
		
		// Assert
		changeTracker.Test1DArray[0].Should().Be(2);
	}

	[TestMethod]
	public void SetAndFlagChange_1DArray_IsChangedStaysFalse_WhenArrayIndexIsInvalid_AndNewValueIsDifferent()
	{
		// Arrange
		MockChangeTracker changeTracker = new();
		
		// Act
		changeTracker.Set1DArrayValue(changeTracker.Test1DArray[0] + 1, -1);
		
		// Assert
		changeTracker.IsChanged.Should().BeFalse();
	}
	
	[TestMethod]
	public void SetAndFlagChange_1DArray_DoesNotSetIsChangedToTrue_WhenNewAndOldValueAreEqual()
	{
		// Arrange
		MockChangeTracker changeTracker = new();
	
		// Act
		changeTracker.Set1DArrayValue(changeTracker.Test1DArray[0], 0);
	
		// Assert
		changeTracker.IsChanged.Should().BeFalse();
	}

	#endregion
	
	#region 2D Array Overload Tests

	[TestMethod]
	public void SetAndFlagChange_2DArray_SetsIsChangedToTrue_WhenSettingToNewValue()
	{
		// Arrange
		MockChangeTracker changeTracker = new();
		
		// Act
		changeTracker.Set2DArrayValue(changeTracker.Test2DArray[0, 0] + 1, Vector2Int.Zero);
		
		// Assert
		changeTracker.IsChanged.Should().BeTrue();
	}
	
	[TestMethod]
	public void SetAndFlagChange_2DArray_UpdatesValue_WhenSettingToNewValue()
	{
		// Arrange
		MockChangeTracker changeTracker = new();
		
		// Act
		changeTracker.Set2DArrayValue(changeTracker.Test2DArray[0, 0] + 1, Vector2Int.Zero);
		
		// Assert
		changeTracker.Test2DArray[0, 0].Should().Be(2);
	}

	[TestMethod]
	public void SetAndFlagChange_2DArray_IsChangedStaysFalse_WhenArrayIndexIsInvalid_AndNewValueIsDifferent()
	{
		// Arrange
		MockChangeTracker changeTracker = new();
		
		// Act
		changeTracker.Set2DArrayValue(changeTracker.Test2DArray[0, 0] + 1, new Vector2Int(-1));
		
		// Assert
		changeTracker.IsChanged.Should().BeFalse();
	}
	
	[TestMethod]
	public void SetAndFlagChange_2DArray_DoesNotSetIsChangedToTrue_WhenNewAndOldValueAreEqual()
	{
		// Arrange
		MockChangeTracker changeTracker = new();
	
		// Act
		changeTracker.Set2DArrayValue(changeTracker.Test2DArray[0, 0], Vector2Int.Zero);
	
		// Assert
		changeTracker.IsChanged.Should().BeFalse();
	}

	#endregion
	
	#region 3D Array Overload Tests

	[TestMethod]
	public void SetAndFlagChange_3DArray_SetsIsChangedToTrue_WhenSettingToNewValue()
	{
		// Arrange
		MockChangeTracker changeTracker = new();
		
		// Act
		changeTracker.Set3DArrayValue(changeTracker.Test3DArray[0, 0, 0] + 1, Vector3Int.Zero);
		
		// Assert
		changeTracker.IsChanged.Should().BeTrue();
	}
	
	[TestMethod]
	public void SetAndFlagChange_3DArray_UpdatesValue_WhenSettingToNewValue()
	{
		// Arrange
		MockChangeTracker changeTracker = new();
		
		// Act
		changeTracker.Set3DArrayValue(changeTracker.Test3DArray[0, 0, 0] + 1, Vector3Int.Zero);
		
		// Assert
		changeTracker.Test3DArray[0, 0, 0].Should().Be(2);
	}

	[TestMethod]
	public void SetAndFlagChange_3DArray_IsChangedStaysFalse_WhenArrayIndexIsInvalid_AndNewValueIsDifferent()
	{
		// Arrange
		MockChangeTracker changeTracker = new();
		
		// Act
		changeTracker.Set3DArrayValue(changeTracker.Test3DArray[0, 0, 0] + 1, new Vector3Int(-1));
		
		// Assert
		changeTracker.IsChanged.Should().BeFalse();
	}
	
	[TestMethod]
	public void SetAndFlagChange_3DArray_DoesNotSetIsChangedToTrue_WhenNewAndOldValueAreEqual()
	{
		// Arrange
		MockChangeTracker changeTracker = new();
	
		// Act
		changeTracker.Set3DArrayValue(changeTracker.Test3DArray[0, 0, 0], Vector3Int.Zero);
	
		// Assert
		changeTracker.IsChanged.Should().BeFalse();
	}

	#endregion

	#endregion
}