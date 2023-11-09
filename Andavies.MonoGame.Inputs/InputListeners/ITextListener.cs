using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Andavies.MonoGame.Inputs.InputListeners;

public interface ITextListener
{
	void Listen(KeyboardState? previousState, KeyboardState? currentState, StringBuilder stringBuilder);
}