﻿using Andavies.MonoGame.Utilities;
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
		_world.CreateNewWorld(Vector2Int.Zero, 5);

		BasicWizard wizard1 = new()
		{
			Name = "Andrew Davies",
			Position = new Vector3Int(0, 10, 0),
			Rotation = 0
		};
		
		BasicWizard wizard2 = new()
		{
			Name = "Andrew Davies",
			Position = new Vector3Int(2, 10, 0),
			Rotation = MathHelper.PiOver2
		};
		
		BasicWizard wizard3 = new()
		{
			Name = "Andrew Davies",
			Position = new Vector3Int(4, 10, 0),
			Rotation = MathHelper.Pi
		};
		
		BasicWizard wizard4 = new()
		{
			Name = "Andrew Davies",
			Position = new Vector3Int(6, 10, 0),
			Rotation = MathHelper.PiOver2 * 3
		};
		
		_wizardManager.AddWizard(wizard1);
		_wizardManager.AddWizard(wizard2);
		_wizardManager.AddWizard(wizard3);
		_wizardManager.AddWizard(wizard4);
	}

	public void Tick()
	{
		// Add Update logic here
	}
}

public interface IWorldManager
{
	void CreateWorld();
	void Tick();
}