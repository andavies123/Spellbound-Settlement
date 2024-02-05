using Microsoft.Xna.Framework;

namespace Andavies.MonoGame.Utilities.Test;

[TestClass]
public class Vector3IntTests
{
	#region Constructor Tests

	[TestMethod]
	public void Constructor_XYZSet_ToValuesFromIntConstructor()
	{
		// Act
		Vector3Int vector = new(97, -725, 3247);
		
		// Assert
		vector.X.Should().Be(97);
		vector.Y.Should().Be(-725);
		vector.Z.Should().Be(3247);
	}

	[TestMethod]
	public void Constructor_XYZSetToSameValue_WhenUsingSingleParameterConstructor()
	{
		// Arrange
		const int value = 55;

		// Act
		Vector3Int vector = new(value);
		
		// Assert
		vector.X.Should().Be(value);
		vector.Y.Should().Be(value);
		vector.Z.Should().Be(value);
	}

	[TestMethod]
	public void Constructor_XYZSet_ToTruncatedValuesFromFloats()
	{
		// Act
		Vector3Int vector3Int = new(-5.99f, 4.975f, 100.7849f);

		// Assert
		vector3Int.X.Should().Be(-5);
		vector3Int.Y.Should().Be(4);
		vector3Int.Z.Should().Be(100);
	}

	[TestMethod]
	public void Constructor_XYZSet_ToTruncatedValuesFromVector3()
	{
		// Arrange
		Vector3 vector3 = new(5.5f, -1.2f, 9.7f);

		// Act
		Vector3Int vector3Int = new(vector3);

		// Assert
		vector3Int.X.Should().Be(5);
		vector3Int.Y.Should().Be(-1);
		vector3Int.Z.Should().Be(9);
	}

	[TestMethod]
	public void Constructor_XYZSetToZero_WhenCallingDefaultConstructor()
	{
		// Arrange
		Vector3Int vector3Int = new();
		
		// Assert
		vector3Int.X.Should().Be(0);
		vector3Int.Y.Should().Be(0);
		vector3Int.Z.Should().Be(0);
	}

	#endregion

	#region Static Property Tests

	[TestMethod]
	public void Property_ZeroVector_AllValuesAreEqualToZero()
	{
		// Arrange
		Vector3Int zeroVector = Vector3Int.Zero;
		
		// Act
		zeroVector.X.Should().Be(0);
		zeroVector.Y.Should().Be(0);
		zeroVector.Z.Should().Be(0);
	}
	
	[TestMethod]
	public void Property_OneVector_AllValuesAreEqualToZero()
	{
		// Arrange
		Vector3Int oneVector = Vector3Int.One;
		
		// Act
		oneVector.X.Should().Be(1);
		oneVector.Y.Should().Be(1);
		oneVector.Z.Should().Be(1);
	}

	#endregion

	#region Distance Tests

	[TestMethod]
	public void Distance_ReturnsSameValue_WhenUsingEitherStaticOrNonStaticMethods()
	{
		// Arrange
		Vector3Int vectorA = new(5, 5, 5);
		Vector3Int vectorB = new(5, 5, 10);
		
		// Act
		float staticAnswer = Vector3Int.Distance(vectorA, vectorB);
		float nonStaticAnswer = vectorA.Distance(vectorB);

		// Assert
		staticAnswer.Should().Be(nonStaticAnswer);
	}

	[TestMethod]
	public void Distance_ReturnsCorrectDistance_WhenUsingStaticMethod()
	{
		// Arrange
		Vector3Int vectorA = new(5, 5, 5);
		Vector3Int vectorB = new(5, 10, 5);
		
		// Act
		float distance = Vector3Int.Distance(vectorA, vectorB);
		
		// Assert
		distance.Should().Be(5);
	}

	[TestMethod]
	public void Distance_ReturnsZero_WhenUsingEqualVectors()
	{
		// Arrange
		Vector3Int vector = new(5);
		
		// Act
		float distance = Vector3Int.Distance(vector, vector);

		// Assert
		distance.Should().Be(0);
	}

