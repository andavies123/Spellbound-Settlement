using LiteNetLib.Utils;
using Serilog;

namespace Andavies.MonoGame.NetworkUtilities.Extensions;

public static class NetworkLoggerExtensions
{
	public static void LogPacketSent(this ILogger logger, INetSerializable packet)
	{
		logger.Debug("Sent packet {type}", packet.GetType().Name);
	}

	public static void LogPacketReceived(this ILogger logger, INetSerializable packet)
	{
		logger.Debug("Received packet {type}", packet.GetType().Name);
	}
}