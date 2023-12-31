﻿using Andavies.MonoGame.Inputs;
using Microsoft.Xna.Framework.Input;

namespace Andavies.SpellboundSettlement.Inputs;

public class PauseMenuInputState : IInputState
{
	private const Keys ExitMenuKey = Keys.Escape;

	public readonly KeyAction ExitMenu = new(ExitMenuKey);
	
	public void UpdateInput()
	{
		ExitMenu.CheckKey();
	}
}