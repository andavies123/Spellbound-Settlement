using Microsoft.Xna.Framework.Graphics;

namespace SpellboundSettlement.UIStates;

public class UIStateMachine : IUIStateMachine
{
	private IUIState _currentUIState;

	public void ChangeUIState(IUIState nextUIState) => _currentUIState = nextUIState;
	public void Draw(SpriteBatch spriteBatch) => _currentUIState.Draw(spriteBatch);
}