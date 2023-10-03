using Microsoft.Xna.Framework;

namespace GameUI;

public class Button : UIElement
{
	public event Action MouseEntered;
	public event Action MouseExited;
	public event Action ButtonClicked;
	
	public Button(Rectangle bounds, LayoutAnchor elementAnchor, 
		LayoutAnchor screenAnchor, string text)
		: base(bounds, elementAnchor, screenAnchor)
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