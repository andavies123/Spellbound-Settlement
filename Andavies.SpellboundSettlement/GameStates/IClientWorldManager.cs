using System;
using System.Collections.Generic;
using Andavies.MonoGame.Utilities;
using Andavies.SpellboundSettlement.GameWorld;
using Andavies.SpellboundSettlement.GameWorld.Wizards;
using Andavies.SpellboundSettlement.Wizards;

namespace Andavies.SpellboundSettlement.GameStates;

public interface IClientWorldManager
{
	IReadOnlyDictionary<Vector2Int, Chunk> AllChunks { get; }
	IReadOnlyDictionary<Guid, Wizard> AllWizards { get; }
}