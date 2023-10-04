using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UI.Styles;

public abstract class UIElementStyle
{
	/// <summary>
	/// The background color that will be displayed by default combined with <see cref="BackgroundTexture"/>
	/// </summary>
	public Color BackgroundColor { get; set; } = Color.Black;
	
	/// <summary>
	/// The texture to use for the background combined with <see cref="BackgroundColor"/>
	/// </summary>
	public Texture2D? BackgroundTexture { get; set; }
}