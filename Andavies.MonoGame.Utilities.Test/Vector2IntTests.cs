using Microsoft.Xna.Framework;

namespace Andavies.MonoGame.Utilities.Test;

[TestClass]
public class Vector2IntTests
{
	#region Constructor Tests

	[TestMethod]
	public void Constructor_XYSetToValuesFromIntConstructor()
	{
		// Act
		Vector2Int vector2Int = new(98, 67);

		// Assert
		vector2Int.X.Should().Be(98);
		vector2Int.Y.Should().Be(67);
	}

	[TestMethod]
	public void Constructor_XYSetToTruncatedValuesFromFloatConstructor()
	{
		// Act
		Vector2Int vector2Int = new(98.78f, -789.367f);
		
		// Assert
		vector2Int.X.Should().Be(98);
		vector2Int.Y.Should().Be(-789);
	}

	[TestMethod]
	public void Constructor_XYSetToSameValueFromSingleValueConstructor()
	{
		// Act
		Vector2Int vector2Int = new(78);
		
		// Assert
		vector2Int.X.Should().Be(78);
		vector2Int.Y.Should().Be(78);
	}
	
	[TestMethod]
	public void Constructor_XYSetToTruncatedValuesFromVector2()
	{
        // Arrange
        Vector2 vector2 = new(0.3f, 13.45667f);
        
        // Act
        Vector2Int vector2Int = new(vector2);
        
        // Assert
        vector2Int.X.Should().Be(0);
        vector2Int.Y.Should().Be(13);
	}

	[TestMethod]
	public void Constructor_XYSetToZeroFromDefaultConstructor()
	{
		// Act
		Vector2Int vector2Int = new();
		
		// Assert
		vector2Int.X.Should().Be(0);
		vector2Int.Y.Should().Be(0);
	}

	#endregion

	#region Static Property Tests

	[TestMethod]
	public void Property_ZeroVector_AllValuesAreEqualToZero()
	{
		// Arrange
		Vector2Int zeroVector = Vector2Int.Zero;
		
		// Act
		zeroVector.X.Should().Be(0);
		zeroVector.Y.Should().Be(0);
	}
	
	[TestMethod]
	public void Property_OneVector_AllValuesAreEqualToZero()
	{
		// Arrange
		Vector2Int oneVector = Vector2Int.One;
		
		// Act
		oneVector.X.Should().Be(1);
		oneVector.Y.Should().Be(1);
	}

	#endregion
	
	#region Decontsruct Tests

	[TestMethod]
	public void Deconstruct_ReturnsInternalXYZ()
	{
		// Arrange
		Vector2Int vector2Int = new(1, 2);
		
		// Act
		(int x, int y) = vector2Int;
		
		// Assert
		x.Should().Be(vector2Int.X);
		y.Should().Be(vector2Int.Y);
	}

	#endregion

	#region ToVector3IntNoY Tests

	[TestMethod]
	public void ToVector3IntNoY_ReturnsVector3IntWithXAndZSetAndYSetToZero()
	{
		// Arrange
		Vector2Int vector2Int = new(5, 10);
		
		// Act
		Vector3Int vector3Int = vector2Int.ToVector3IntNoY();
		
		// Assert
		vector3Int.X.Should().Be(5);
		vector3Int.Y.Should().Be(0);
		vector3Int.Z.Should().Be(10);
	}

	#endregion

	#region IsValidArrayIndex Tests

	[TestMethod]
	public void IsValidArrayIndex_ReturnsTrue_WhenValuesAreWithinArrayRange()
	{
		// Arrange
		Vector2Int vector2Int = new(0, 1);
		int[,] array = {{1, 2, 3}, {1, 2, 3}};
		
		// Act
		bool isValidArrayIndex = vector2Int.IsValidArrayIndex(array);

		// Assert
		isValidArrayIndex.Should().BeTrue();
	}

	[TestMethod]
	public void IsValidArrayIndex_ReturnsFalse_WhenBothValuesAreNegative()
	{
		// Arrange
		Vector2Int vector2Int = new(-1, -1);
		int[,] array = {{1, 2, 3}, {1, 2, 3}};
		
		// Act
		bool isValidArrayIndex = vector2Int.IsValidArrayIndex(array);

		// Assert
		isValidArrayIndex.Should().BeFalse();
	}

