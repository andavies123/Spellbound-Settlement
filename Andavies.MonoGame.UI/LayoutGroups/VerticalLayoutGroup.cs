using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.UIElements;
using Microsoft.Xna.Framework;

namespace Andavies.MonoGame.UI.LayoutGroups;

public class VerticalLayoutGroup : ILayoutGroup
{
	private readonly List<UIElement> _childrenUIElements = new();
	
	public bool ForceExpandChildWidth { get; set; } = true;
	public int Spacing { get; set; } = 50;
	public Rectangle Bounds { get; set; } = Rectangle.Empty;
	public HorizontalAnchor ChildAnchor { get; set; } = HorizontalAnchor.Center;

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
		// Calculate how high up the ui elements position starts at index 0
		float startY = Bounds.Center.Y - (_childrenUIElements.Count - 1) * Spacing / 2f;
		
		// Loop through each of the children UI elements and set their position
		for (int index = 0; index < _childrenUIElements.Count; index++)
		{
			UIElement child = _childrenUIElements[index];

			if (ForceExpandChildWidth)
				child.Size = new Point(Bounds.Width, child.Size.Y);

			Point position = ChildAnchor switch
			{
				HorizontalAnchor.Left => new Point(Bounds.Left, (int)startY + index * Spacing),
				HorizontalAnchor.Center => new Point(Bounds.Center.X - (child.Size.X / 2), (int)startY + index * Spacing),
				HorizontalAnchor.Right => new Point(Bounds.Right - child.Size.X, (int)startY + index * Spacing),
				_ => throw new ArgumentOutOfRangeException()
			};

			LayoutAnchor anchor = ChildAnchor switch
			{
				HorizontalAnchor.Left => LayoutAnchor.MiddleLeft,
				HorizontalAnchor.Center => LayoutAnchor.MiddleCenter,
				HorizontalAnchor.Right => LayoutAnchor.MiddleRight,
				_ => throw new ArgumentOutOfRangeException()
			};
			
			child.LayoutAnchor = anchor;
			child.Position = position;
		}
	}
}