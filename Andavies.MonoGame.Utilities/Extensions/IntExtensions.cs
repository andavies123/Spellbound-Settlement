namespace Andavies.MonoGame.Utilities.Extensions;

public static class IntExtensions
{
	/// <summary>
	/// Checks whether or not this integer is a valid array index in a given array
	/// </summary>
	/// <param name="arrayIndex">The extended int used as the array index</param>
	/// <param name="array">The array that would be indexed</param>
	/// <returns>True if this int is a valid index. False otherwise</returns>
	public static bool IsValidArrayIndex<T>(this int arrayIndex,  T[] array)
	{
		return arrayIndex >= 0 && arrayIndex < array.Length;
	}
}