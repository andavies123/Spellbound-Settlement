namespace Andavies.MonoGame.Inputs.InputListeners;

public interface IInputListener
{
	/// <summary>The current text from the internal string</summary>
	string Text { get; }
	
	/// <summary>Returns the current length of the internal string</summary>
	int Length { get; }
	
	/// <summary>Resets the internal string to empty</summary>
	void ResetListener();

	/// <summary>Removes the last character that was inputted</summary>
	void RemoveLastCharacter();
	
	/// <summary>Listens for keypresses and updates the internal string</summary>
	void Listen();
}