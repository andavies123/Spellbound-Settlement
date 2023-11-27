using Andavies.MonoGame.UI.StateMachines;
using Andavies.SpellboundSettlement.Inputs;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.SpellboundSettlement.GameStates;

public interface IGameState
{
	// State machine to handle multiple UI states
	IUIStateMachine UIStateMachine { get; }
	IInputState InputState { get; }

	void Init();
	void LateInit();
	void Start();
	void Update(float deltaTimeSeconds);
	void Draw3D(GraphicsDevice graphicsDevice);
	void DrawUI(SpriteBatch spriteBatch);
	void End();
}