using System.Collections.Concurrent;
using System.Net;
using System.Text;
using static Andavies.SpellboundSettlement.Server.ServerCommandLineUtility;

namespace Andavies.SpellboundSettlement.Server;

public class ServerCommandLineArgsBuilder
{
	private readonly ConcurrentDictionary<string, string?> _arguments = new();
	
	public ServerCommandLineArgsBuilder SetIpAddress(IPAddress ipAddress)
	{
		SetArg(IpCommandLineArgKey, ipAddress.ToString());
		return this;
	}

	public ServerCommandLineArgsBuilder SetPort(int port)
	{
		SetArg(PortCommandLineArgKey, port.ToString());
		return this;
	}

	public ServerCommandLineArgsBuilder SetLocalOnlyServer(bool isLocal)
	{
		if (!isLocal)
			SetArg(LocalOnlyCommandLineArgKey, null);
		return this;
	}

	/// <summary>Builds the command line arguments string</summary>
	/// <returns>The command line arguments as a space seperated string</returns>
	public string BuildArgs()
	{
		StringBuilder stringBuilder = new();
		
		foreach ((string key, string? value) in _arguments)
		{
			stringBuilder.Append($"{key} ");
			if (value != null)
				stringBuilder.Append($"\"{value}\" ");
		}

		return stringBuilder.ToString().TrimEnd();
	}

	/// <summary>Clears out the internal collection</summary>
	public void Reset()
	{
		_arguments.Clear();
	}

	private void SetArg(string key, string? value)
	{
		_arguments.TryAdd(key, value);
	}
}