using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.UIElements;
using Microsoft.Xna.Framework;

namespace Andavies.MonoGame.UI.LayoutGroups;

public class HorizontalLayoutGroup : ILayoutGroup
{
	private readonly List<UIElement> _childrenUIElements = new();
	
	public bool ForceExpandChildHeight { get; set; } = true;
	public int Spacing { get; set; } = 50;
	public Rectangle Bounds { get; set; } = Rectangle.Empty;
	public VerticalAnchor ChildAnchor { get; set; } = VerticalAnchor.Center;

	public void AddUIElements(params UIElement[] uiElements)
	{
		foreach (UIElement uiElement in uiElements)
		{
			_childrenUIElements.Add(uiElement);
		}
		RecalculateChildElements();
	}

	public void RecalculateChildElements()
	{
		// Calculate how left the ui elements position starts at index 0
		float startX = Bounds.Center.X - (_childrenUIElements.Count - 1) * Spacing / 2f;
		
		// Loop through each of the children UI elements and set their position
		for (int index = 0; index < _childrenUIElements.Count; index++)
		{
			UIElement child = _childrenUIElements[index];

			if (ForceExpandChildHeight)
				child.Size = new Point(child.Size.X, Bounds.Height);

			Point position = ChildAnchor switch
			{
				VerticalAnchor.Top => new Point((int)startX + index * Spacing, Bounds.Top),
				VerticalAnchor.Center => new Point((int)startX + index * Spacing, Bounds.Center.Y - child.Size.Y / 2),
				VerticalAnchor.Bottom => new Point((int)startX + index * Spacing, Bounds.Bottom - child.Size.Y),
				_ => throw new ArgumentOutOfRangeException()
			};

			// Todo: Is this anchor really needed anymore?
			LayoutAnchor anchor = ChildAnchor switch
			{
				VerticalAnchor.Top => LayoutAnchor.MiddleLeft,
				VerticalAnchor.Center => LayoutAnchor.MiddleCenter,
				VerticalAnchor.Bottom => LayoutAnchor.MiddleRight,
				_ => throw new ArgumentOutOfRangeException()
			};
			
			child.LayoutAnchor = anchor;
			child.Position = position;
		}
	}
}