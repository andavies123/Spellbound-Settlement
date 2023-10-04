using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameUI;

public class Button : UIElement
{
	public Button(Point position, Point size, string text) : base(position, size)
	{
		Text = text;
	}

	/// <summary>
	/// The text that will be displayed on the button
	/// </summary>
	public string Text { get; set; }
	
	/// <summary>
	/// The font <see cref="Text"/> will be displayed with
	/// </summary>
	public SpriteFont Font { get; set; }
	
	/// <summary>
	/// The alignment <see cref="Text"/> will be displayed with
	/// </summary>
	public TextAlignment TextAlignment { get; set; } = TextAlignment.Center;
	
	/// <summary>
	/// True if this button can be clicked by the user
	/// False if this button can not be clicked by the user
	/// </summary>
	public bool IsClickable { get; set; } = true;
	
	public Color HoverBackgroundColor { get; set; }
	public Color MousePressedBackgroundColor { get; set; }

	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(BackgroundTexture, Bounds, GetCurrentBackgroundColor());

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

	private Color GetCurrentBackgroundColor()
	{
		Color color;
		if (IsElementPressed)
			color = MousePressedBackgroundColor;
		else if (IsMouseInside)
			color = HoverBackgroundColor;
		else
			color = BackgroundColor;
		return color;
	}
}