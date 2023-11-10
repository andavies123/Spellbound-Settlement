using Andavies.MonoGame.UI.StateMachines;
using Andavies.MonoGame.UI.UIElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.Globals;

namespace SpellboundSettlement.UIStates.Gameplay;

public class GameplayUIState : IUIState
{
	private readonly IUIStyleCollection _uiStyleCollection;
	
	public GameplayUIState(IUIStyleCollection uiStyleCollection)
	{
		_uiStyleCollection = uiStyleCollection;
	}
	
	public Button PauseButton { get; private set; }

	public void Init() { }

	public void LateInit()
	{
		PauseButton = new Button(new Point(GameManager.Viewport.Width - 100, 50), new Point(75, 25), "Pause", _uiStyleCollection.DefaultButtonStyle);
	}

	public void Update(float deltaTimeSeconds)
	{
		PauseButton.Update(deltaTimeSeconds);
	}
    
	public void Draw(SpriteBatch spriteBatch)
	{
		PauseButton.Draw(spriteBatch);
	}

	public void Exit() { }
}