using GameUI;
using GameUI.Styles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpellboundSettlement.UIStates;

public class GameplayUIState : IUIState
{
	private const string PauseButtonText = "Pause";
	private const LayoutAnchor PauseButtonAnchor = LayoutAnchor.TopRight;
	private static readonly Point PauseButtonPosition = new(-20, 20);
	private static readonly Point PauseButtonSize = new(75, 25);
    
	private readonly ButtonStyle _buttonStyle;
	private readonly Button _pauseButton;

	public GameplayUIState()
	{
		_buttonStyle = new ButtonStyle
		{
			Font = GameManager.Font,
			BackgroundColor = Color.LightSlateGray,
			HoverBackgroundColor = Color.SlateGray,
			MousePressedBackgroundColor = Color.DarkSlateGray,
			DisabledBackgroundColor = Color.Red,
			BackgroundTexture = GameManager.Texture
		};

		_pauseButton = new Button(PauseButtonPosition, PauseButtonSize, PauseButtonText, _buttonStyle, PauseButtonAnchor);

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