	[TestMethod]
	public void Distance_ReturnsSameValue_WhenSwappingVectorOrder()
	{
		// Arrange
		Vector3Int vectorA = new(5);
		Vector3Int vectorB = new(10);
		
		// Act
		float distanceAtoB = Vector3Int.Distance(vectorA, vectorB);
		float distanceBtoA = Vector3Int.Distance(vectorB, vectorA);
		
		// Assert
		distanceAtoB.Should().Be(distanceBtoA);
	}

	#endregion

	#region Operator + Tests

	[TestMethod]
	public void OperatorAddition_ReturnsCorrectValue()
	{
		// Arrange
		Vector3Int vectorA = new(5, 5, 5);
		Vector3Int vectorB = new(17, 23, 64);
		
		// Act
		Vector3Int answerVector = vectorA + vectorB;
		
		// Assert
		answerVector.X.Should().Be(22);
		answerVector.Y.Should().Be(28);
		answerVector.Z.Should().Be(69);
	}

	[TestMethod]
	public void OperatorAddition_ReturnsSameValue_WhenSwappingOrder()
	{
		// Arrange
		Vector3Int vectorA = new(5, 5, 5);
		Vector3Int vectorB = new(17, 23, 64);
		
		// Act
		Vector3Int answerVectorA = vectorA + vectorB;
		Vector3Int answerVectorB = vectorB + vectorA;
		
		// Assert
		answerVectorA.Should().Be(answerVectorB);
	}

	[TestMethod]
	public void OperatorAddition_ReturnsCorrectValue_AddingVector3IntWithVector3()
	{
		// Arrange
		Vector3Int vector3Int = new(78, 38, 58);
		Vector3 vector3 = new(2.3f, 12.334f, -12.3f);
		
		// Act
		Vector3 answer = vector3Int + vector3;
		
		// Assert
		answer.X.Should().Be(80.3f);
		answer.Y.Should().Be(50.334f);
		answer.Z.Should().Be(45.7f);
	}

	[TestMethod]
	public void OperatorAddition_ReturnsSameValue_WhenSwappingOrderOfVector3IntAndVector3()
	{
		// Arrange
		Vector3Int vector3Int = new(5, 5, 5);
		Vector3 vector3 = new(17.46f, 23.925f, 64.479f);
		
		// Act
		Vector3 answerVectorA = vector3Int + vector3;
		Vector3 answerVectorB = vector3 + vector3Int;
		
		// Assert
		answerVectorA.Should().Be(answerVectorB);
	}

	#endregion

	#region Operator - Tests

	[TestMethod]
	public void OperatorSubtraction_ReturnsCorrectValue()
	{
		// Arrange
		Vector3Int vectorA = new(5, 5, 5);
		Vector3Int vectorB = new(17, 23, 64);
		
		// Act
		Vector3Int answerVector = vectorA - vectorB;
		
		// Assert
		answerVector.X.Should().Be(-12);
		answerVector.Y.Should().Be(-18);
		answerVector.Z.Should().Be(-59);
	}

	[TestMethod]
	public void OperatorSubtraction_ReturnsCorrectValue_SubtractingVector3FromVector3Int()
	{
		// Arrange
		Vector3Int vector3Int = new(78, 38, 58);
		Vector3 vector3 = new(2.3f, 12.334f, -12.3f);
		
		// Act
		Vector3 answer = vector3Int - vector3;
		
		// Assert
		answer.X.Should().Be(75.7f);
		answer.Y.Should().Be(25.666f);
		answer.Z.Should().Be(70.3f);
	}

	[TestMethod]
	public void OperatorSubtraction_ReturnsCorrectValue_SubtractingVector3IntFromVector3()
	{
		// Arrange
		Vector3Int vector3Int = new(78, 38, 58);
		Vector3 vector3 = new(2.3f, 12.334f, -12.3f);
		
		// Act
		Vector3 answer = vector3 - vector3Int;
		
		// Assert
		answer.X.Should().Be(-75.7f);
		answer.Y.Should().Be(-25.666f);
		answer.Z.Should().Be(-70.3f);
	}

	#endregion

	#region Operator * Tests