	[TestMethod]
	public void IsValidArrayIndex_ReturnsFalse_WhenXValueIsNegative_AndYValueIsValid()
	{
		// Arrange
		Vector2Int vector2Int = new(-1, 1);
		int[,] array = {{1, 2, 3}, {1, 2, 3}};
		
		// Act
		bool isValidArrayIndex = vector2Int.IsValidArrayIndex(array);

		// Assert
		isValidArrayIndex.Should().BeFalse();
	}

	[TestMethod]
	public void IsValidArrayIndex_ReturnsFalse_WhenYValueIsNegative_AndXValueIsValid()
	{
		// Arrange
		Vector2Int vector2Int = new(1, -1);
		int[,] array = {{1, 2, 3}, {1, 2, 3}};
		
		// Act
		bool isValidArrayIndex = vector2Int.IsValidArrayIndex(array);

		// Assert
		isValidArrayIndex.Should().BeFalse();
	}

	[TestMethod]
	public void IsValidArrayIndex_ReturnsFalse_WhenXValueIsArrayLength_AndYValueIsValid()
	{
		// Arrange
		Vector2Int vector2Int = new(3, 1);
		int[,] array = {{1, 2, 3}, {1, 2, 3}};
		
		// Act
		bool isValidArrayIndex = vector2Int.IsValidArrayIndex(array);

		// Assert
		isValidArrayIndex.Should().BeFalse();
	}

	[TestMethod]
	public void IsValidArrayIndex_ReturnsFalse_WhenYValueIsArrayLength_AndXValueIsValid()
	{
		// Arrange
		Vector2Int vector2Int = new(1, 3);
		int[,] array = {{1, 2, 3}, {1, 2, 3}};
		
		// Act
		bool isValidArrayIndex = vector2Int.IsValidArrayIndex(array);

		// Assert
		isValidArrayIndex.Should().BeFalse();
	}

	[TestMethod]
	public void IsValidArrayIndex_ReturnsFalse_WhenBothValuesAreInvalid()
	{
		// Arrange
		Vector2Int vector2Int = new(4, 4);
		int[,] array = {{1, 2, 3}, {1, 2, 3}};
		
		// Act
		bool isValidArrayIndex = vector2Int.IsValidArrayIndex(array);

		// Assert
		isValidArrayIndex.Should().BeFalse();
	}

	#endregion
	
	#region Distance Tests

	[TestMethod]
	public void Distance_ReturnsSameValue_WhenUsingEitherStaticOrNonStaticMethods()
	{
		// Arrange
		Vector2Int vectorA = new(5, 5);
		Vector2Int vectorB = new(5, 10);
		
		// Act
		float staticAnswer = Vector2Int.Distance(vectorA, vectorB);
		float nonStaticAnswer = vectorA.Distance(vectorB);

		// Assert
		staticAnswer.Should().Be(nonStaticAnswer);
	}

	[TestMethod]
	public void Distance_ReturnsCorrectDistance_WhenUsingStaticMethod()
	{
		// Arrange
		Vector2Int vectorA = new(5, 5);
		Vector2Int vectorB = new(5, 10);
		
		// Act
		float distance = Vector2Int.Distance(vectorA, vectorB);
		
		// Assert
		distance.Should().Be(5);
	}

	[TestMethod]
	public void Distance_ReturnsZero_WhenUsingEqualVectors()
	{
		// Arrange
		Vector2Int vector = new(5);
		
		// Act
		float distance = Vector2Int.Distance(vector, vector);

		// Assert
		distance.Should().Be(0);
	}

	[TestMethod]
	public void Distance_ReturnsSameValue_WhenSwappingVectorOrder()
	{
		// Arrange
		Vector2Int vectorA = new(5);
		Vector2Int vectorB = new(10);
		
		// Act
		float distanceAtoB = Vector2Int.Distance(vectorA, vectorB);
		float distanceBtoA = Vector2Int.Distance(vectorB, vectorA);
		
		// Assert
		distanceAtoB.Should().Be(distanceBtoA);
	}

	#endregion

	#region Operator + Tests

	[TestMethod]
	public void OperatorAddition_ReturnsCorrectValue()
	{
		// Arrange
		Vector2Int vectorA = new(5, 5);
		Vector2Int vectorB = new(17, 23);
		
		// Act
		Vector2Int answerVector = vectorA + vectorB;
		
		// Assert
		answerVector.X.Should().Be(22);
		answerVector.Y.Should().Be(28);
	}

