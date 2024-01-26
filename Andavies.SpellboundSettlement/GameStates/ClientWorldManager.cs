using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Andavies.MonoGame.Network.Client;
using Andavies.MonoGame.Utilities;
using Andavies.SpellboundSettlement.GameWorld;
using Andavies.SpellboundSettlement.Meshes;
using Andavies.SpellboundSettlement.NetworkMessages.Messages.World;
using Andavies.SpellboundSettlement.Wizards;
using LiteNetLib.Utils;

namespace Andavies.SpellboundSettlement.GameStates;

public class ClientWorldManager : IClientWorldManager
{
	private readonly INetworkClient _networkClient;
	private readonly IChunkMeshBuilder _chunkMeshBuilder;
	private readonly WorldMesh _worldMesh;
	
	private readonly ConcurrentDictionary<Vector2Int, Chunk> _chunks = new();
	private readonly ConcurrentDictionary<Guid, Wizard> _wizards = new();

	public ClientWorldManager(
		INetworkClient networkClient,
		IChunkMeshBuilder chunkMeshBuilder,
		WorldMesh worldMesh)
	{
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
	
	private void AddOrUpdateChunk(Chunk chunk)
	{
		_chunks[chunk.ChunkPosition] = chunk;
	}

	private void AddOrUpdateWizard(Wizard wizard)
	{
		_wizards[wizard.Id] = wizard;
	}
	
	private void OnWorldChunkResponsePacketReceived(INetSerializable packet)
	{
		if (packet is not WorldChunkResponsePacket worldChunkResponsePacket)
			return;

		if (worldChunkResponsePacket.Chunk == null)
			return;
		
		AddOrUpdateChunk(worldChunkResponsePacket.Chunk);
		
		ChunkMesh chunkMesh = _chunkMeshBuilder.BuildChunkMesh(worldChunkResponsePacket.Chunk);
		_worldMesh.SetChunkMesh(chunkMesh, worldChunkResponsePacket.Chunk.ChunkPosition);
	}

	private void OnWizardUpdatedPacketReceived(INetSerializable packet)
	{
		if (packet is not WizardUpdatedPacket wizardUpdatedPacket || wizardUpdatedPacket.Wizard == null)
			return;

		AddOrUpdateWizard(wizardUpdatedPacket.Wizard);
	}
}