using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.UIElements;
using Microsoft.Xna.Framework;

namespace Andavies.MonoGame.UI.LayoutGroups;

public class VerticalLayoutGroup : ILayoutGroup
{
	private readonly List<UIElement> _uiElements = new();

	private int _spacing = 50;
	private LayoutAnchor _layoutAnchor = LayoutAnchor.TopLeft;
	private Point _position = Point.Zero;

	public int Spacing
	{
		get => _spacing;
		set
		{
			_spacing = value;
			Restructure();
		}
	}

	public LayoutAnchor LayoutAnchor
	{
		get => _layoutAnchor;
		set
		{
			_layoutAnchor = value;
			Restructure();
		}
	}
	
	public Point Position
	{
		get => _position;
		set
		{
			_position = value;
			Restructure();
		}
	}

	public void AddUIElements(params UIElement[] uiElements)
	{
		foreach (UIElement uiElement in uiElements)
		{
			_uiElements.Add(uiElement);
		}
		Restructure();
	}

	private void Restructure()
	{
		for (int index = 0; index < _uiElements.Count; index++)
		{
			UIElement uiElement = _uiElements[index];
			uiElement.LayoutAnchor = LayoutAnchor;

			Point position = LayoutAnchor switch
			{
				LayoutAnchor.TopLeft => new Point(Position.X, Position.Y + index * Spacing),
				LayoutAnchor.TopCenter => new Point(Position.X, Position.Y + index * Spacing),
				LayoutAnchor.TopRight => new Point(Position.X, Position.Y + index * Spacing),
				LayoutAnchor.MiddleLeft => new Point(Position.X),
				LayoutAnchor.MiddleCenter => new Point(Position.X),
				LayoutAnchor.MiddleRight => new Point(Position.X),
				LayoutAnchor.BottomLeft => new Point(Position.X),
				LayoutAnchor.BottomCenter => new Point(Position.X),
				LayoutAnchor.BottomRight => new Point(Position.X),
				_ => throw new ArgumentOutOfRangeException()
			};
			
			uiElement.Position = position;
		}
	}
}