using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Andavies.MonoGame.Inputs.InputListeners;

public abstract class InputListener : IInputListener
{
	protected readonly IInputManager InputManager;
	protected readonly StringBuilder StringBuilder = new();

	protected InputListener(IInputManager inputManager)
	{
		InputManager = inputManager ?? throw new ArgumentNullException(nameof(inputManager));
	}

	public string Text => StringBuilder.ToString();
	public int Length => StringBuilder.Length;
	
	protected abstract Dictionary<Keys, char> KeyMap { get; }
	
	public void ResetListener() => StringBuilder.Clear();

	public void RemoveLastCharacter()
	{
		if (StringBuilder.Length == 0)
			return;

		StringBuilder.Remove(StringBuilder.Length - 1, 1);
	}

	public virtual void Listen()
	{
		foreach (Keys pressedKey in InputManager.KeysPressedThisFrame)
		{
			if (IsKeyValid(pressedKey, out char character))
				StringBuilder.Append(character);
		}
	}

	/// <summary>Checks whether or not a given key is valid or not, and returns the proper character</summary>
	/// <param name="key">The key to check against</param>
	/// <param name="character">The character that is mapped to the key</param>
	/// <returns>True if the key/character is valid. False if not</returns>
	protected abstract bool IsKeyValid(Keys key, out char character);
}