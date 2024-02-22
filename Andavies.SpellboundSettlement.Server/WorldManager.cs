using Andavies.MonoGame.Utilities;
using Andavies.SpellboundSettlement.GameWorld;
using Andavies.SpellboundSettlement.GameWorld.Wizards;
using Microsoft.Xna.Framework;
using Serilog;

namespace Andavies.SpellboundSettlement.Server;

public class WorldManager : IWorldManager
{
	private readonly ILogger _logger;
	private readonly IWizardManager _wizardManager;
	private readonly World _world;

	public WorldManager(ILogger logger, IWizardManager wizardManager, World world)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_wizardManager = wizardManager ?? throw new ArgumentNullException(nameof(wizardManager));
		_world = world ?? throw new ArgumentNullException(nameof(world));
	}

	public void Tick(float deltaTimeSeconds)
	{
		foreach (Wizard wizard in _wizardManager.AllWizards.Values)
		{
			wizard.Update(deltaTimeSeconds);
		}
	}

	public void CreateWorld()
	{
		_logger.Debug("Creating the world...");
		_world.CreateNewWorld(Vector2Int.Zero, 5);

		for (int i = 0; i < 1; i++)
		{
			SpawnWizard();
		}
	}

	public void UpdateTile(Vector3Int tileWorldPosition, string newTileId)
	{
		Vector2Int chunkPosition = WorldHelper.WorldPositionToChunkPosition(tileWorldPosition);
		Vector3Int tilePosition = WorldHelper.WorldPositionToTilePosition(tileWorldPosition);

		Chunk chunk = _world.GetChunk(chunkPosition);
		chunk.UpdateWorldTile(tilePosition, newTileId);
	}

	private void SpawnWizard()
	{
		int xPos = Random.Shared.Next(50);
		int zPos = Random.Shared.Next(50);
		int yPos = _world.GetHeightAtPosition(new Vector3Int(xPos, 0, zPos)) + 1; // Increase by 1 to be on top of the terrain
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
			World = _world
		};
		
		_wizardManager.AddOrUpdateWizard(wizard);
		wizard.Loiter();
	}
}

public interface IWorldManager
{
	void Tick(float deltaTimeSeconds);
	void CreateWorld();
	void UpdateTile(Vector3Int tileWorldPosition, string newTileId);
}