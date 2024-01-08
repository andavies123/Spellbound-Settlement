using Microsoft.Xna.Framework.Graphics;

namespace Andavies.SpellboundSettlement.GameWorld;

public interface IModelRepository
{
	bool TryAddModel(string key, Model model);
	bool TryGetModel(string key, out Model? model);
}