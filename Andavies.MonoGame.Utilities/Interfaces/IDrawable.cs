using Microsoft.Xna.Framework.Graphics;

namespace Andavies.MonoGame.Utilities.Interfaces;

public interface IDrawable
{
	/// <summary>
	/// True when this instance should be drawn
	/// False when this instance shouldn't be drawn
	/// </summary>
	bool Visible { get; set; }
	
	/// <summary>
	/// The order at which this instance should be drawn
	/// Small numbers come first
	/// Larger numbers are drawn last
	/// </summary>
	int DrawOrder { get; }

	/// <summary>
	/// Called once per frame to draw 3d objects
	/// </summary>
	/// <param name="graphicsDevice"></param>
	void Draw3D(GraphicsDevice graphicsDevice);

	/// <summary>
	/// Called once per frame to draw UI objects
	/// </summary>
	/// <param name="spriteBatch"></param>
	void DrawUI(SpriteBatch spriteBatch);
}