using Andavies.SpellboundSettlement.GameWorld.Repositories;
using Andavies.SpellboundSettlement.GameWorld.Tiles;
using Serilog;

namespace Andavies.SpellboundSettlement.GameWorld;

/// <summary>
/// Class to remove the duplication of registering the same default tiles for both client and server
/// </summary>
public class TileRegister : ITileRegister
{
	private readonly ILogger _logger;
	private readonly ITileRegistry _tileRegistry;
	
	public TileRegister(ILogger logger, ITileRegistry tileRegistry)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_tileRegistry = tileRegistry ?? throw new ArgumentNullException(nameof(tileRegistry));
	}

	public void RegisterTiles()
	{
		_tileRegistry.RegisterTile(new AirTile());
		_tileRegistry.RegisterTile(new GroundTile());
		_tileRegistry.RegisterTile(new GrassTile());
		_tileRegistry.RegisterTile(new SmallRockTile());
		_tileRegistry.RegisterTile(new BushTile());
		
		_logger.Debug("Registered {tileCount} tiles", _tileRegistry.TileCount);
	}
}