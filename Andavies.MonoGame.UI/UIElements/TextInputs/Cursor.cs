using Andavies.MonoGame.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.MonoGame.UI.UIElements.TextInputs;

public class Cursor
{
	private bool _blinkVisibility = true; // Is visible or not
	private float _blinkElapsedTime = 0f; // Time since last blink change
	
	/// <summary>True if the cursor should be visible despite of blinking. False if not.</summary>
	public bool IsVisible { get; set; } = true;
	/// <summary>The rate at which this cursor will blink. This is a half cycle. How long the cursor is visible.</summary>
	public float BlinkRate { get; set; } = 0.5f; // Seconds

	public void Update(float deltaTimeSeconds)
	{
		_blinkElapsedTime += deltaTimeSeconds;

		if (_blinkElapsedTime >= BlinkRate)
		{
			_blinkElapsedTime = 0f;
			_blinkVisibility = !_blinkVisibility;
		}
	}

	public void Draw(SpriteBatch spriteBatch, Vector2 topPoint, float height)
	{
		if (!IsVisible || !_blinkVisibility)
			return;
	
		spriteBatch.DrawLine(topPoint, (float) (Math.PI/2), height, Color.Black, 2);
	}
}