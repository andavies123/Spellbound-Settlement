using Andavies.MonoGame.Input;
using Microsoft.Xna.Framework.Input;

namespace SpellboundSettlement.Inputs;

public class PauseMenuInputManager : IInputManager
{
	private const Keys ExitMenuKey = Keys.Escape;

	public KeyAction ExitMenu = new(ExitMenuKey);
	
	public void UpdateInput()
	{
		ExitMenu.CheckKey();
	}
}