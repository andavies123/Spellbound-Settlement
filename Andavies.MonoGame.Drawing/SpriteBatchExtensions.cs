using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.MonoGame.Drawing;

public static class SpriteBatchExtensions
{
	private static Texture2D? s_PixelTexture;

	/// <summary>Initializes the pixel texture used in drawing for this class. Must be called before this class can be used</summary>
	/// <param name="spriteBatch">SpriteBatch object used for drawing</param>
	public static void InitializePixelTexture(SpriteBatch spriteBatch)
	{
		s_PixelTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
		s_PixelTexture.SetData(new[]{ Color.White });
	}

	/// <summary>Overload method that will set the internal pixel texture with a given one if one is already in use</summary>
	/// <param name="pixelTexture">The pixel texture to be stored internally</param>
	public static void InitializePixelTexture(Texture2D pixelTexture)
	{
		s_PixelTexture = pixelTexture;
	}
	
	/// <summary>Draws a line between 2 given points</summary>
	/// <param name="spriteBatch">Extended SpriteBatch object</param>
	/// <param name="point1">The first point of the line</param>
	/// <param name="point2">The second point of the line</param>
	/// <param name="color">The color of the line</param>
	/// <param name="thickness">How thick the line will be</param>
	public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, float thickness)
	{
		// calculate the distance between the two vectors
		float distance = Vector2.Distance(point1, point2);

		// calculate the angle between the two vectors
		float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);

		spriteBatch.DrawLine(point1, angle, distance, color, thickness);
	}
	
	/// <summary>Draws a line using a given point, angle, and length</summary>
	/// <param name="spriteBatch">Extended SpriteBatch object</param>
	/// <param name="point">The starting point of the line</param>
	/// <param name="angle">The angle in radians the line will be pointed at from <paramref name="point"/>.Pi/2 = straight down</param>
	/// <param name="length">How long the line will be</param>
	/// <param name="color">The color of the line</param>
	/// <param name="thickness">How thick the line will be</param>
	public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point, float angle, float length, Color color, float thickness)
	{
		spriteBatch.Draw(s_PixelTexture, point, null, color, angle, 
			Vector2.Zero, new Vector2(length, thickness), SpriteEffects.None, 0);
	}
	
	/// <summary>Draws a rectangle from a given rectangle object</summary>
	/// <param name="spriteBatch">Extended SpriteBatch object</param>
	/// <param name="rect">The rectangle object that will be drawn</param>
	/// <param name="color">The outline color of the rectangle</param>
	/// <param name="thickness">The thickness of the outline of the rectangle</param>
	public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rect, Color color, float thickness)
	{
		DrawLine(spriteBatch, new Vector2(rect.X, rect.Y), new Vector2(rect.Right, rect.Y), color, thickness); // top
		DrawLine(spriteBatch, new Vector2(rect.X + 1f, rect.Y), new Vector2(rect.X + 1f, rect.Bottom + thickness), color, thickness); // left
		DrawLine(spriteBatch, new Vector2(rect.X, rect.Bottom), new Vector2(rect.Right, rect.Bottom), color, thickness); // bottom
		DrawLine(spriteBatch, new Vector2(rect.Right + 1f, rect.Y), new Vector2(rect.Right + 1f, rect.Bottom + thickness), color, thickness); // right
	}
}