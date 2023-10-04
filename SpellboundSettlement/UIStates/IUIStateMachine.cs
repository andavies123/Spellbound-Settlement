using Microsoft.Xna.Framework.Graphics;

namespace SpellboundSettlement.UIStates;

public interface IUIStateMachine
{
	void ChangeUIState(IUIState nextUIState);
	void Draw(SpriteBatch spriteBatch);
}