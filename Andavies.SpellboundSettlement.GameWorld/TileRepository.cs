using Serilog;

namespace Andavies.SpellboundSettlement.GameWorld;

public class TileRepository : ITileRepository
{
	private readonly ILogger _logger;
	private readonly Dictionary<int, ITileDetails> _tiles = new();
	
	public TileRepository(ILogger logger)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	public bool TryAddTileDetails(int key, ITileDetails tileDetails)
	{
		return _tiles.TryAdd(key, tileDetails);
	}

	public bool TryGetTileDetails(int key, out ITileDetails? tileDetails)
	{
		return _tiles.TryGetValue(key, out tileDetails);
	}
}