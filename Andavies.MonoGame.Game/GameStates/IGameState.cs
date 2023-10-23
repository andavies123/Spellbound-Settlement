using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.Inputs;
using Andavies.MonoGame.UI.StateMachines;

namespace SpellboundSettlement.GameStates;

public interface IGameState
{
	// State machine to handle multiple UI states
	IUIStateMachine UIStateMachine { get; }
	IInputManager InputState { get; }

	void Init();
	void LateInit();
	void Start();
	void Update(float deltaTimeSeconds);
	void Draw3D(GraphicsDevice graphicsDevice);
	void DrawUI(SpriteBatch spriteBatch);
	void End();
}