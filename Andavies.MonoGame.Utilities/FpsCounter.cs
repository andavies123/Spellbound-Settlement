using Microsoft.Xna.Framework;
using Serilog;
using IUpdateable = Andavies.MonoGame.Utilities.Interfaces.IUpdateable;

namespace Andavies.MonoGame.Utilities;

public class FpsCounter : IUpdateable
{
	private readonly ILogger _logger;

	private int _frameCount = 0;
	private double _fpsUpdateTimer = 0f;
	
	public FpsCounter(ILogger logger, int updateOrder)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		UpdateOrder = updateOrder;
	}
	
	public bool UpdateEnabled { get; set; } = false;

	public int UpdateOrder { get; }

	public void Update(GameTime gameTime)
	{
		_frameCount++;
		_fpsUpdateTimer += gameTime.ElapsedGameTime.TotalSeconds;
		if (_fpsUpdateTimer >= 1.0)
		{
			_logger.Debug("FPS: {fps}", _frameCount);
			_frameCount = 0;
			_fpsUpdateTimer -= 1f;
		}
	}
}