	[TestMethod]
	public void OperatorMultiplication_ReturnsCorrectValue()
	{
		// Arrange
		Vector3Int vectorA = new(5, 5, 5);
		Vector3Int vectorB = new(17, 23, 64);
		
		// Act
		Vector3Int answerVector = vectorA * vectorB;
		
		// Assert
		answerVector.X.Should().Be(85);
		answerVector.Y.Should().Be(115);
		answerVector.Z.Should().Be(320);
	}

	[TestMethod]
	public void OperatorMultiplication_ReturnsSameValue_WhenSwappingOrder()
	{
		// Arrange
		Vector3Int vectorA = new(5, 5, 5);
		Vector3Int vectorB = new(17, 23, 64);
		
		// Act
		Vector3Int answerVectorA = vectorA * vectorB;
		Vector3Int answerVectorB = vectorB * vectorA;
		
		// Assert
		answerVectorA.Should().Be(answerVectorB);
	}

	[TestMethod]
	public void OperatorMultiplication_ReturnsCorrectValue_MultiplyingVector3IntWithVector3()
	{
		// Arrange
		Vector3Int vector3Int = new(78, 38, 58);
		Vector3 vector3 = new(2.3f, 12.334f, -12.3f);
		
		// Act
		Vector3 answer = vector3Int * vector3;
		
		// Assert
		answer.X.Should().Be(179.4f);
		answer.Y.Should().Be(468.692f);
		answer.Z.Should().Be(-713.4f);
	}

	[TestMethod]
	public void OperatorMultiplication_ReturnsSameValue_WhenSwappingOrderOfVector3IntAndVector3()
	{
		// Arrange
		Vector3Int vector3Int = new(5, 5, 5);
		Vector3 vector3 = new(17.46f, 23.925f, 64.479f);
		
		// Act
		Vector3 answerVectorA = vector3Int * vector3;
		Vector3 answerVectorB = vector3 * vector3Int;
		
		// Assert
		answerVectorA.Should().Be(answerVectorB);
	}

	[TestMethod]
	public void OperatorMultiplication_ReturnsCorrectValue_WhenUsingScaleInt()
	{
		// Arrange
		Vector3Int vector = new(5, 10, 15);
		
		// Act
		Vector3Int scaledVector = vector * 10;
		
		// Assert
		scaledVector.X.Should().Be(50);
		scaledVector.Y.Should().Be(100);
		scaledVector.Z.Should().Be(150);
	}

	[TestMethod]
	public void OperatorMultiplication_ReturnsCorrectVector3_WhenUsingScaleFloat()
	{
		// Arrange
		Vector3Int vector3Int = new(5, 10, 15);
		
		// Act
		Vector3 scaledVector = vector3Int * 1.5f;
		
		// Assert
		scaledVector.X.Should().Be(7.5f);
		scaledVector.Y.Should().Be(15f);
		scaledVector.Z.Should().Be(22.5f);
	}

	#endregion

	#region Operator / Tests

	[TestMethod]
	public void OperatorDivision_ReturnsCorrectValue_BetweenTwoVector3Ints()
	{
		// Arrange
		Vector3Int vectorA = new(100, 100, 100);
		Vector3Int vectorB = new(2, 20, 50);
		
		// Act
		Vector3 answerVector = vectorA / vectorB;
		
		// Assert
		answerVector.X.Should().Be(50);
		answerVector.Y.Should().Be(5);
		answerVector.Z.Should().Be(2);
	}

	[TestMethod]
	public void OperatorDivision_ThrowsDivideByZeroException_WhenSecondVector3IntHasZeroX()
	{
		// Arrange
		Vector3Int vector3IntA = new(5, 10, 15);
		Vector3Int vector3IntB = new(0, 10, 15);
		
		// Act
		Action act = () =>
		{
			Vector3 _ = vector3IntA / vector3IntB;
		};

		// Assert
		act.Should().ThrowExactly<DivideByZeroException>();
	}

	[TestMethod]
	public void OperatorDivision_ThrowsDivideByZeroException_WhenSecondVector3IntHasZeroY()
	{
		// Arrange
		Vector3Int vector3IntA = new(5, 10, 15);
		Vector3Int vector3IntB = new(5, 0, 15);
		
		// Act
		Action act = () =>
		{
			Vector3 _ = vector3IntA / vector3IntB;
		};

		// Assert
		act.Should().ThrowExactly<DivideByZeroException>();
	}

