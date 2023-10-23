using Andavies.MonoGame.UI.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.MonoGame.UI.UIElements;

public class Label : UIElement
{
	public string Text { get; set; } = "N/A"; // Text that is displayed on the label
	public LabelStyle? Style { get; set; } // How the label will be displayed
	
	public override void Draw(SpriteBatch spriteBatch)
	{
		if (Style == null)
			return;
		
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