using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameUI;

public class Button : UIElement
{
	public Button(Point position, Point size, string text) : base(position, size)
	{
		Text = text;
	}

	public string Text { get; set; }
	public SpriteFont Font { get; set; }
	public TextAlignment TextAlignment { get; set; } = TextAlignment.Center;
	
	public bool HasFocus { get; set; } = false;
	public bool IsClickable { get; set; } = true;

	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(BackgroundTexture, Bounds, BackgroundColor);

		Vector2 textSize = Font.MeasureString(Text);
		float verticalCenter = Bounds.Top + (Bounds.Height - textSize.Y) / 2;
		Vector2 textPosition = TextAlignment switch
		{
			TextAlignment.Left => new Vector2(Bounds.Left, verticalCenter),
			TextAlignment.Center => new Vector2(Bounds.Left + (Bounds.Width - textSize.X)/2, verticalCenter),
			TextAlignment.Right => new Vector2(Bounds.Right - textSize.X, verticalCenter),
			_ => new Vector2(Bounds.Left, verticalCenter)
		};
		spriteBatch.DrawString(Font, Text, textPosition, Color.Black);
	}
}