using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameUI.Styles;

public class ButtonStyle : UIElementStyle
{
	/// <summary>
	/// The font the button text will be displayed with
	/// </summary>
	public SpriteFont? Font { get; set; }

	/// <summary>
	/// The alignment the button's text will be displayed with
	/// </summary>
	public TextAlignment TextAlignment { get; set; } = TextAlignment.Center;
	
	/// <summary>
	/// The background color of the button when the mouse is hovering over the button
	/// </summary>
	public Color HoverBackgroundColor { get; set; }
	
	/// <summary>
	/// The background color of the button when the mouse is pressed on the button
	/// </summary>
	public Color MousePressedBackgroundColor { get; set; }
}