	[TestMethod]
	public void OperatorAddition_ReturnsSameValue_WhenSwappingOrder()
	{
		// Arrange
		Vector2Int vectorA = new(5, 5);
		Vector2Int vectorB = new(17, 23);
		
		// Act
		Vector2Int answerVectorA = vectorA + vectorB;
		Vector2Int answerVectorB = vectorB + vectorA;
		
		// Assert
		answerVectorA.Should().Be(answerVectorB);
	}

	[TestMethod]
	public void OperatorAddition_ReturnsCorrectValue_AddingVector2IntWithVector2()
	{
		// Arrange
		Vector2Int vector2Int = new(78, 38);
		Vector2 vector2 = new(2.3f, 12.334f);
		
		// Act
		Vector2 answer = vector2Int + vector2;
		
		// Assert
		answer.X.Should().Be(80.3f);
		answer.Y.Should().Be(50.334f);
	}

	[TestMethod]
	public void OperatorAddition_ReturnsSameValue_WhenSwappingOrderOfVector2IntAndVector2()
	{
		// Arrange
		Vector2Int vector2Int = new(5, 5);
		Vector2 vector2 = new(17.46f, 23.925f);
		
		// Act
		Vector2 answerVectorA = vector2Int + vector2;
		Vector2 answerVectorB = vector2 + vector2Int;
		
		// Assert
		answerVectorA.Should().Be(answerVectorB);
	}

	#endregion

	#region Operator - Tests

	[TestMethod]
	public void OperatorSubtraction_ReturnsCorrectValue()
	{
		// Arrange
		Vector2Int vectorA = new(5, 5);
		Vector2Int vectorB = new(17, 23);
		
		// Act
		Vector2Int answerVector = vectorA - vectorB;
		
		// Assert
		answerVector.X.Should().Be(-12);
		answerVector.Y.Should().Be(-18);
	}

	[TestMethod]
	public void OperatorSubtraction_ReturnsCorrectValue_SubtractingVector2FromVector2Int()
	{
		// Arrange
		Vector2Int vector2Int = new(78, 38);
		Vector2 vector2 = new(2.3f, 12.334f);
		
		// Act
		Vector2 answer = vector2Int - vector2;
		
		// Assert
		answer.X.Should().Be(75.7f);
		answer.Y.Should().Be(25.666f);
	}

	[TestMethod]
	public void OperatorSubtraction_ReturnsCorrectValue_SubtractingVector2IntFromVector2()
	{
		// Arrange
		Vector2Int vector2Int = new(78, 38);
		Vector2 vector2 = new(2.3f, 12.334f);
		
		// Act
		Vector2 answer = vector2 - vector2Int;
		
		// Assert
		answer.X.Should().Be(-75.7f);
		answer.Y.Should().Be(-25.666f);
	}

	#endregion

	#region Operator * Tests

	[TestMethod]
	public void OperatorMultiplication_ReturnsCorrectValue()
	{
		// Arrange
		Vector2Int vectorA = new(5, 5);
		Vector2Int vectorB = new(17, 23);
		
		// Act
		Vector2Int answerVector = vectorA * vectorB;
		
		// Assert
		answerVector.X.Should().Be(85);
		answerVector.Y.Should().Be(115);
	}

	[TestMethod]
	public void OperatorMultiplication_ReturnsSameValue_WhenSwappingOrder()
	{
		// Arrange
		Vector2Int vectorA = new(5, 5);
		Vector2Int vectorB = new(17, 23);
		
		// Act
		Vector2Int answerVectorA = vectorA * vectorB;
		Vector2Int answerVectorB = vectorB * vectorA;
		
		// Assert
		answerVectorA.Should().Be(answerVectorB);
	}

	[TestMethod]
	public void OperatorMultiplication_ReturnsCorrectValue_MultiplyingVector2IntWithVector2()
	{
		// Arrange
		Vector2Int vector2Int = new(78, 38);
		Vector2 vector2 = new(2.3f, 12.334f);
		
		// Act
		Vector2 answer = vector2Int * vector2;
		
		// Assert
		answer.X.Should().Be(179.4f);
		answer.Y.Should().Be(468.692f);
	}

