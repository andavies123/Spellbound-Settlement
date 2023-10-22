using Andavies.MonoGame.UI.Enums;
using Microsoft.Xna.Framework;

namespace Andavies.MonoGame.UI.UIElements;

public abstract class UIElementBuilder<T> where T : UIElement, new()
{
	protected T UIElement = new();

	public UIElementBuilder<T> SetPositionAndSize(Point position, Point size)
	{
		UIElement.Position = position;
		UIElement.Size = size;
		return this;
	}

	public UIElementBuilder<T> SetLayoutAnchor(LayoutAnchor layoutAnchor)
	{
		UIElement.LayoutAnchor = layoutAnchor;
		return this;
	}

	public T Build()
	{
		T result = UIElement;
		UIElement = new T(); // Reset the builder
		return result;
	}
}