using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.UIElements;
using Microsoft.Xna.Framework;

namespace Andavies.MonoGame.UI.LayoutGroups;

public interface ILayoutGroup
{
	/// <summary>The spacing between UI Elements</summary>
	int Spacing { get; set; }
	
	/// <summary>Where on the screen this layout group is anchored</summary>
	LayoutAnchor LayoutAnchor { get; set; }

	/// <summary>The position of this layout group based on the LayoutAnchor</summary>
	Point Position { get; set; }

	/// <summary>Adds UI elements to the end of this layout group</summary>
	void AddUIElements(params UIElement[] uiElements);
}