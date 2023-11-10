using System.Text.RegularExpressions;
using Andavies.MonoGame.Inputs.InputListeners;
using Andavies.MonoGame.UI.Styles;
using Microsoft.Xna.Framework;

namespace Andavies.MonoGame.UI.UIElements.TextInputs;

public class IpAddressTextInput : TextInput
{
	private const string IpAddressPattern = @"^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)\.?\b){4}$";
	
	public IpAddressTextInput(Point position, Point size, TextInputStyle style) : 
		base(position, size, style, new NumbersOnlyTextListener()) { }

	public IpAddressTextInput(Point size, TextInputStyle style) : 
		base(size, style, new NumbersOnlyTextListener()) { }

	protected override void ValidateText()
	{
		ContainsValidString = Regex.IsMatch(Text, IpAddressPattern);
	}
}