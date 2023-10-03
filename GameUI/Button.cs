namespace GameUI;

public class Button : UIElement
{
	public event Action MouseEntered;
	public event Action MouseExited;
	public event Action ButtonClicked;
	
	public Button((float width, float height) size, (float x, float y) position, string text) : 
		base(size, position)
	{
		Text = text;
	}

	/// <summary>
	/// The text that is displayed on the button
	/// </summary>
	public string Text { get; set; }
	
	/// <summary>
	/// True if the button currently has focus
	/// False if the button does not currently have focus
	/// </summary>
	public bool IsFocusedOn { get; set; } = false;

	/// <summary>
	/// True if the button can be clicked
	/// False if the button can not be clicked
	/// </summary>
	public bool IsClickable { get; set; } = true;
}