using Serilog;

namespace Andavies.SpellboundSettlement.GameWorld.Repositories;

public class TileRepository : ITileRepository
{
	private readonly ILogger _logger;
	private readonly Dictionary<int, ITileDetails> _allTileDetails = new();
	
	public TileRepository(ILogger logger)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	public bool TryAddTileDetails(int key, ITileDetails tileDetails)
	{
		if (!_allTileDetails.TryAdd(key, tileDetails))
		{
			_logger.Warning("Unable to add Tile Details with key/Name: {key}/{name}", key, tileDetails.DisplayName);
			return false;
		}
		
		return true;
	}

	public bool TryGetTileDetails(int key, out ITileDetails? tileDetails)
	{
		if (!_allTileDetails.TryGetValue(key, out tileDetails))
		{
			_logger.Warning("Unable to get Tile Details with key: {key}", key);
			return false;
		}

		return true;
	}

	public List<T> GetAllTileDetailsOfType<T>() where T : ITileDetails
	{
		List<T> tileDetailsByType = new();
        
		foreach (ITileDetails tileDetails in _allTileDetails.Values)
		{
			if (tileDetails is T tileDetailsTyped)
			{
				tileDetailsByType.Add(tileDetailsTyped);
			}
		}

		return tileDetailsByType;
	}
}