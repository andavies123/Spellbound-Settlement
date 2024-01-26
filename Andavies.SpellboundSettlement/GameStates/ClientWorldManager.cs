using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Andavies.MonoGame.Utilities;
using Andavies.SpellboundSettlement.GameWorld;
using Andavies.SpellboundSettlement.Wizards;

namespace Andavies.SpellboundSettlement.GameStates;

public class ClientWorldManager : IClientWorldManager
{
	private readonly ConcurrentDictionary<Vector2Int, Chunk> _chunks = new();
	private readonly ConcurrentDictionary<Guid, Wizard> _wizards = new();

	public IReadOnlyDictionary<Vector2Int, Chunk> AllChunks => _chunks;
	public IReadOnlyDictionary<Guid, Wizard> AllWizards => _wizards;
	
	public void AddOrUpdateChunk(Chunk chunk)
	{
		_chunks[chunk.ChunkPosition] = chunk;
	}

	public void AddOrUpdateWizard(Wizard wizard)
	{
		_wizards[wizard.Id] = wizard;
	}
}

public interface IClientWorldManager
{
	IReadOnlyDictionary<Vector2Int, Chunk> AllChunks { get; }
	IReadOnlyDictionary<Guid, Wizard> AllWizards { get; }
	
	void AddOrUpdateChunk(Chunk chunk);
	void AddOrUpdateWizard(Wizard wizard);
}