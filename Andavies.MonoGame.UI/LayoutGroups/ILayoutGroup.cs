using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.UIElements;
using Microsoft.Xna.Framework;

namespace Andavies.MonoGame.UI.LayoutGroups;

public interface ILayoutGroup
{
	/// <summary> The spacing between UI Elements </summary>
	int Spacing { get; set; }

	/// <summary> The rectangular bounds of this layout group based on the LayoutAnchor </summary>
	Rectangle Bounds { get; set; }

	/// <summary> Adds UI elements to the end of this layout group </summary>
	void AddUIElements(params UIElement[] uiElements);
	
	/// <summary> Recalculates the bounds of each child element in this layout group </summary>
	void RecalculateChildElements();
}