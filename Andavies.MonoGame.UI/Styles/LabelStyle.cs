using Andavies.MonoGame.UI.Enums;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.MonoGame.UI.Styles;

public class LabelStyle : UIElementStyle
{
	/// <summary>
	/// The font the button text will be displayed with
	/// </summary>
	public SpriteFont? Font { get; set; }

	/// <summary>
	/// The alignment the button's text will be displayed with
	/// </summary>
	public TextAlignment TextAlignment { get; set; } = TextAlignment.Center;
}