	[TestMethod]
	public void OperatorDivision_ThrowsDivideByZeroException_WhenSecondVector3IntHasZeroZ()
	{
		// Arrange
		Vector3Int vector3IntA = new(5, 10, 15);
		Vector3Int vector3IntB = new(5, 10, 0);
		
		// Act
		Action act = () =>
		{
			Vector3 _ = vector3IntA / vector3IntB;
		};

		// Assert
		act.Should().ThrowExactly<DivideByZeroException>();
	}

	[TestMethod]
	public void OperatorDivision_DoesNotThrowDivideByZeroException_WhenSecondVector3IntDoesNotContainZero()
	{
		// Arrange
		Vector3Int vector3IntA = new(5, 10, 15);
		Vector3Int vector3IntB = new(5, 10, 15);
		
		// Act
		Action act = () =>
		{
			Vector3 _ = vector3IntA / vector3IntB;
		};

		// Assert
		act.Should().NotThrow<DivideByZeroException>();
	}

	[TestMethod]
	public void OperatorDivision_ReturnsCorrectValue_WhenDividingVector3IntWithVector3()
	{
		// Arrange
		Vector3Int vector3Int = new(100, 100, 100);
		Vector3 vector3 = new(1.25f, -4f, 0.5f);
		
		// Act
		Vector3 answer = vector3Int / vector3;
		
		// Assert
		answer.X.Should().Be(80f);
		answer.Y.Should().Be(-25f);
		answer.Z.Should().Be(200f);
	}

	[TestMethod]
	public void OperatorDivision_ThrowsDivideByZeroException_WhenSecondVector3HasZeroX()
	{
		// Arrange
		Vector3Int vector3Int = new(5, 10, 15);
		Vector3 vector3 = new(0f, -4f, 0.5f);
		
		// Act
		Action act = () =>
		{
			Vector3 _ = vector3Int / vector3;
		};

		// Assert
		act.Should().ThrowExactly<DivideByZeroException>();
	}

	[TestMethod]
	public void OperatorDivision_ThrowsDivideByZeroException_WhenSecondVector3HasZeroY()
	{
		// Arrange
		Vector3Int vector3Int = new(5, 10, 15);
		Vector3 vector3 = new(1.5f, 0f, 0.5f);
		
		// Act
		Action act = () =>
		{
			Vector3 _ = vector3Int / vector3;
		};

		// Assert
		act.Should().ThrowExactly<DivideByZeroException>();
	}

	[TestMethod]
	public void OperatorDivision_ThrowsDivideByZeroException_WhenSecondVector3HasZeroZ()
	{
		// Arrange
		Vector3Int vector3Int = new(5, 10, 15);
		Vector3 vector3 = new(1.5f, -4f, 0f);
		
		// Act
		Action act = () =>
		{
			Vector3 _ = vector3Int / vector3;
		};

		// Assert
		act.Should().ThrowExactly<DivideByZeroException>();
	}

	[TestMethod]
	public void OperatorDivision_DoesNotThrowDivideByZeroException_WhenSecondVector3DoesNotContainZero()
	{
		// Arrange
		Vector3Int vector3Int = new(5, 10, 15);
		Vector3 vector3 = new(1.5f, -4f, 0.5f);
		
		// Act
		Action act = () =>
		{
			Vector3 _ = vector3Int / vector3;
		};

		// Assert
		act.Should().NotThrow<DivideByZeroException>();
	}

	[TestMethod]
	public void OperatorDivision_ReturnsCorrectValue_WhenDividingVector3WithVector3Int()
	{
		// Arrange
		Vector3Int vector3Int = new(100, 100, 100);
		Vector3 vector3 = new(1.25f, -4f, 0.5f);
		
		// Act
		Vector3 answer = vector3 / vector3Int;
		
		// Assert
		answer.X.Should().Be(.0125f);
		answer.Y.Should().Be(-.04f);
		answer.Z.Should().Be(.005f);
	}

