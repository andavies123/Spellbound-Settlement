using Andavies.MonoGame.Inputs;
using Microsoft.Xna.Framework.Input;

namespace SpellboundSettlement.Inputs;

public class PauseMenuInputManager : IInputManager
{
	private const Keys ExitMenuKey = Keys.Escape;

	public readonly KeyAction ExitMenu = new(ExitMenuKey);
	
	public void UpdateInput()
	{
		ExitMenu.CheckKey();
	}
}