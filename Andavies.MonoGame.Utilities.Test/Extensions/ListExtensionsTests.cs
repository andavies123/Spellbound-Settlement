using Andavies.MonoGame.Utilities.Extensions;

namespace Andavies.MonoGame.Utilities.Test.Extensions;

[TestClass]
public class ListExtensionsTests
{
	#region IsEmpty Tests

	[TestMethod]
	public void IsEmpty_ReturnsTrue_WhenTheListDoesNotContainAnyValues()
	{
		// Arrange
		List<int> list = new();
		
		// Act
		bool isEmpty = list.IsEmpty();
        
		// Assert
		isEmpty.Should().BeTrue();
	}

	[TestMethod]
	public void IsEmpty_ReturnsFalse_WhenTheListContainsASingleItem()
	{
		// Arrange
		List<int> list = new() {1};

		// Act
		bool isEmpty = list.IsEmpty();

		// Assert
		isEmpty.Should().BeFalse();
	}

	[TestMethod]
	public void IsEmpty_ReturnsTrue_WhenListRemovesAllItems()
	{
		// Arrange
		List<int> list = new() {1, 2, 3, 4, 5};
		
		// Act
		list.Clear();
		bool isEmpty = list.IsEmpty();
		
		// Assert
		isEmpty.Should().BeTrue();
	}

	#endregion
}