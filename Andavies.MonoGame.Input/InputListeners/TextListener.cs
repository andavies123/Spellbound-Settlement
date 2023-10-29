using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Andavies.MonoGame.Input.InputListeners;

public abstract class TextListener : ITextListener
{
	protected abstract Dictionary<Keys, char> KeyMap { get; }

	public void Listen(KeyboardState? previousState, KeyboardState? currentState, StringBuilder stringBuilder)
	{
		foreach (KeyValuePair<Keys, char> kvp in KeyMap)
		{
			if ((previousState?.IsKeyDown(kvp.Key) ?? false) && (currentState?.IsKeyUp(kvp.Key) ?? false))
				stringBuilder.Append(kvp.Value);
		}
	}
}