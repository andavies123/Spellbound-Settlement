using Microsoft.Xna.Framework.Graphics;

namespace Andavies.SpellboundSettlement.GameStates;

public interface IGameStateManager
{
	IGameState CurrentGameState { get; }

	void Draw3D(GraphicsDevice graphicsDevice);
	void DrawUI(SpriteBatch spriteBatch);
	void SetState(IGameState nextState);
}