	[TestMethod]
	public void OperatorDivision_ThrowsDivideByZeroException_WhenSecondVector3IntHasZeroX_WhenDividingVector3ByVector3Int()
	{
		// Arrange
		Vector3Int vector3Int = new(0, 100, 100);
		Vector3 vector3 = new(1.25f, -4f, 0.5f);
		
		// Act
		Action act = () =>
		{
			Vector3 _ = vector3 / vector3Int;
		};

		// Assert
		act.Should().ThrowExactly<DivideByZeroException>();
	}

	[TestMethod]
	public void OperatorDivision_ThrowsDivideByZeroException_WhenSecondVector3IntHasZeroY_WhenDividingVector3ByVector3Int()
	{
		// Arrange
		Vector3Int vector3Int = new(100, 0, 100);
		Vector3 vector3 = new(1.25f, -4f, 0.5f);
		
		// Act
		Action act = () =>
		{
			Vector3 _ = vector3 / vector3Int;
		};

		// Assert
		act.Should().ThrowExactly<DivideByZeroException>();
	}

	[TestMethod]
	public void OperatorDivision_ThrowsDivideByZeroException_WhenSecondVector3IntHasZeroZ_WhenDividingVector3ByVector3Int()
	{
		// Arrange
		Vector3Int vector3Int = new(100, 100, 0);
		Vector3 vector3 = new(1.25f, -4f, 0.5f);
		
		// Act
		Action act = () =>
		{
			Vector3 _ = vector3 / vector3Int;
		};

		// Assert
		act.Should().ThrowExactly<DivideByZeroException>();
	}

	[TestMethod]
	public void OperatorDivision_DoesNotThrowDivideByZeroException_WhenSecondVector3IntDoesNotContainZero_WhenDividingVector3ByVector3Int()
	{
		// Arrange
		Vector3Int vector3Int = new(100, 100, 100);
		Vector3 vector3 = new(1.25f, -4f, 0.5f);
		
		// Act
		Action act = () =>
		{
			Vector3 _ = vector3 / vector3Int;
		};

		// Assert
		act.Should().NotThrow<DivideByZeroException>();
	}

	[TestMethod]
	public void OperatorDivision_ReturnsCorrectValue_WhenUsingScaleInt()
	{
		// Arrange
		Vector3Int vector = new(5, 10, 15);
		
		// Act
		Vector3 scaledVector = vector / 10;
		
		// Assert
		scaledVector.X.Should().Be(.5f);
		scaledVector.Y.Should().Be(1f);
		scaledVector.Z.Should().Be(1.5f);
	}

	[TestMethod]
	public void OperatorDivision_ReturnsCorrectVector3_WhenUsingScaleFloat()
	{
		// Arrange
		Vector3Int vector3Int = new(3, 9, 27);
		
		// Act
		Vector3 scaledVector = vector3Int / 1.5f;
		
		// Assert
		scaledVector.X.Should().Be(2f);
		scaledVector.Y.Should().Be(6f);
		scaledVector.Z.Should().Be(18f);
	}

	[TestMethod]
	public void OperatorDivision_ThrowsDivideByZeroException_WhenPassingIntScaleAsZero()
	{
		// Arrange
		Vector3Int vector3Int = new(5, 10, 15);
		
		// Act
		Action act = () =>
		{
			Vector3 _ = vector3Int / 0;
		};

		// Assert
		act.Should().ThrowExactly<DivideByZeroException>();
	}

	[TestMethod]
	public void OperatorDivision_DoesNotThrowDivideByZeroException_WhenPassingIntScaleAsNonZero()
	{
		// Arrange
		Vector3Int vector3Int = new(5, 10, 15);
		
		// Act
		Action act = () =>
		{
			Vector3 _ = vector3Int / 1;
		};

		// Assert
		act.Should().NotThrow<DivideByZeroException>();
	}

	[TestMethod]
	public void OperatorDivision_ThrowsDivideByZeroException_WhenPassingFloatScaleAsZero()
	{
		// Arrange
		Vector3Int vector3Int = new(5, 10, 15);
		
		// Act
		Action act = () =>
		{
			Vector3 _ = vector3Int / 0f;
		};

		// Assert
		act.Should().ThrowExactly<DivideByZeroException>();
	}

