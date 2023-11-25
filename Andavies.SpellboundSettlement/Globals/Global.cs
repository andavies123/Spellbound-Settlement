using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.SpellboundSettlement.Globals;

public static class Global
{
	public static GameManager GameManager { get; set; }
	public static GraphicsDeviceManager GraphicsDeviceManager { get; set; }
	public static SpriteBatch SpriteBatch { get; set; }
	
	public static void QuitGame() => GameManager.Exit();
}