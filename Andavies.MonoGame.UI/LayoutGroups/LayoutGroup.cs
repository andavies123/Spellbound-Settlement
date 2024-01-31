using Andavies.MonoGame.Inputs;
using Andavies.MonoGame.UI.Core;
using Andavies.MonoGame.UI.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.MonoGame.UI.LayoutGroups;

public abstract class LayoutGroup : UIElement, ILayoutGroup
{
	protected readonly List<IUIElement> Children = new();

	protected LayoutGroup(IInputManager inputManager, Rectangle bounds) : base(inputManager, bounds) { }
	protected LayoutGroup(IInputManager inputManager, Point location, Point size) : base(inputManager, location, size) { }
	protected LayoutGroup(IInputManager inputManager, Point size) : base(inputManager, size) { }
	
	public int Spacing { get; set; }
	
	public override void Update(float deltaTimeSeconds)
	{
		Children.ForEach(child => child.Update(deltaTimeSeconds));
		
		base.Update(deltaTimeSeconds); // Mouse events
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);
		Children.ForEach(child => child.Draw(spriteBatch));
	}
	
	public void AddChildren(params IUIElement[] children)
	{
		foreach (IUIElement child in children)
		{
			Children.Add(child);
		}
		RecalculateChildrenBounds();
	}

	protected override void OnBoundsChanged()
	{
		RecalculateChildrenBounds();
	}

	public abstract void RecalculateChildrenBounds();
}