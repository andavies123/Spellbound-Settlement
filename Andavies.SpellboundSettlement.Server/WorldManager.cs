using Andavies.MonoGame.Utilities;
using Andavies.SpellboundSettlement.GameWorld;
using Andavies.SpellboundSettlement.Wizards;
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

	public void CreateWorld()
	{
		_logger.Debug("Creating the world...");
		_world.CreateNewWorld(Vector2Int.Zero, 5);

		for (int i = 0; i < 50; i++)
		{
			SpawnWizard();
		}
	}

	public void Tick()
	{
		// Add Update logic here
	}

	private void SpawnWizard()
	{
		int xPos = Random.Shared.Next(50);
		int zPos = Random.Shared.Next(50);
		int yPos = _world.GetHeightAtPosition(new Vector3Int(xPos, 0, zPos)) + 1; // Increase by 1 to be on top of the terrain
		Vector3Int position = new(xPos, yPos, zPos);
		float rotation = Random.Shared.Next(4) * MathHelper.PiOver2;
		
		BasicWizard wizard = new()
		{
			Name = "Andrew Davies",
			Position = position,
			Rotation = rotation
		};
		
		_wizardManager.AddWizard(wizard);
	}
}

public interface IWorldManager
{
	void CreateWorld();
	void Tick();
}