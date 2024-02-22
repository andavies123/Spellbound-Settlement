using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IDrawable = Andavies.MonoGame.Utilities.Interfaces.IDrawable;
using IUpdateable = Andavies.MonoGame.Utilities.Interfaces.IUpdateable;

namespace Andavies.MonoGame.Utilities;

public abstract class GameObject : IGameObject, IUpdateable
{
	protected GameObject(int initOrder, int updateOrder)
	{
		InitOrder = initOrder;
		UpdateOrder = updateOrder;
	}
	
	public int InitOrder { get; }
	public int UpdateOrder { get; }
	public bool Enabled { get; set; } = true;
	
	public virtual void Init() { }
	public virtual void LateInit() { }
	public virtual void Update(GameTime gameTime) { }
}

public abstract class DrawableGameObject : GameObject, IDrawable
{
	protected DrawableGameObject(int initOrder, int updateOrder, int drawOrder) : base(initOrder, updateOrder)
	{
		DrawOrder = drawOrder;
	}

	public int DrawOrder { get; }
	public bool Visible { get; set; } = true;
	
	public virtual void Draw3D(GraphicsDevice graphicsDevice) { }
	public virtual void DrawUI(SpriteBatch spriteBatch) { }
}

public interface IGameObject
{
	/// <summary>
	/// The order at which this instance should be initialized
	/// Small numbers come first
	/// Larger numbers are updated last
	/// </summary>
	int InitOrder { get; }
	
	/// <summary>
	/// Called once at the beginning of the game BEFORE any content has been loaded
	/// </summary>
	void Init();
	
	/// <summary>
	/// Called once at the beginning of the game AFTER the content has been loaded
	/// </summary>
	void LateInit();
}