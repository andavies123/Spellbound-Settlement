using LiteNetLib.Utils;
using Serilog;

namespace Andavies.MonoGame.Network.Extensions;

public static class NetworkLoggerExtensions
{
	/// <summary>Flag for logging anything network packet related. Overrides other options</summary>
	public static bool LogPackets { get; set; } = false;
	
	/// <summary>Logs information when a network packet is sent</summary>
	/// <param name="logger">Extended ILogger object</param>
	/// <param name="packet">The packet that is being sent</param>
	/// <param name="to">Name of who the packet is being sent to</param>
	public static void LogPacketSent(this ILogger logger, INetSerializable packet, string to)
	{
		if (!LogPackets || !LogPackets)
			return;
		
		logger.Debug("Sent packet {type} to {to}", packet.GetType().Name, to);
	}

	/// <summary>Logs information when a network packet is received</summary>
	/// <param name="logger">Extended ILogger object</param>
	/// <param name="packet">The packet that is being received</param>
	/// <param name="from">The sender of the packet. Where the packet came from</param>
	public static void LogPacketReceived(this ILogger logger, INetSerializable packet, string from)
	{
		if (!LogPackets || !LogPackets)
			return;
		
		logger.Debug("Received packet {type} from {from}", packet.GetType().Name, from);
	}
}