using Microsoft.Xna.Framework.Graphics;

namespace Andavies.MonoGame.UI.StateMachines;

public class UIStateMachine : IUIStateMachine
{
	private IUIState? _currentUIState;

	public void ChangeUIState(IUIState nextUIState) => _currentUIState = nextUIState;
	public void Update(float deltaTimeSeconds) => _currentUIState?.Update(deltaTimeSeconds);
	public void Draw(SpriteBatch spriteBatch) => _currentUIState?.Draw(spriteBatch);
}