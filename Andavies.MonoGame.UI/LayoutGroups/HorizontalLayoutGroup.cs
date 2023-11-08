using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.Interfaces;
using Microsoft.Xna.Framework;

namespace Andavies.MonoGame.UI.LayoutGroups;

public class HorizontalLayoutGroup : LayoutGroup
{
	public HorizontalLayoutGroup(Rectangle bounds) : base(bounds) { }
	public HorizontalLayoutGroup(Point location, Point size) : base(location, size) { }
	public HorizontalLayoutGroup(Point size) : base(size) { }
	
	public bool ForceExpandChildHeight { get; set; } = true;
	public VerticalAnchor ChildAnchor { get; set; } = VerticalAnchor.Center;

	public override void RecalculateChildrenBounds()
	{
		// Calculate how left the ui elements position starts at index 0
		float startX = Bounds.Center.X - (Children.Count - 1) * Spacing / 2f;
		
		// Loop through each of the children UI elements and set their position
		for (int index = 0; index < Children.Count; index++)
		{
			IUIElement child = Children[index];

			Point size = ForceExpandChildHeight ? new Point(child.Bounds.Width, Bounds.Height) : child.Bounds.Size;

			int childXPos = (int) startX - child.Width / 2 + index * Spacing;

			Point location = ChildAnchor switch
			{
				VerticalAnchor.Top => new Point(childXPos, Bounds.Top),
				VerticalAnchor.Center => new Point(childXPos, Bounds.Center.Y - child.Bounds.Height / 2),
				VerticalAnchor.Bottom => new Point(childXPos, Bounds.Bottom - child.Bounds.Height),
				_ => throw new ArgumentOutOfRangeException()
			};

			child.Bounds = new Rectangle(location, size);
		}
	}
}