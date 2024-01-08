using Microsoft.Xna.Framework.Graphics;

namespace Andavies.SpellboundSettlement.Repositories;

public interface IModelRepository
{
	/// <summary>
	/// Adds a Model object to the internal collection using a string as its key
	/// </summary>
	/// <param name="key">The unique key that will be used to store/access the Model</param>
	/// <param name="model">The Model object that will be stored and accessed</param>
	/// <returns>True if the Model was stored successfully. False if not</returns>
	bool TryAddModel(string key, Model model);
	
	/// <summary>
	/// Gets a Model object from the internal collection using a string as the access key
	/// </summary>
	/// <param name="key">The unique key that is used to access the Model</param>
	/// <param name="model">The Model object that is stored in the internal collection. Will be null if not found</param>
	/// <returns>True if the Model was found successfully. False if not</returns>
	bool TryGetModel(string key, out Model model);
	
	/// <summary>
	/// Removes a Model object from the internal collection using its access key
	/// </summary>
	/// <param name="key">The unique key that is used to find the Model object</param>
	/// <returns>True if the Model object was successfully removed. False if it wasn't removed or not found</returns>
	bool TryRemoveModel(string key);
}