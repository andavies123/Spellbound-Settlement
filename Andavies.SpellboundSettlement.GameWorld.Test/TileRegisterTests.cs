using Andavies.SpellboundSettlement.GameWorld.Repositories;
using Andavies.SpellboundSettlement.GameWorld.Tiles;
using Serilog;

namespace Andavies.SpellboundSettlement.GameWorld.Test;

[TestClass]
public class TileRegisterTests
{
	#region Constructor Tests

	[TestMethod]
	public void Constructor_DoesNotThrow_WhenParametersAreNotNull()
	{
		// Arrange / Act
		Action action = () =>
		{
			TileRegister _ = new(
				Substitute.For<ILogger>(),
				Substitute.For<ITileRegistry>());
		};

		// Assert
		action.Should().NotThrow<ArgumentNullException>();
	}

	[DataTestMethod]
	[DataRow(typeof(ILogger))]
	[DataRow(typeof(ITileRegistry))]
	public void Constructor_ThrowsException_WhenParameterIsNull(Type nullType)
	{
		// Arrange / Act
		Action action = () =>
		{
			TileRegister _ = new(
				(nullType == typeof(ILogger) ? null : Substitute.For<ILogger>())!,
				(nullType == typeof(ITileRegistry) ? null : Substitute.For<ITileRegistry>())!);
		};
		
		// Assert
		action.Should().Throw<ArgumentNullException>();
	}

	#endregion

	#region RegisterTiles Tests

	[TestMethod]
	public void RegisterTiles_AddsTilesToTileRegistry()
	{
		// Arrange
		TestHelper testHelper = new();

		// Act
		testHelper.TileRegister.RegisterTiles();

		// Assert
		testHelper.TileRegistry.Received().RegisterTile(Arg.Any<Tile>());
	}

	#endregion

	private class TestHelper
	{
		public TestHelper()
		{
			Logger = Substitute.For<ILogger>();
			TileRegistry = Substitute.For<ITileRegistry>();

			TileRegister = new TileRegister(Logger, TileRegistry);
		}
        
		public TileRegister TileRegister { get; }
		public ILogger Logger { get; }
		public ITileRegistry TileRegistry { get; }
	}
}