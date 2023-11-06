using Andavies.MonoGame.UI.Core;
using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.Styles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.MonoGame.UI.UIElements;

public class Label : UIElement
{
	public Label(Point position, Point size, string text, LabelStyle style) : base(position, size)
	{
		Text = text;
		Style = style;
	}
	
	/// <summary>
	/// The text that will be displayed on the label
	/// </summary>
	public string Text { get; set; }
	
	/// <summary>
	/// Defines how the label will be drawn
	/// </summary>
	public LabelStyle Style { get; set; }
	
	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Style.BackgroundTexture, Bounds, Style.BackgroundColor);

		Vector2 textSize = Style.Font?.MeasureString(Text) ?? Vector2.Zero;
		float verticalCenter = Bounds.Top + (Bounds.Height - textSize.Y) / 2;
		Vector2 textPosition = Style.TextAlignment switch
		{
			TextAlignment.Left => new Vector2(Bounds.Left, verticalCenter),
			TextAlignment.Center => new Vector2(Bounds.Left + (Bounds.Width - textSize.X)/2, verticalCenter),
			TextAlignment.Right => new Vector2(Bounds.Right - textSize.X, verticalCenter),
			_ => new Vector2(Bounds.Left, verticalCenter)
		};
		spriteBatch.DrawString(Style.Font, Text, textPosition, Color.Black);
	}
}