using Andavies.SpellboundSettlement.GameWorld.Tiles;
using Serilog;

namespace Andavies.SpellboundSettlement.GameWorld.Repositories;

public class TileRegistry : ITileRegistry
{
	private readonly ILogger _logger;
	private readonly Dictionary<string, Tile> _allTiles = new();
	
	public TileRegistry(ILogger logger)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	public bool RegisterTile(Tile tile)
	{
		if (!_allTiles.TryAdd(tile.TileId, tile))
		{
			_logger.Warning("Unable to add tile: {tile name}", tile.DisplayName);
			return false;
		}
		
		return true;
	}

	public bool TryGetTile(string key, out Tile? tile)
	{
		if (!_allTiles.TryGetValue(key, out tile))
		{
			_logger.Warning("Unable to get tile: {key}", key);
			return false;
		}

		return true;
	}

	public List<T> GetAllTilesOfType<T>() where T : Tile
	{
		List<T> tilesByType = new();
        
		foreach (Tile tile in _allTiles.Values)
		{
			if (tile is T tileTyped)
			{
				tilesByType.Add(tileTyped);
			}
		}

		return tilesByType;
	}
}