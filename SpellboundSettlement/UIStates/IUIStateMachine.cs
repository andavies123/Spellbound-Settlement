using Microsoft.Xna.Framework.Graphics;

namespace SpellboundSettlement.UIStates;

public interface IUIStateMachine
{
	void ChangeUIState(IUIState nextUIState);
	void Update();
	void Draw(SpriteBatch spriteBatch);
}