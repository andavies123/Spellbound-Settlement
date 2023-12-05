using System.Net;

namespace Andavies.SpellboundSettlement.Server;

public static class ServerCommandLineUtility
{
	public const string IpCommandLineArgKey = "-ip";
	public const string PortCommandLineArgKey = "-port";
	public const string WorldNameCommandLineArgKey = "-worldName";

	/// <summary>Builds a command line args string to be used to start the server process</summary>
	/// <param name="ipAddress">The IP Address the server will run off of</param>
	/// <param name="port">The port the server will run off of</param>
	/// <param name="worldName">The name of the world. Can include spaces</param>
	/// <returns></returns>
	public static string BuildArgs(IPAddress ipAddress, int port, string worldName) =>
		$"{IpCommandLineArgKey} {ipAddress} " +
		$"{PortCommandLineArgKey} {port} " +
		$"{WorldNameCommandLineArgKey} \"{worldName}\"";
}