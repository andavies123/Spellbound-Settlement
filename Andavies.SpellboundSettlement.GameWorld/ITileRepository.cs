namespace Andavies.SpellboundSettlement.GameWorld;

public interface ITileRepository
{
	bool TryAddTileDetails(int key, ITileDetails tileDetails);
	bool TryGetTileDetails(int key, out ITileDetails? tileDetails);
}