namespace Andavies.MonoGame.Utilities.Interfaces;

public interface IInitializable
{
	/// <summary>
	/// The order at which this instance should be initialized
	/// Small numbers come first
	/// Larger numbers are initialized last
	/// </summary>
	int InitOrder { get; }
    
	/// <summary>
	/// Called once at the beginning of the game
	/// </summary>
	void Init();
}