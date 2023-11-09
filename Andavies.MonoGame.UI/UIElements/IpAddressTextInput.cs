using System.Text.RegularExpressions;
using Andavies.MonoGame.Inputs.InputListeners;
using Andavies.MonoGame.UI.Styles;
using Microsoft.Xna.Framework;

namespace Andavies.MonoGame.UI.UIElements;

public class IpAddressTextInput : TextInput
{
	private const string IpAddressPattern = @"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$";
	
	public IpAddressTextInput(Point position, Point size, TextInputStyle style) : 
		base(position, size, style, new NumbersOnlyTextListener()) { }

	public IpAddressTextInput(Point size, TextInputStyle style) : 
		base(size, style, new NumbersOnlyTextListener()) { }

	protected override void CheckValidity()
	{
		ContainsValidString = Regex.IsMatch(Text, IpAddressPattern);
	}
}