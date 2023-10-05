using Microsoft.Xna.Framework.Graphics;

namespace SpellboundSettlement.GameStates;

public interface IGameStateManager
{
	IGameState CurrentGameState { get; }

	void Init();
	void LateInit();
	void Update(float deltaTimeSeconds);
	void Draw(SpriteBatch spriteBatch);
	void SetState(IGameState nextState);
}