	[TestMethod]
	public void OperatorMultiplication_ReturnsSameValue_WhenSwappingOrderOfVector2IntAndVector2()
	{
		// Arrange
		Vector2Int vector2Int = new(5, 5);
		Vector2 vector2 = new(17.46f, 23.925f);
		
		// Act
		Vector2 answerVectorA = vector2Int * vector2;
		Vector2 answerVectorB = vector2 * vector2Int;
		
		// Assert
		answerVectorA.Should().Be(answerVectorB);
	}

	[TestMethod]
	public void OperatorMultiplication_ReturnsCorrectValue_WhenUsingScaleInt()
	{
		// Arrange
		Vector2Int vector = new(5, 10);
		
		// Act
		Vector2Int scaledVector = vector * 10;
		
		// Assert
		scaledVector.X.Should().Be(50);
		scaledVector.Y.Should().Be(100);
	}

	[TestMethod]
	public void OperatorMultiplication_ReturnsCorrectVector2_WhenUsingScaleFloat()
	{
		// Arrange
		Vector2Int vector2Int = new(5, 10);
		
		// Act
		Vector2 scaledVector = vector2Int * 1.5f;
		
		// Assert
		scaledVector.X.Should().Be(7.5f);
		scaledVector.Y.Should().Be(15f);
	}

	#endregion

	#region Operator / Tests

	[TestMethod]
	public void OperatorDivision_ReturnsCorrectValue_BetweenTwoVector2Ints()
	{
		// Arrange
		Vector2Int vectorA = new(100, 100);
		Vector2Int vectorB = new(2, 20);
		
		// Act
		Vector2 answerVector = vectorA / vectorB;
		
		// Assert
		answerVector.X.Should().Be(50);
		answerVector.Y.Should().Be(5);
	}

	[TestMethod]
	public void OperatorDivision_ThrowsDivideByZeroException_WhenSecondVector2IntHasZeroX()
	{
		// Arrange
		Vector2Int vectorA = new(5, 10);
		Vector2Int vectorB = new(0, 10);
		
		// Act
		Action act = () =>
		{
			Vector2 _ = vectorA / vectorB;
		};

		// Assert
		act.Should().ThrowExactly<DivideByZeroException>();
	}

	[TestMethod]
	public void OperatorDivision_ThrowsDivideByZeroException_WhenSecondVector2IntHasZeroY()
	{
		// Arrange
		Vector2Int vectorA = new(5, 10);
		Vector2Int vectorB = new(5, 0);
		
		// Act
		Action act = () =>
		{
			Vector2 _ = vectorA / vectorB;
		};

		// Assert
		act.Should().ThrowExactly<DivideByZeroException>();
	}

	[TestMethod]
	public void OperatorDivision_DoesNotThrowDivideByZeroException_WhenSecondVector2IntDoesNotContainZero()
	{
		// Arrange
		Vector2Int vectorA = new(5, 10);
		Vector2Int vectorB = new(5, 10);
		
		// Act
		Action act = () =>
		{
			Vector2 _ = vectorA / vectorB;
		};

		// Assert
		act.Should().NotThrow<DivideByZeroException>();
	}

	[TestMethod]
	public void OperatorDivision_ReturnsCorrectValue_WhenDividingVector2IntWithVector2()
	{
		// Arrange
		Vector2Int vector2Int = new(100, 100);
		Vector2 vector2 = new(1.25f, -4f);
		
		// Act
		Vector2 answer = vector2Int / vector2;
		
		// Assert
		answer.X.Should().Be(80f);
		answer.Y.Should().Be(-25f);
	}

	[TestMethod]
	public void OperatorDivision_ThrowsDivideByZeroException_WhenSecondVector2HasZeroX()
	{
		// Arrange
		Vector2Int vector2Int = new(5, 10);
		Vector2 vector2 = new(0f, -4f);
		
		// Act
		Action act = () =>
		{
			Vector2 _ = vector2Int / vector2;
		};

		// Assert
		act.Should().ThrowExactly<DivideByZeroException>();
	}

	[TestMethod]
	public void OperatorDivision_ThrowsDivideByZeroException_WhenSecondVector2HasZeroY()
	{
		// Arrange
		Vector2Int vector2Int = new(5, 10);
		Vector2 vector2 = new(1.5f, 0f);
		
		// Act
		Action act = () =>
		{
			Vector2 _ = vector2Int / vector2;
		};

		// Assert
		act.Should().ThrowExactly<DivideByZeroException>();
	}

