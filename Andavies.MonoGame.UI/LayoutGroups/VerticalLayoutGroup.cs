using Andavies.MonoGame.Inputs;
using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.Interfaces;
using Microsoft.Xna.Framework;

namespace Andavies.MonoGame.UI.LayoutGroups;

public class VerticalLayoutGroup : LayoutGroup
{
	public VerticalLayoutGroup(IInputManager inputManager, Rectangle bounds) : base(inputManager, bounds) { }
	public VerticalLayoutGroup(IInputManager inputManager, Point location, Point size) : base(inputManager, location, size) { }
	public VerticalLayoutGroup(IInputManager inputManager, Point size) : base(inputManager, size) { }
	
	public bool ForceExpandChildWidth { get; set; } = true;
	public HorizontalAnchor ChildAnchor { get; set; } = HorizontalAnchor.Center;

	public override void RecalculateChildrenBounds()
	{
		// Calculate how high up the ui elements position starts at index 0
		float startY = Bounds.Center.Y - (Children.Count - 1) * Spacing / 2f;
		
		// Loop through each of the children UI elements and set their position
		for (int index = 0; index < Children.Count; index++)
		{
			IUIElement child = Children[index];

			Point size = ForceExpandChildWidth ? new Point(Bounds.Width, child.Bounds.Height) : child.Bounds.Size;

			int childYPos = (int) startY - child.Height / 2 + index * Spacing;
			
			Point location = ChildAnchor switch
			{
				HorizontalAnchor.Left => new Point(Bounds.Left, childYPos),
				HorizontalAnchor.Center => new Point(Bounds.Center.X - child.Bounds.Width / 2, childYPos),
				HorizontalAnchor.Right => new Point(Bounds.Right - child.Bounds.Width, childYPos),
				_ => throw new ArgumentOutOfRangeException()
			};

			child.Bounds = new Rectangle(location, size);
		}
	}
}