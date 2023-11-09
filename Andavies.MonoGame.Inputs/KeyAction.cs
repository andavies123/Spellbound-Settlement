using Microsoft.Xna.Framework.Input;

namespace Andavies.MonoGame.Inputs;

public class KeyAction
{
	public KeyAction(Keys key) => Key = key;

	public bool IsKeyDown { get; private set; } = false;
	public Keys Key { get; set; }
	
	public event Action? OnKeyDown;
	public event Action? OnKeyUp;

	public void CheckKey()
	{
		if (!IsKeyDown && Keyboard.GetState().IsKeyDown(Key))
		{
			IsKeyDown = true;
			OnKeyDown?.Invoke();
		}
		else if (IsKeyDown && Keyboard.GetState().IsKeyUp(Key))
		{
			IsKeyDown = false;
			OnKeyUp?.Invoke();
		}
	}
}