﻿using System.Collections.Generic;
using Andavies.SpellboundSettlement.GameWorld;

namespace Andavies.SpellboundSettlement.Repositories;

public interface ITileRepository
{
	bool TryAddTileDetails(int key, ITileDetails tileDetails);
	bool TryGetTileDetails(int key, out ITileDetails tileDetails);

	List<T> GetAllTileDetailsOfType<T>() where T : ITileDetails;
}