using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.SpellboundSettlement.GameStates;

public interface IGameStateManager : IUpdateable
{
	IGameState CurrentGameState { get; }

	void Init();
	void LateInit();
	void Draw3D(GraphicsDevice graphicsDevice);
	void DrawUI(SpriteBatch spriteBatch);
	void SetState(IGameState nextState);
}