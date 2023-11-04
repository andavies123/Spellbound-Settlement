using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Andavies.MonoGame.UI.StateMachines;
using Andavies.MonoGame.UI.Styles;
using Andavies.MonoGame.UI.UIElements;
using SpellboundSettlement.Globals;

namespace SpellboundSettlement.UIStates;

public class GameplayUIState : IUIState
{
	public Button PauseButton { get; private set; }

	public void Init() { }

	public void LateInit()
	{
		ButtonStyle buttonStyle = new()
		{
			Font = GlobalFonts.DefaultFont,
			BackgroundColor = Color.LightSlateGray,
			HoverBackgroundColor = Color.SlateGray,
			MousePressedBackgroundColor = Color.DarkSlateGray,
			DisabledBackgroundColor = Color.Red,
			BackgroundTexture = GameManager.Texture
		};

		PauseButton = new Button(new Point(GameManager.Viewport.Width - 100, 50), new Point(75, 25), "Pause", buttonStyle);
	}

	public void Update(float deltaTimeSeconds)
	{
		PauseButton.Update();
	}
    
	public void Draw(SpriteBatch spriteBatch)
	{
		PauseButton.Draw(spriteBatch);
	}

	public void Exit() { }
}