using Microsoft.Xna.Framework.Graphics;
using Serilog;

namespace Andavies.SpellboundSettlement.GameWorld;

public class ModelRepository : IModelRepository
{
	private readonly ILogger _logger;
	private readonly Dictionary<string, Model> _models = new();
	
	public ModelRepository(ILogger logger)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	public bool TryAddModel(string key, Model model)
	{
		if (!_models.TryAdd(key, model))
		{
			_logger.Warning("Unable to add Model with key: {key}", key);
			return false;
		}

		return true;
	}

	public bool TryGetModel(string key, out Model? model)
	{
		if (!_models.TryGetValue(key, out model))
		{
			_logger.Warning("Unable to get Model with key: {key}", key);
			return false;
		}

		return true;
	}
}