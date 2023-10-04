using GameUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpellboundSettlement.UIStates;

public class GameplayUIState : IUIState
{
	private readonly Button _pauseButton;

	public GameplayUIState()
	{
		_pauseButton = new Button(new Point(-20, 20), new Point(75, 25), "PAUSE")
		{
			BackgroundColor = Color.LightSlateGray,
			HoverBackgroundColor = Color.SlateGray,
			MousePressedBackgroundColor = Color.DarkSlateGray,
			BackgroundTexture = GameManager.Texture,
			Font = GameManager.Font,
			LayoutAnchor = LayoutAnchor.TopRight
		};
		
		_pauseButton.CalculateBounds(GameManager.Viewport.Bounds.Size);
	}

	public void Update()
	{
		_pauseButton.Update();
	}
    
	public void Draw(SpriteBatch spriteBatch)
	{
		_pauseButton.Draw(spriteBatch);
	}
}