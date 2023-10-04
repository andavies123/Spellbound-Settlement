using Microsoft.Xna.Framework.Graphics;

namespace SpellboundSettlement.UIStates;

public interface IUIState
{
	void Draw(SpriteBatch spriteBatch);
}