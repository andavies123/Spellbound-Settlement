using Andavies.MonoGame.Utilities.Extensions;

namespace Andavies.MonoGame.Utilities.Test.Extensions;

[TestClass]
public class IntExtensionsTests
{
	#region IsValidArrayIndex Tests

	[TestMethod]
	public void IsValidArrayIndex_ReturnsTrue_WhenValueIsZeroAndArrayIsNotEmpty()
	{
		// Arrange
		const int index = 0;
		int[] array = {1, 2, 3};
		
		// Act
		bool isValidArrayIndex = index.IsValidArrayIndex(array);
		
		// Assert
		isValidArrayIndex.Should().BeTrue();
	}

	[TestMethod]
	public void IsValidArrayIndex_ReturnsTrue_WhenValueIsPositiveAndLessThanArraySize()
	{
		// Arrange
		const int index = 2;
		int[] array = {1, 2, 3};
		
		// Act
		bool isValidArrayIndex = index.IsValidArrayIndex(array);
		
		// Assert
		isValidArrayIndex.Should().BeTrue();
	}

	[TestMethod]
	public void IsValidArrayIndex_ReturnsFalse_WhenValueIsLessThanZero()
	{
		// Arrange
		const int index = -1;
		int[] array = {1, 2, 3};
		
		// Act
		bool isValidArrayIndex = index.IsValidArrayIndex(array);

		// Assert
		isValidArrayIndex.Should().BeFalse(); 
	}

	[TestMethod]
	public void IsValidArrayIndex_ReturnsFalse_WhenValueIsEqualToArraySize()
	{
		// Arrange
		const int index = 3;
		int[] array = {1, 2, 3};
		
		// Act
		bool isValidArrayIndex = index.IsValidArrayIndex(array);
		
		// Assert
		isValidArrayIndex.Should().BeFalse();
	}

	[TestMethod]
	public void IsValidArrayIndex_ReturnsFalse_WhenValueIsGreaterThanArraySize()
	{
		// Arrange
		const int index = 5;
		int[] array = {1, 2, 3};

		// Act
		bool isValidArrayIndex = index.IsValidArrayIndex(array);

		// Assert
		isValidArrayIndex.Should().BeFalse();
	}

	[TestMethod]
	public void IsValidArrayIndex_ReturnsFalse_WhenValueIsZeroAndArrayIsEmpty()
	{
		// Arrange
		const int index = 0;
		int[] array = Array.Empty<int>();
		
		// Act
		bool isValidArrayIndex = index.IsValidArrayIndex(array);
		
		// Assert
		isValidArrayIndex.Should().BeFalse();
	}

	#endregion
}