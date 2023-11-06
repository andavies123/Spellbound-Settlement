using Andavies.MonoGame.UI.Interfaces;

namespace Andavies.MonoGame.UI.LayoutGroups;

public interface ILayoutGroup
{
	/// <summary> The spacing between child UI Elements </summary>
	int Spacing { get; set; }

	/// <summary> Adds child UI elements to the end of this layout group </summary>
	void AddChildren(params IUIElement[] children);
	
	/// <summary> Recalculates the bounds of each child element in this layout group </summary>
	void RecalculateChildrenBounds();
}