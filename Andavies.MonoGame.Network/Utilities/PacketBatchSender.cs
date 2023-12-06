using System.Collections.Concurrent;
using Andavies.MonoGame.Network.Extensions;
using LiteNetLib;
using LiteNetLib.Utils;
using Serilog;

namespace Andavies.MonoGame.Network.Utilities;

public class PacketBatchSender : IPacketBatchSender
{
	private readonly ILogger _logger;
	private readonly NetPacketProcessor _packetProcessor = new();
	private readonly NetDataWriter _dataWriter = new();
	private readonly ConcurrentQueue<Action> _batchQueue = new();
	private readonly object _queueLock = new(); // Lock to manage adding/removing from the batch queue
    
	public PacketBatchSender(ILogger logger)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	public void AddPacket<T>(NetPeer client, T packet) where T : INetSerializable
	{
		lock (_queueLock)
		{
			_batchQueue.Enqueue(() => SendPacketNow(client, packet));
		}
	}

	public void SendPacketNow<T>(NetPeer client, T packet) where T : INetSerializable
	{
		_dataWriter.Reset();
		_packetProcessor.WriteNetSerializable(_dataWriter, ref packet);
		client.Send(_dataWriter, DeliveryMethod.ReliableOrdered);
		_logger.LogPacketSent(packet, client.EndPoint.ToString());
	}

	public void SendBatch()
	{
		lock (_queueLock)
		{
			while (!_batchQueue.IsEmpty)
			{
				if (_batchQueue.TryDequeue(out Action? sendAction))
					sendAction.Invoke();
			}	
		}
	}

	public void ClearBatch()
	{
		lock (_queueLock)
		{
			_batchQueue.Clear();
		}
	}
}