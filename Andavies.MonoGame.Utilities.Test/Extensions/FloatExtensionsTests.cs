using Andavies.MonoGame.Utilities.Extensions;

namespace Andavies.MonoGame.Utilities.Test.Extensions;

[TestClass]
public class FloatExtensionsTests
{
	#region GetTenthsPlace Tests

	[DataTestMethod]
	[DataRow(1.234f, 2)]
	[DataRow(75.468f, 4)]
	[DataRow(972.249f, 2)]
	[DataRow(-1.237f, 2)]
	public void GetTenthsPlace_ReturnsCorrectNumber(float value, int expected)
	{
		// Act
		int answer = value.GetTenthsPlace();

		// Assert
		answer.Should().Be(expected);
	}

	#endregion
	
	#region GetTenthsPlace Tests

	[DataTestMethod]
	[DataRow(1.234f, 3)]
	[DataRow(75.468f, 6)]
	[DataRow(972.249f, 4)]
	[DataRow(-1.237f, 3)]
	public void GetHundredthsPlace_ReturnsCorrectNumber(float value, int expected)
	{
		// Act
		int answer = value.GetHundredthsPlace();

		// Assert
		answer.Should().Be(expected);
	}

	#endregion
	
	#region GetThousandthsPlace Tests

	[DataTestMethod]
	[DataRow(1.234f, 4)]
	[DataRow(75.468f, 8)]
	[DataRow(972.249f, 9)]
	[DataRow(-1.237f, 7)]
	public void GetThousandthsPlace_ReturnsCorrectNumber(float value, int expected)
	{
		// Act
		int answer = value.GetThousandthsPlace();

		// Assert
		answer.Should().Be(expected);
	}

	#endregion
}