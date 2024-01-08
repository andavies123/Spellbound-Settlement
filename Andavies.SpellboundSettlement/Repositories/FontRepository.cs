using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Serilog;

namespace Andavies.SpellboundSettlement.Repositories;

public class FontRepository : IFontRepository
{
	private readonly ILogger _logger;
	private readonly Dictionary<string, SpriteFont> _fonts = new();
	
	public FontRepository(ILogger logger)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	public bool TryAddFont(string key, SpriteFont font)
	{
		if (!_fonts.TryAdd(key, font))
		{
			_logger.Warning("Unable to add Font with key: {key}", key);
			return false;
		}

		return true;
	}

	public bool TryGetFont(string key, out SpriteFont font)
	{
		if (!_fonts.TryGetValue(key, out font))
		{
			_logger.Warning("Unable to get Font with key: {key}", key);
			return false;
		}

		return true;
	}

	public bool TryRemoveFont(string key)
	{
		if (!_fonts.Remove(key))
		{
			_logger.Warning("Unable to remove Font with key: {key}", key);
			return false;
		}

		return true;
	}
}