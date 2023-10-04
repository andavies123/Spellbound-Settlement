using Microsoft.Xna.Framework.Graphics;

namespace UI.StateMachines;

public interface IUIStateMachine
{
	void ChangeUIState(IUIState nextUIState);
	void Update(float deltaTime);
	void Draw(SpriteBatch spriteBatch);
}