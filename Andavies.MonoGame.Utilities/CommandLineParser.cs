using Serilog;

namespace Andavies.MonoGame.Utilities;

/// <summary>
/// Utility class to help parse and get command line arguments
/// </summary>
public sealed class CommandLineParser
{
	private readonly ILogger _logger;
	private readonly Dictionary<string, string> _parsedArgs = new();

	/// <summary>Constructor for CommandLineParser</summary>
	/// <param name="logger">ILogger object to help with logging warnings</param>
	public CommandLineParser(ILogger logger)
	{
		_logger = logger;
	}

	/// <summary>Parses the given argument string array into an internal collection</summary>
	/// <param name="args">The command line args that will be parsed</param>
	/// <param name="dontClearPreviousArgs">Set this to true if you do not want to clear the internal dictionary</param>
	public void ParseArgs(string[] args, bool dontClearPreviousArgs = false)
	{
		if (!dontClearPreviousArgs)
			_parsedArgs.Clear();

		for (int argIndex = 0; argIndex < args.Length; argIndex++)
		{
			if (!IsArgumentKey(args[argIndex]))
			{
				_logger.Warning("Expected an argument key bug found argument value. Keys must start with '-'. Found: {arg}", args[argIndex]);
				continue;
			}
			string key = args[argIndex];
			string value = "true";
			
			// Check to see if the next argument is a key or a value
			if (argIndex + 1 < args.Length && !IsArgumentKey(args[argIndex + 1]))
			{
				value = args[argIndex + 1];
				argIndex++;
			}

			if (!_parsedArgs.TryAdd(key, value))
			{
				_logger.Warning("Duplicate command line arg found. The first instance will be kept. Arg: {arg}", key);
			}
		}
	}

	/// <summary>Tries to get an argument value from a given argument key</summary>
	/// <param name="argKey">The argument key that will be used to find the argument value</param>
	/// <param name="argValue">The value of the argument corresponding to the given argument key</param>
	/// <returns>True if the key/value was found. False if the key/value does not exist</returns>
	public bool TryGetArg(string argKey, out string? argValue)
	{
		if (!IsArgumentKey(argKey))
		{
			argValue = null;
			_logger.Warning("Unable to get arg. Key does not begin with '-'. Key: {key}", argKey);
			return false;
		}

		return _parsedArgs.TryGetValue(argKey, out argValue);
	}

	private static bool IsArgumentKey(string arg) => arg.StartsWith('-');
}