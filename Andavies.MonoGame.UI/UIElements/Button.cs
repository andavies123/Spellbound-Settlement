using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.Styles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.MonoGame.UI.UIElements;

public class Button : UIElement
{
	// Text that is displayed on the button
	public string Text { get; set; } = "N/A";
	
	// How the button will be displayed
	public ButtonStyle? Style { get; set; }
	
	// True = Button can be clicked. False = Can't be clicked
	public bool IsEnabled { get; set; } = true;

	public override void Draw(SpriteBatch spriteBatch)
	{
		if (Style == null)
			return;
		
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
		if (Style == null)
			return Color.Black;
		
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