	[TestMethod]
	public void OperatorDivision_DoesNotThrowDivideByZeroException_WhenSecondVector2DoesNotContainZero()
	{
		// Arrange
		Vector2Int vector2Int = new(5, 10);
		Vector2 vector2 = new(1.5f, -4f);
		
		// Act
		Action act = () =>
		{
			Vector2 _ = vector2Int / vector2;
		};

		// Assert
		act.Should().NotThrow<DivideByZeroException>();
	}

	[TestMethod]
	public void OperatorDivision_ReturnsCorrectValue_WhenDividingVector2WithVector2Int()
	{
		// Arrange
		Vector2Int vector2Int = new(100, 100);
		Vector2 vector2 = new(1.25f, -4f);
		
		// Act
		Vector2 answer = vector2 / vector2Int;
		
		// Assert
		answer.X.Should().Be(.0125f);
		answer.Y.Should().Be(-.04f);
	}

	[TestMethod]
	public void OperatorDivision_ThrowsDivideByZeroException_WhenSecondVector2IntHasZeroX_WhenDividingVector2ByVector2Int()
	{
		// Arrange
		Vector2Int vector2Int = new(0, 100);
		Vector2 vector2 = new(1.25f, -4f);
		
		// Act
		Action act = () =>
		{
			Vector2 _ = vector2 / vector2Int;
		};

		// Assert
		act.Should().ThrowExactly<DivideByZeroException>();
	}

	[TestMethod]
	public void OperatorDivision_ThrowsDivideByZeroException_WhenSecondVector2IntHasZeroY_WhenDividingVector2ByVector2Int()
	{
		// Arrange
		Vector2Int vector2Int = new(100, 0);
		Vector2 vector2 = new(1.25f, -4f);
		
		// Act
		Action act = () =>
		{
			Vector2 _ = vector2 / vector2Int;
		};

		// Assert
		act.Should().ThrowExactly<DivideByZeroException>();
	}

	[TestMethod]
	public void OperatorDivision_DoesNotThrowDivideByZeroException_WhenSecondVector2IntDoesNotContainZero_WhenDividingVector2ByVector2Int()
	{
		// Arrange
		Vector2Int vector2Int = new(100, 100);
		Vector2 vector2 = new(1.25f, -4f);
		
		// Act
		Action act = () =>
		{
			Vector2 _ = vector2 / vector2Int;
		};

		// Assert
		act.Should().NotThrow<DivideByZeroException>();
	}

	[TestMethod]
	public void OperatorDivision_ReturnsCorrectValue_WhenUsingScaleInt()
	{
		// Arrange
		Vector2Int vector = new(5, 10);
		
		// Act
		Vector2 scaledVector = vector / 10;
		
		// Assert
		scaledVector.X.Should().Be(.5f);
		scaledVector.Y.Should().Be(1f);
	}

	[TestMethod]
	public void OperatorDivision_ReturnsCorrectVector2_WhenUsingScaleFloat()
	{
		// Arrange
		Vector2Int vector2Int = new(3, 9);
		
		// Act
		Vector2 scaledVector = vector2Int / 1.5f;
		
		// Assert
		scaledVector.X.Should().Be(2f);
		scaledVector.Y.Should().Be(6f);
	}

	[TestMethod]
	public void OperatorDivision_ThrowsDivideByZeroException_WhenPassingIntScaleAsZero()
	{
		// Arrange
		Vector2Int vector2Int = new(5, 10);
		
		// Act
		Action act = () =>
		{
			Vector2 _ = vector2Int / 0;
		};

		// Assert
		act.Should().ThrowExactly<DivideByZeroException>();
	}

	[TestMethod]
	public void OperatorDivision_DoesNotThrowDivideByZeroException_WhenPassingIntScaleAsNonZero()
	{
		// Arrange
		Vector2Int vector2Int = new(5, 10);
		
		// Act
		Action act = () =>
		{
			Vector2 _ = vector2Int / 1;
		};

		// Assert
		act.Should().NotThrow<DivideByZeroException>();
	}

	[TestMethod]
	public void OperatorDivision_ThrowsDivideByZeroException_WhenPassingFloatScaleAsZero()
	{
		// Arrange
		Vector2Int vector2Int = new(5, 10);
		
		// Act
		Action act = () =>
		{
			Vector2 _ = vector2Int / 0f;
		};

		// Assert
		act.Should().ThrowExactly<DivideByZeroException>();
	}

