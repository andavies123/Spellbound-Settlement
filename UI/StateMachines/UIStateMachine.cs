using Microsoft.Xna.Framework.Graphics;

namespace UI.StateMachines;

public class UIStateMachine : IUIStateMachine
{
	private IUIState? _currentUIState;

	public void ChangeUIState(IUIState nextUIState) => _currentUIState = nextUIState;
	public void Update(float deltaTime) => _currentUIState?.Update();
	public void Draw(SpriteBatch spriteBatch) => _currentUIState?.Draw(spriteBatch);
}