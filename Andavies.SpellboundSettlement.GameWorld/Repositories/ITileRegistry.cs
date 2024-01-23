using Andavies.SpellboundSettlement.GameWorld.Tiles;

namespace Andavies.SpellboundSettlement.GameWorld.Repositories;

public interface ITileRegistry
{
	/// <summary>
	/// The current number of registered tiles
	/// </summary>
	int TileCount { get; }
	
	/// <summary>
	/// Registers a tile to the registry.
	/// Uses the tile's unique Id to register
	/// </summary>
	/// <param name="tile">The tile that will be added to the registry</param>
	/// <returns>True if the tile was registered properly, False if there is an existing tile with the same Id</returns>
	bool RegisterTile(Tile tile);
	
	/// <summary>
	/// Checks and returns a registered tile based on the given key
	/// </summary>
	/// <param name="key">The key that will be used to look for the tile</param>
	/// <param name="tile">The tile that is held in the registry. Will be null if not found</param>
	/// <returns>True if the tile was found, False if the tile was not found</returns>
	bool TryGetTile(string key, out Tile? tile);

	/// <summary>
	/// Searches the registry for all tiles of a given tile type
	/// </summary>
	/// <typeparam name="T">Generic type for Tile</typeparam>
	/// <returns>A list containing all tiles in the registry of given type. Empty list if none exist</returns>
	List<T> GetAllTilesOfType<T>() where T : Tile;
}