	[TestMethod]
	public void OperatorDivision_DoesNotThrowDivideByZeroException_WhenPassingFloatScaleAsNonZero()
	{
		// Arrange
		Vector2Int vector2Int = new(5, 10);
		
		// Act
		Action act = () =>
		{
			Vector2 _ = vector2Int / .0001f;
		};

		// Assert
		act.Should().NotThrow<DivideByZeroException>();
	}

	#endregion

	#region Operator == Tests

	[TestMethod]
	public void OperatorEqualsEquals_ReturnsTrue_WhenComparingTheSameObject()
	{
		// Arrange
		Vector2Int vector2Int = new(5, 10);
		
		// Act
		// ReSharper disable once EqualExpressionComparison
		bool isEqual = vector2Int == vector2Int;

		// Assert
		isEqual.Should().BeTrue();
	}

	[TestMethod]
	public void OperatorEqualsEquals_ReturnsTrue_WhenComparingTwoVectorsWithSameValues()
	{
		// Arrange
		Vector2Int vector2IntA = new(5, 10);
		Vector2Int vector2IntB = new(5, 10);
		
		// Act
		bool isEqual = vector2IntA == vector2IntB;

		// Assert
		isEqual.Should().BeTrue();
	}

	[TestMethod]
	public void OperatorEqualsEquals_ReturnsFalse_WhenComparingTwoVectorsWithDifferentValues()
	{
		// Arrange
		Vector2Int vector2IntA = new(5, 10);
		Vector2Int vector2IntB = new(15, 20);
		
		// Act
		bool isEqual = vector2IntA == vector2IntB;

		// Assert
		isEqual.Should().BeFalse();	
	}

	[TestMethod]
	public void OperatorEqualsEquals_ReturnsFalse_WhenComparingTwoVectorsWithSameValuesInDifferentPositions()
	{
		// Arrange
		Vector2Int vector2IntA = new(5, 10);
		Vector2Int vector2IntB = new(10, 5);
		
		// Act
		bool isEqual = vector2IntA == vector2IntB;

		// Assert
		isEqual.Should().BeFalse();
	}

	#endregion

	#region Operator != Tests

	[TestMethod]
	public void OperatorNotEquals_ReturnsFalse_WhenComparingTheSameObject()
	{
		// Arrange
		Vector2Int vector2Int = new(5, 10);
		
		// Act
		// ReSharper disable once EqualExpressionComparison
		bool isNotEqual = vector2Int != vector2Int;
		
		// Assert
		isNotEqual.Should().BeFalse();
	}

	[TestMethod]
	public void OperatorNotEquals_ReturnsFalse_WhenComparingTwoVectorsWithSameValues()
	{
		// Arrange
		Vector2Int vector2IntA = new(5, 10);
		Vector2Int vector2IntB = new(5, 10);
		
		// Act
		bool isNotEqual = vector2IntA != vector2IntB;
		
		// Assert
		isNotEqual.Should().BeFalse();
	}

	[TestMethod]
	public void OperatorNotEquals_ReturnsTrue_WhenComparingTwoVectorsWithDifferentValues()
	{
		// Arrange
		Vector2Int vector2IntA = new(5, 10);
		Vector2Int vector2IntB = new(67, 78);
		
		// Act
		bool isNotEqual = vector2IntA != vector2IntB;
		
		// Assert
		isNotEqual.Should().BeTrue();
	}

	[TestMethod]
	public void OperatorNotEquals_ReturnsTrue_WhenComparingTwoVectorsWithSameValuesInDifferentPositions()
	{
		// Arrange
		Vector2Int vector2IntA = new(5, 10);
		Vector2Int vector2IntB = new(10, 5);
		
		// Act
		bool isNotEqual = vector2IntA != vector2IntB;
		
		// Assert
		isNotEqual.Should().BeTrue();
	}

	#endregion

	#region Equals Tests

	[TestMethod]
	public void Equals_ReturnsTrue_WhenComparingTheSameObject()
	{
		// Arrange
		Vector2Int vector2Int = new(5, 10);
		
		// Act
		// ReSharper disable once EqualExpressionComparison
		bool isEqual = vector2Int.Equals(vector2Int);
		
		// Assert
		isEqual.Should().BeTrue();
	}

