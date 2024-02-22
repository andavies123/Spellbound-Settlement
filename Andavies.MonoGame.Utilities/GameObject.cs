using Microsoft.Xna.Framework;

namespace Andavies.MonoGame.Utilities;

public abstract class GameObject : IGameObject
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

public interface IGameObject
{
	/// <summary>
	/// The order at which this instance should be initialized
	/// Small numbers come first
	/// Larger numbers are updated last
	/// </summary>
	int InitOrder { get; }
	
	/// <summary>
	/// The order at which this instance should be updated
	/// Small numbers come first
	/// Larger numbers are updated last
	/// </summary>
	int UpdateOrder { get; }
	
	/// <summary>
	/// Flag to determine whether this instance will be updated.
	/// Instance will still be initialized no matter the value
	/// True = Will be updated
	/// False = Will not be updated
	/// </summary>
	bool Enabled { get; set; }
	
	/// <summary>
	/// Called once at the beginning of the game BEFORE any content has been loaded
	/// </summary>
	void Init();
	
	/// <summary>
	/// Called once at the beginning of the game AFTER the content has been loaded
	/// </summary>
	void LateInit();
	
	/// <summary>
	/// Called once per frame
	/// </summary>
	/// <param name="gameTime">GameTime object for this last frame</param>
	void Update(GameTime gameTime);
}