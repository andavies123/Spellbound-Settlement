using System.Collections;

namespace Andavies.MonoGame.Utilities.Extensions;

public static class ListExtensions
{
	/// <summary>
	/// Checks if this list is empty or not.
	/// </summary>
	/// <param name="list">The extended list that will be checked if it's empty or not</param>
	/// <returns>True when the list is empty and does not contain any items. False otherwise</returns>
	public static bool IsEmpty(this IList list)
	{
		return list.Count == 0;
	}
}