	[TestMethod]
	public void Equals_ReturnsTrue_WhenComparingTwoVectorsWithSameValues()
	{
		// Arrange
		Vector2Int vector2IntA = new(5, 10);
		Vector2Int vector2IntB = new(5, 10);
		
		// Act
		bool isEqual = vector2IntA.Equals(vector2IntB);
		
		// Assert
		isEqual.Should().BeTrue();
	}

	[TestMethod]
	public void Equals_ReturnsFalse_WhenComparingTwoVectorsWithDifferentValues()
	{
		// Arrange
		Vector2Int vector2IntA = new(5, 10);
		Vector2Int vector2IntB = new(130, 15);
		
		// Act
		bool isEqual = vector2IntA.Equals(vector2IntB);
		
		// Assert
		isEqual.Should().BeFalse();
	}

	[TestMethod]
	public void Equals_ReturnsFalse_WhenComparingTwoVectorsWithSameValuesInDifferentPositions()
	{
		// Arrange
		Vector2Int vector2IntA = new(5, 10);
		Vector2Int vector2IntB = new(10, 5);
		
		// Act
		bool isEqual = vector2IntA.Equals(vector2IntB);
		
		// Assert
		isEqual.Should().BeFalse();
	}

	[TestMethod]
	public void Equals_ReturnsFalse_WhenComparingToVector2Object_WithSameValues()
	{
		// Arrange
		Vector2Int vector2Int = new(5, 10);
		Vector2 vector2 = new(5, 10);
		
		// Act
		// ReSharper disable once SuspiciousTypeConversion.Global
		bool isEqual = vector2Int.Equals(vector2);
		
		// Assert
		isEqual.Should().BeFalse();
	}

	#endregion

	#region GetHashCode Tests

	[TestMethod]
	public void GetHashCode_ReturnsSameValue_FromTwoEqualVectors()
	{
		// Arrange
		Vector2Int vector2IntA = new(5, 10);
		Vector2Int vector2IntB = new(5, 10);
		
		// Act
		int hashA = vector2IntA.GetHashCode();
		int hashB = vector2IntB.GetHashCode();
		
		// Assert
		hashA.Should().Be(hashB);
	}

	[TestMethod]
	public void GetHashCode_DoesNotReturnSameValue_FromTwoDifferentVectors()
	{
		// Arrange
		Vector2Int vector2IntA = new(5, 10);
		Vector2Int vector2IntB = new(34, 64);
		
		// Act
		int hashA = vector2IntA.GetHashCode();
		int hashB = vector2IntB.GetHashCode();
		
		// Assert
		hashA.Should().NotBe(hashB);
	}

	[TestMethod]
	public void GetHashCode_DoesNotReturnSameValue_FromTwoVectorsWithSwappedValues()
	{
		// Arrange
		Vector2Int vector2IntA = new(5, 10);
		Vector2Int vector2IntB = new(10, 5);
		
		// Act
		int hashA = vector2IntA.GetHashCode();
		int hashB = vector2IntB.GetHashCode();
		
		// Assert
		hashA.Should().NotBe(hashB);
	}

	#endregion

	#region ToString Tests

	[TestMethod]
	public void ToString_ReturnsStringContainingXAndYValues()
	{
		// Arrange
		Vector2Int vector2Int = new(5, 7);
		
		// Act
		string vector2IntString = vector2Int.ToString();
		
		// Assert
		vector2IntString.Should().ContainAll(new List<string> {"5", "7"});
	}

	#endregion

	#region Casting from Vector3Int to Vector3 Tests

	[TestMethod]
	public void CastVector3_ReturnsCorrectValue()
	{
		// Arrange
		Vector2Int vector2Int = new(5, 10);
		
		// Act
		Vector2 vector2 = (Vector2) vector2Int;
		
		// Assert
		vector2.X.Should().Be(5f);
		vector2.Y.Should().Be(10f);
	}

	#endregion

	#region Casting from Vector3 to Vector3Int Tests

	[TestMethod]
	public void CastVector2Int_ReturnsCorrectValue()
	{
		// Arrange
		Vector2 vector2 = new(5.7f, -1.2f);
		
		// Act
		Vector2Int vector2Int = (Vector2Int) vector2;
		
		// Assert
		vector2Int.X.Should().Be(5);
		vector2Int.Y.Should().Be(-1);
	}

	#endregion
}