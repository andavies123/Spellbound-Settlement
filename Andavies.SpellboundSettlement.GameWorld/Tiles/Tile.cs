using Andavies.MonoGame.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.SpellboundSettlement.GameWorld.Tiles;

public abstract class Tile
{
	public abstract string TileId { get; }
	public abstract string DisplayName { get; }
	public abstract string Description { get; }
	public abstract bool IsBreakable { get; }
}

public abstract class TerrainTile : Tile
{
	public abstract bool IsLiquid { get; }
}

public abstract class ModelTile : Tile
{
	public abstract float MinGenerationScale { get; }
	public abstract float MaxGenerationScale { get; }
	
	public abstract ModelDetails ModelDetails { get; }
	
	public Model? Model { get; set; } = null;
}

public class AirTile : Tile
{
	public override string TileId => nameof(AirTile);
	public override string DisplayName => "Air";
	public override string Description => "Air";
	public override bool IsBreakable => false;
}

public class GroundTile : TerrainTile
{
	public override string TileId => nameof(GroundTile);
	public override string DisplayName => "Ground";
	public override string Description => "Ground";
	public override bool IsBreakable => true;
	public override bool IsLiquid => false;
}

public class GrassTile : ModelTile
{
	public override string TileId => nameof(GrassTile);
	public override string DisplayName => "Grass";
	public override string Description => "Grass that will grow after some time";
	public override bool IsBreakable => true;

	public override ModelDetails ModelDetails => new("Models/Grass/grass");

	public override float MinGenerationScale => .75f;
	public override float MaxGenerationScale => 1f;
}

public class SmallRockTile : ModelTile
{
	public override string TileId => nameof(SmallRockTile);
	public override string DisplayName => "Small Rock";
	public override string Description => "A small rock that can be broken for stone";
	public override bool IsBreakable => true;

	public override ModelDetails ModelDetails => new("Models/Rocks/rockSmall1");

	public override float MinGenerationScale => .25f;
	public override float MaxGenerationScale => 1f;
}

public class BushTile : ModelTile
{
	public override string TileId => nameof(BushTile);
	public override string DisplayName => "Bush";
	public override string Description => "A bush";
	public override bool IsBreakable => true;

	public override ModelDetails ModelDetails => new("Models/Bushes/bush");

	public override float MinGenerationScale => .25f;
	public override float MaxGenerationScale => 1f;
}