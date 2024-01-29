using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Andavies.MonoGame.Network.Client;
using Andavies.MonoGame.Utilities;
using Andavies.SpellboundSettlement.GameWorld;
using Andavies.SpellboundSettlement.GameWorld.Wizards;
using Andavies.SpellboundSettlement.Meshes;
using Andavies.SpellboundSettlement.NetworkMessages.Messages.World;
using Andavies.SpellboundSettlement.Wizards;
using LiteNetLib.Utils;
using Serilog;

namespace Andavies.SpellboundSettlement.GameStates;

public class ClientWorldManager : IClientWorldManager
{
	private readonly ILogger _logger;
	private readonly INetworkClient _networkClient;
	private readonly IChunkMeshBuilder _chunkMeshBuilder;
	private readonly WorldMesh _worldMesh;
	
	private readonly ConcurrentDictionary<Vector2Int, Chunk> _chunks = new();
	private readonly ConcurrentDictionary<Guid, Wizard> _wizards = new();

	public ClientWorldManager(
		ILogger logger,
		INetworkClient networkClient,
		IChunkMeshBuilder chunkMeshBuilder,
		WorldMesh worldMesh)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_networkClient = networkClient ?? throw new ArgumentNullException(nameof(networkClient));
		_chunkMeshBuilder = chunkMeshBuilder ?? throw new ArgumentNullException(nameof(chunkMeshBuilder));
		_worldMesh = worldMesh ?? throw new ArgumentNullException(nameof(worldMesh));
		
		SubscribeToServerMessages();
	}
	
	public IReadOnlyDictionary<Vector2Int, Chunk> AllChunks => _chunks;
	public IReadOnlyDictionary<Guid, Wizard> AllWizards => _wizards;
	
	private void SubscribeToServerMessages()
	{
		_networkClient.AddSubscription<WizardUpdatedPacket>(OnWizardUpdatedPacketReceived);
		_networkClient.AddSubscription<WorldChunkResponsePacket>(OnWorldChunkResponsePacketReceived);
	}
	
	private void OnWorldChunkResponsePacketReceived(INetSerializable packet)
	{
		if (packet is not WorldChunkResponsePacket worldChunkResponsePacket)
			return;

		if (worldChunkResponsePacket.Chunk == null)
			return;

		_chunks[worldChunkResponsePacket.Chunk.ChunkPosition] = worldChunkResponsePacket.Chunk;
		
		ChunkMesh chunkMesh = _chunkMeshBuilder.BuildChunkMesh(worldChunkResponsePacket.Chunk);
		_worldMesh.SetChunkMesh(chunkMesh, worldChunkResponsePacket.Chunk.ChunkPosition);
	}

	private void OnWizardUpdatedPacketReceived(INetSerializable packet)
	{
		if (packet is not WizardUpdatedPacket wizardUpdatedPacket || wizardUpdatedPacket.Wizard == null)
			return;

		_wizards[wizardUpdatedPacket.Wizard.Id] = wizardUpdatedPacket.Wizard;
	}
}