namespace Andavies.MonoGame.Utilities.Interfaces;

public interface ILateInitializable
{
	/// <summary>
	/// The order at which this instance should be initialized
	/// Small numbers come first
	/// Larger numbers are initialized last
	/// </summary>
	int LateInitOrder { get; }

	/// <summary>
	/// Called once after LoadContent has been called
	/// Should be used for initialization regarding content
	/// </summary>
	void LateInit();
}