using Microsoft.Xna.Framework.Graphics;

namespace UI.StateMachines;

public interface IUIState
{
	void Update();
	void Draw(SpriteBatch spriteBatch);
}