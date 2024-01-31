using System.Text.RegularExpressions;
using Andavies.MonoGame.Inputs;
using Andavies.MonoGame.Inputs.InputListeners;
using Andavies.MonoGame.UI.Styles;
using Microsoft.Xna.Framework;

namespace Andavies.MonoGame.UI.UIElements.TextInputs;

public class IpAddressTextInput : TextInput
{
	private const string IpAddressPattern = @"^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)\.?\b){4}$";
	
	public IpAddressTextInput(IInputManager inputManager, Point position, Point size, TextInputStyle style) : 
		base(inputManager, position, size, style, new NumberDecimalInputListener(inputManager)) { }

	public IpAddressTextInput(IInputManager inputManager, Point size, TextInputStyle style) : 
		base(inputManager, size, style, new NumberDecimalInputListener(inputManager)) { }

	protected override void ValidateText()
	{
		ContainsValidString = Regex.IsMatch(Text, IpAddressPattern);
	}
}