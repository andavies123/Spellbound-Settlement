using Microsoft.Xna.Framework.Graphics;

namespace Andavies.MonoGame.UI.StateMachines;

public interface IUIState
{
	void Init();
	void LateInit();
	void Update(float deltaTimeSeconds);
	void Draw(SpriteBatch spriteBatch);
}