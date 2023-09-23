using System;
using Microsoft.Xna.Framework.Input;

namespace SpellboundSettlement.Inputs;

public class PauseMenuInputManager : IInputManager
{
	private const Keys ExitMenuKey = Keys.Escape;

	public event Action ExitMenu;
	
	public void UpdateInput()
	{
		CheckExitMenuInput();
	}

	private void CheckExitMenuInput()
	{
		if (Keyboard.GetState().IsKeyDown(ExitMenuKey))
			ExitMenu?.Invoke();
	}
}