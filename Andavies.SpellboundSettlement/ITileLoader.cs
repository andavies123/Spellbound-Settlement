using Andavies.SpellboundSettlement.GameWorld;

namespace Andavies.SpellboundSettlement;

public interface ITileLoader
{
	void LoadTilesFromJson(string filePath, ITileRepository tileRepository);
}