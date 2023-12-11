namespace Andavies.SpellboundSettlement.Server;

public class ServerSettings
{
	public string IpAddress { get; set; } = "localhost";
	public string Port { get; set; } = "5555";
	public bool IsLocalOnly { get; set; } = false;

	public bool WhiteListEnabled { get; set; } = false;
	public bool BlackListEnabled { get; set; } = false;
	public List<string> WhiteList { get; set; } = new();
	public List<string> BlackList { get; set; } = new();
}