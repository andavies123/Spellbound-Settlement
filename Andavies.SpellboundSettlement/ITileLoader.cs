using Andavies.SpellboundSettlement.GameWorld;
using Andavies.SpellboundSettlement.Repositories;

namespace Andavies.SpellboundSettlement;

public interface ITileLoader
{
	void LoadTilesFromJson(string filePath, ITileRepository tileRepository);
}