using Microsoft.Xna.Framework.Graphics;

namespace SpellboundSettlement.UIStates;

public interface IUIState
{
	void Update();
	void Draw(SpriteBatch spriteBatch);
}