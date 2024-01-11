using Andavies.SpellboundSettlement.GameWorld.Repositories;

namespace Andavies.SpellboundSettlement.GameWorld;

public interface ITileLoader
{
	void LoadTilesFromJson(string tileDetailsJson, ITileRepository tileRepository);
}