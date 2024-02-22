using Microsoft.Xna.Framework;

namespace Andavies.MonoGame.Utilities.Interfaces;

public interface IUpdateable
{
	/// <summary>
	/// True if this instance should be updated
	/// False if this instance should not be updated
	/// </summary>
	bool Enabled { get; set; }
	
	/// <summary>
	/// The order at which this instance should be updated
	/// Small numbers come first
	/// Larger numbers are updated last
	/// </summary>
	int UpdateOrder { get; }

	/// <summary>
	/// Called once per frame
	/// </summary>
	/// <param name="gameTime">Current game time for this frame</param>
	void Update(GameTime gameTime);
}