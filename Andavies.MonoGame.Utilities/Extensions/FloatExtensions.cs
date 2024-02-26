namespace Andavies.MonoGame.Utilities.Extensions;

public static class FloatExtensions
{
	/// <summary>
	/// Calculates and returns the digit in the tenths place (0.1)
	/// </summary>
	/// <param name="value">The number to find the tenths place in</param>
	/// <returns>An int from 0 to 9 that is in the tenths place of the given number</returns>
	public static int GetTenthsPlace(this float value) => (int) Math.Abs(value * 10 % 10);
	
	/// <summary>
	/// Calculates and returns the digit in the hundredths place (0.01)
	/// </summary>
	/// <param name="value">The number to find the hundredths place in</param>
	/// <returns>An int from 0 to 9 that is in the hundredths place of the given number</returns>
	public static int GetHundredthsPlace(this float value) => (int) Math.Abs(value * 100 % 10);
	
	/// <summary>
	/// Calculates and returns the digit in the thousandths place (0.001)
	/// </summary>
	/// <param name="value">The number to find the thousandths place in</param>
	/// <returns>An int from 0 to 9 that is in the thousandths place of the given number</returns>
	public static int GetThousandthsPlace(this float value) => (int) Math.Abs(value * 1000 % 10);
}