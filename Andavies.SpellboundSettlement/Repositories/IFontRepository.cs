using Microsoft.Xna.Framework.Graphics;

namespace Andavies.SpellboundSettlement.Repositories;

public interface IFontRepository
{
	/// <summary>
	/// Adds a Font object to the internal collection using a unique string as its key
	/// </summary>
	/// <param name="key">The unique key that will be used to store/access the Font</param>
	/// <param name="font">The Font object that will be stored and accessed</param>
	/// <returns>True if the Font was stored successfully. False if not</returns>
	bool TryAddFont(string key, SpriteFont font);
	
	/// <summary>
	/// Gets a Font object from the internal collection using a unique string as the access key
	/// </summary>
	/// <param name="key">The unique key that is used to access the Font object</param>
	/// <param name="font">The Font object that is stored in the internal collection. Will be null if not found</param>
	/// <returns>True if the Font object was found successfully. False if not</returns>
	bool TryGetFont(string key, out SpriteFont font);
	
	/// <summary>
	/// Removes a Font object from the internal collection using its unique access key
	/// </summary>
	/// <param name="key">The unique key that is used to find the Font object</param>
	/// <returns>True if the Font object was successfully removed. False if it wasn't removed or not found</returns>
	bool TryRemoveFont(string key);
}