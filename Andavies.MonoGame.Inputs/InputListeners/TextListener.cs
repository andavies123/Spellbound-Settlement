using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Andavies.MonoGame.Inputs.InputListeners;

public abstract class TextListener : ITextListener
{
	protected abstract Dictionary<Keys, char> KeyMap { get; }

	public virtual void Listen(KeyboardState? previousState, KeyboardState? currentState, StringBuilder stringBuilder)
	{
		if (!currentState.HasValue || !previousState.HasValue)
			return;

		// Values that are in the current state and not the previous state
		// This should represent the keys that were just pressed this state
		List<Keys> newlyPressedKeys = currentState.Value.GetPressedKeys().Except(previousState.Value.GetPressedKeys()).ToList();

		foreach (Keys pressedKey in newlyPressedKeys)
		{
			if (KeyMap.TryGetValue(pressedKey, out char character))
				stringBuilder.Append(character);
		}
	}
}