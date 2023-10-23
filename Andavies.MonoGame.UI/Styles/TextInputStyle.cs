using Andavies.MonoGame.UI.Enums;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.MonoGame.UI.Styles;

public class TextInputStyle : UIElementStyle
{
	/// <summary> The font the button text will be displayed with </summary>
	public SpriteFont? Font { get; set; }
	
	/// <summary> The font that will be used for the hint text </summary>
	public SpriteFont? HintTextFont { get; set; }

	/// <summary> The alignment the button's text will be displayed with </summary>
	public TextAlignment TextAlignment { get; set; } = TextAlignment.Center;
}