	[TestMethod]
	public void OperatorDivision_DoesNotThrowDivideByZeroException_WhenPassingFloatScaleAsNonZero()
	{
		// Arrange
		Vector3Int vector3Int = new(5, 10, 15);
		
		// Act
		Action act = () =>
		{
			Vector3 _ = vector3Int / .0001f;
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
		Vector3Int vector = new(5, 10, 15);
		
		// Act
		// ReSharper disable once EqualExpressionComparison
		bool isEqual = vector == vector;
		
		// Assert
		isEqual.Should().BeTrue();
	}

	[TestMethod]
	public void OperatorEqualsEquals_ReturnsTrue_WhenComparingTwoVectorsWithSameValues()
	{
		// Arrange
		Vector3Int vectorA = new(5, 10, 15);
		Vector3Int vectorB = new(5, 10, 15);
		
		// Act
		bool isEqual = vectorA == vectorB;
		
		// Assert
		isEqual.Should().BeTrue();
	}
	
	[TestMethod]
	public void OperatorEqualsEquals_ReturnsFalse_WhenComparingTwoVectorsWithDifferentValues()
	{
		// Arrange
		Vector3Int vectorA = new(5, 10, 15);
		Vector3Int vectorB = new(98, 1, 43);
		
		// Act
		bool isEqual = vectorA == vectorB;
		
		// Assert
		isEqual.Should().BeFalse();
	}

	[TestMethod]
	public void OperatorEqualsEquals_ReturnsFalse_WhenComparingTwoVectorsWithSameValuesInDifferentPositions()
	{
		// Arrange
		Vector3Int vectorA = new(15, 5, 10);
		Vector3Int vectorB = new(5, 10, 15);
		
		// Act
		bool isEqual = vectorA == vectorB;
		
		// Assert
		isEqual.Should().BeFalse();
	}

	#endregion

	#region Operator != Tests

	[TestMethod]
	public void OperatorNotEquals_ReturnsFalse_WhenComparingTheSameObject()
	{
		// Arrange
		Vector3Int vector = new(5, 10, 15);
		
		// Act
		// ReSharper disable once EqualExpressionComparison
		bool isNotEqual = vector != vector;
		
		// Assert
		isNotEqual.Should().BeFalse();
	}

	[TestMethod]
	public void OperatorNotEquals_ReturnsFalse_WhenComparingTwoVectorsWithSameValues()
	{
		// Arrange
		Vector3Int vectorA = new(5, 10, 15);
		Vector3Int vectorB = new(5, 10, 15);
		
		// Act
		bool isNotEqual = vectorA != vectorB;
		
		// Assert
		isNotEqual.Should().BeFalse();
	}
	
	[TestMethod]
	public void OperatorNotEquals_ReturnsTrue_WhenComparingTwoVectorsWithDifferentValues()
	{
		// Arrange
		Vector3Int vectorA = new(5, 10, 15);
		Vector3Int vectorB = new(98, 1, 43);
		
		// Act
		bool isNotEqual = vectorA != vectorB;
		
		// Assert
		isNotEqual.Should().BeTrue();
	}

	[TestMethod]
	public void OperatorNotEquals_ReturnsTrue_WhenComparingTwoVectorsWithSameValuesInDifferentPositions()
	{
		// Arrange
		Vector3Int vectorA = new(15, 5, 10);
		Vector3Int vectorB = new(5, 10, 15);
		
		// Act
		bool isNotEqual = vectorA != vectorB;
		
		// Assert
		isNotEqual.Should().BeTrue();
	}

	#endregion

	#region Equals Tests

	[TestMethod]
	public void Equals_ReturnsTrue_WhenComparingTheSameObject()
	{
		// Arrange
		Vector3Int vector = new(5, 10, 15);
		
		// Act
		// ReSharper disable once EqualExpressionComparison
		bool isEqual = vector.Equals(vector);
		
		// Assert
		isEqual.Should().BeTrue();
	}

	[TestMethod]
	public void Equals_ReturnsTrue_WhenComparingTwoVectorsWithSameValues()
	{
		// Arrange
		Vector3Int vectorA = new(5, 10, 15);
		Vector3Int vectorB = new(5, 10, 15);
		
		// Act
		bool isEqual = vectorA.Equals(vectorB);
		
		// Assert
		isEqual.Should().BeTrue();
	}
	
	[TestMethod]
	public void Equals_ReturnsFalse_WhenComparingTwoVectorsWithDifferentValues()
	{
		// Arrange
		Vector3Int vectorA = new(5, 10, 15);
		Vector3Int vectorB = new(98, 1, 43);
		
		// Act
		bool isEqual = vectorA.Equals(vectorB);
		
		// Assert
		isEqual.Should().BeFalse();
	}

	[TestMethod]
	public void Equals_ReturnsFalse_WhenComparingTwoVectorsWithSameValuesInDifferentPositions()
	{
		// Arrange
		Vector3Int vectorA = new(15, 5, 10);
		Vector3Int vectorB = new(5, 10, 15);
		
		// Act
		bool isEqual = vectorA.Equals(vectorB);
		
		// Assert
		isEqual.Should().BeFalse();
	}

	[TestMethod]
	public void Equals_ReturnsFalse_WhenComparingToVector3Object_WithSameValues()
	{
		// Arrange
		Vector3Int vectorA = new(5, 10, 15);
		Vector3 vectorB = new(5, 10, 15);
		
		// Act
		// ReSharper disable once SuspiciousTypeConversion.Global
		bool isEqual = vectorA.Equals(vectorB);
		
		// Assert
		isEqual.Should().BeFalse();
	}

	#endregion

	#region GetHashCode Tests

	[TestMethod]
	public void GetHashCode_ReturnsSameValue_FromTwoEqualVectors()
	{
		// Arrange
		Vector3Int vectorA = new(5, 10, 15);
		Vector3Int vectorB = new(5, 10, 15);
		
		// Act
		int hashA = vectorA.GetHashCode();
		int hashB = vectorB.GetHashCode();
		
		// Assert
		hashA.Should().Be(hashB);
	}

	[TestMethod]
	public void GetHashCode_DoesNotReturnSameValue_FromTwoDifferentVectors()
	{
		// Arrange
		Vector3Int vectorA = new(76, 25, 18);
		Vector3Int vectorB = new(5, 10, 15);
		
		// Act
		int hashA = vectorA.GetHashCode();
		int hashB = vectorB.GetHashCode();
		
		// Assert
		hashA.Should().NotBe(hashB);
	}

	[TestMethod]
	public void GetHashCode_DoesNotReturnSameValue_FromTwoVectorsWithSameValuesInDifferentSpots()
	{
		// Arrange
		Vector3Int vectorA = new(10, 15, 5);
		Vector3Int vectorB = new(5, 10, 15);
		
		// Act
		int hashA = vectorA.GetHashCode();
		int hashB = vectorB.GetHashCode();
		
		// Assert
		hashA.Should().NotBe(hashB);
	}

	#endregion

	#region ToString Tests

	[TestMethod]
	public void ToString_ReturnsStringContainingXYAndZValues()
	{
		// Arrange
		Vector3Int vector3Int = new(5, 7, 9); // Using different numbers for each
		
		// Act
		string vector3IntString = vector3Int.ToString();
		
		// Assert
		vector3IntString.Should().ContainAll(new List<string> {vector3Int.X.ToString(), vector3Int.Y.ToString(), vector3Int.Z.ToString()});
	}

	#endregion

	#region Casting from Vector3Int to Vector3 Tests

	[TestMethod]
	public void CastVector3_ReturnsCorrectValue()
	{
		// Arrange
		Vector3Int vector3Int = new(5, 5, 5);
		
		// Act
		Vector3 vector3 = (Vector3) vector3Int;
		
		// Assert
		vector3.Should().Be(new Vector3(5, 5, 5));
	}

	#endregion

	#region Casting from Vector3 to Vector3Int Tests

	[TestMethod]
	public void CastVector3Int_ReturnsCorrectValue()
	{
		// Arrange
		Vector3 vector3 = new(5.7f, -1.2f, 0.7f);
		
		// Act
		Vector3Int vector3Int = (Vector3Int) vector3;
		
		// Assert
		vector3Int.Should().Be(new Vector3Int(5, -1, 0));
	}

	#endregion
}