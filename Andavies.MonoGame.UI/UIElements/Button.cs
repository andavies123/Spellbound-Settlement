using Andavies.MonoGame.UI.Core;
using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.Styles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.MonoGame.UI.UIElements;

/// <summary>
/// Class that defines and handles an instance of a UI button
/// </summary>
public class Button : UIElement
{
	public Button(Point position, Point size, string text, ButtonStyle style) : base(position, size)
	{
		Text = text;
		Style = style;
	}

	public Button(Point size, string text, ButtonStyle style) : base(size)
	{
		Text = text;
		Style = style;
	}
	
	/// <summary>
	/// The text that will be displayed on the button
	/// </summary>
	public string Text { get; set; }
	
	/// <summary>
	/// The style components of the button that defines how it will look when drawn
	/// </summary>
	public ButtonStyle Style { get; set; }

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
		
		if (!IsInteractable)
			color = Style.DisabledBackgroundColor;
		else if (IsElementPressed)
			color = Style.MousePressedBackgroundColor;
		else if (IsElementHovered)
			color = Style.HoverBackgroundColor;
		else
			color = Style.BackgroundColor;
		
		return color;
	}
}