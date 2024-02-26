using Andavies.MonoGame.Utilities;
using Andavies.SpellboundSettlement.GameWorld;
using Andavies.SpellboundSettlement.GameWorld.Wizards;
using Microsoft.Xna.Framework;
using Serilog;

namespace Andavies.SpellboundSettlement.Server;

public class WorldManager : IWorldManager
{
	private const int InitialWorldGenerationRadius = 7;
	private const int InitialWizardSpawns = 5;
	
	private readonly ILogger _logger;
	private readonly IWizardManager _wizardManager;
	private readonly IWorldBuilder _worldBuilder;

	public WorldManager(ILogger logger, IWizardManager wizardManager, IWorldBuilder worldBuilder)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_wizardManager = wizardManager ?? throw new ArgumentNullException(nameof(wizardManager));
		_worldBuilder = worldBuilder ?? throw new ArgumentNullException(nameof(worldBuilder));
	}

	public World? World { get; private set; }

	public void Update(float deltaTimeSeconds)
	{
		foreach (Wizard wizard in _wizardManager.AllWizards.Values)
		{
			wizard.Update(deltaTimeSeconds);
		}

		World?.Update();
	}

	public void CreateNewWorld()
	{
		_logger.Debug("Creating new world...");
		World = _worldBuilder.BuildNewWorld(Vector2Int.Zero, InitialWorldGenerationRadius);

		for (int i = 0; i < InitialWizardSpawns; i++)
		{
			SpawnWizard();
		}
	}

	public void UpdateTile(Vector3Int tileWorldPosition, string newTileId)
	{
		if (World is null)
			return;
		
		Vector2Int chunkPosition = WorldHelper.WorldPositionToChunkPosition(tileWorldPosition);
		Vector3Int tilePosition = WorldHelper.WorldPositionToTilePosition(tileWorldPosition);

		if (!World.TryGetChunk(chunkPosition, out Chunk? chunk))
			return;

		chunk?.ChunkData.UpdateWorldTileId(tilePosition, newTileId);
	}

	private void SpawnWizard()
	{
		if (World is null)
		{
			_logger.Warning("Unable to spawn wizards. World does not exist");
			return;
		}
        
		int xPos = Random.Shared.Next(50);
		int zPos = Random.Shared.Next(50);

		if (!World.TryGetHeightAtPosition(new Vector3Int(xPos, 0, zPos), out int? terrainHeight) || terrainHeight == null)
			terrainHeight = 10;
			
		int yPos = terrainHeight.Value + 1; // Increase by 1 to be on top of the terrain
		Vector3Int position = new(xPos, yPos, zPos);
		float rotation = Random.Shared.Next(4) * MathHelper.PiOver2;

		EarthWizard wizard = new()
		{
			Data =
			{
				Name = "Andrew Davies",
				Position = position,
				Rotation = rotation,	
			},
			World = World
		};
		
		_wizardManager.AddOrUpdateWizard(wizard);
		wizard.Loiter();
	}
}

public interface IWorldManager
{
	World? World { get; }
    
	void Update(float deltaTimeSeconds);
	void CreateNewWorld();
	void UpdateTile(Vector3Int tileWorldPosition, string newTileId);
}