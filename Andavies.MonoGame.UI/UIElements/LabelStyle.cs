using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.Styles;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.MonoGame.UI.UIElements;

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