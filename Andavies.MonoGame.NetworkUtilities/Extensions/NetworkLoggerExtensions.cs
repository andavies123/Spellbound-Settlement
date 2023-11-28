using LiteNetLib.Utils;
using Serilog;

namespace Andavies.MonoGame.NetworkUtilities.Extensions;

public static class NetworkLoggerExtensions
{
	public static void LogPacketSent(this ILogger logger, INetSerializable packet, string to)
	{
		logger.Debug("Sent packet {type} to {to}", packet.GetType().Name, to);
	}

	public static void LogPacketReceived(this ILogger logger, INetSerializable packet, string from)
	{
		logger.Debug("Received packet {type} from {from}", packet.GetType().Name, from);
	}
}