using GameUI.Styles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameUI;

public class Button : UIElement
{
	public Button(Point position, Point size, string text, ButtonStyle buttonStyle, LayoutAnchor anchor) : base(position, size, anchor)
	{
		Text = text;
		Style = buttonStyle;
	}

	/// <summary>
	/// The text that will be displayed on the button
	/// </summary>
	public string Text { get; set; }
	
	/// <summary>
	/// Contains properties to help display the button
	/// </summary>
	public ButtonStyle Style { get; }
	
	/// <summary>
	/// True if this button can be clicked by the user
	/// False if this button can not be clicked by the user
	/// </summary>
	public bool IsEnabled { get; set; } = true;

	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Style.BackgroundTexture, Bounds, GetCurrentBackgroundColor());

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

	private Color GetCurrentBackgroundColor()
	{
		Color color;
		
		if (!IsEnabled)
			color = Style.DisabledBackgroundColor;
		else if (IsElementPressed)
			color = Style.MousePressedBackgroundColor;
		else if (IsMouseInside)
			color = Style.HoverBackgroundColor;
		else
			color = Style.BackgroundColor;
		
		return color;
	}
}