using Andavies.SpellboundSettlement.GameWorld.Repositories;
using Andavies.SpellboundSettlement.GameWorld.Tiles;
using Serilog;

namespace Andavies.SpellboundSettlement.GameWorld.Test;

[TestClass]
public class TileRegistryTests
{
	#region TileCount Tests

	[TestMethod]
	public void TileCount_ReturnsAmountOfTilesInRegistry()
	{
		// Arrange
		TileRegistry tileRegistry = new(Substitute.For<ILogger>());

		// Act
		tileRegistry.RegisterTile(new AirTile());
		tileRegistry.RegisterTile(new BushTile());
		tileRegistry.RegisterTile(new GrassTile());
		tileRegistry.RegisterTile(new GroundTile());

		// Assert
		tileRegistry.TileCount.Should().Be(4);
	}

	#endregion

	#region RegisterTile Tests

	[TestMethod]
	public void RegisterTile_AddsTileToInternalCollection_WhenTileDoesNotAlreadyExist()
	{
		// Arrange
		TileRegistry tileRegistry = new(Substitute.For<ILogger>());
		Tile tile = new BushTile();
        
		// Act
		tileRegistry.RegisterTile(tile);
		
		// Assert
		tileRegistry.TryGetTile(tile.TileId, out _).Should().BeTrue();
	}

	[TestMethod]
	public void RegisterTile_DoesNotAddTileToInternalCollection_WhenTileAlreadyExists()
	{
		// Arrange
		TileRegistry tileRegistry = new(Substitute.For<ILogger>());
		Tile tile = new GrassTile();
		
		// Act
		tileRegistry.RegisterTile(tile);
		tileRegistry.RegisterTile(tile);
		
		// Assert
		tileRegistry.TileCount.Should().Be(1);
	}

	[TestMethod]
	public void RegisterTile_ReturnsTrue_WhenSuccessfullyAddingToRegistry()
	{
		// Arrange
		TileRegistry tileRegistry = new(Substitute.For<ILogger>());
		
		// Act
		bool addedToRegistry = tileRegistry.RegisterTile(new BushTile());
		
		// Assert
		addedToRegistry.Should().BeTrue();
	}

	[TestMethod]
	public void RegisterTile_ReturnsFalse_WhenTileAlreadyExistsInRegistry()
	{
		// Arrange
		TileRegistry tileRegistry = new(Substitute.For<ILogger>());
		Tile tile = new AirTile();
		
		// Act
		tileRegistry.RegisterTile(tile);
		bool addedToRegistry = tileRegistry.RegisterTile(tile);
		
		// Assert
		addedToRegistry.Should().BeFalse();
	}

	[TestMethod]
	public void RegisterTile_LoggerDoesNotWarn_WhenAbleToRegisterTile()
	{
		// Arrange
		ILogger logger = Substitute.For<ILogger>();
		TileRegistry tileRegistry = new(logger);
		
		// Act
		tileRegistry.RegisterTile(new AirTile());
		tileRegistry.RegisterTile(new BushTile()); // Registering 2 different tiles shouldn't cause warning

		// Assert
		logger.DidNotReceive().Warning(Arg.Is<string>(x => x.Contains("Unable to add tile")), Arg.Any<string>());
	}

	[TestMethod]
	public void RegisterTile_LoggerWarns_WhenUnableToRegisterTile()
	{
		// Arrange
		ILogger logger = Substitute.For<ILogger>();
		TileRegistry tileRegistry = new(logger);
		Tile tile = new AirTile();

		// Act
		tileRegistry.RegisterTile(tile);
		tileRegistry.RegisterTile(tile); // Registering same tile twice should cause warning
		
		// Assert
		logger.Received(1).Warning(Arg.Is<string>(x => x.Contains("Unable to add tile")), Arg.Any<string>());
	}

	#endregion

	#region TryGetTile Tests

	[TestMethod]
	public void TryGetTile_ReturnsTrue_WhenTileHasBeenAddedToRegistry()
	{
		// Arrange
		TileRegistry tileRegistry = new(Substitute.For<ILogger>());
		Tile tile = new AirTile();
		
		// Act
		tileRegistry.RegisterTile(tile);
		bool tileExists = tileRegistry.TryGetTile(tile.TileId, out _);
		
		// Assert
		tileExists.Should().BeTrue();
	}

	[TestMethod]
	public void TryGetTile_ReturnsFalse_WhenTileHasNotBeenAddedToRegistry()
	{
		// Arrange
		TileRegistry tileRegistry = new(Substitute.For<ILogger>());
		Tile tile = new AirTile();
		
		// Act
		bool tileExists = tileRegistry.TryGetTile(tile.TileId, out _); // We never added tile to the registry
		
		// Assert
		tileExists.Should().BeFalse();
	}

	[TestMethod]
	public void TryGetTile_SuppliesTileStoredInternally_WhenTileHasBeenAddedToRegistry()
	{
		// Arrange
		TileRegistry tileRegistry = new(Substitute.For<ILogger>());
		Tile airTile = new AirTile();
		
		// Act
		tileRegistry.RegisterTile(airTile);
		tileRegistry.TryGetTile(airTile.TileId, out Tile? tile);
		
		// Assert
		tile.Should().Be(airTile);
	}

	[TestMethod]
	public void TryGetTile_SuppliesNullTile_WhenTileHasNotBeenAddedToRegistry()
	{
		// Arrange
		TileRegistry tileRegistry = new(Substitute.For<ILogger>());
		Tile airTile = new AirTile();
		
		// Act
		tileRegistry.TryGetTile(airTile.TileId, out Tile? tile);
		
		// Assert
		tile.Should().BeNull();
	}

	[TestMethod]
	public void TryGetTile_LoggerDoesNotWarn_WhenTileHasBeenRegistered()
	{
		// Arrange
		ILogger logger = Substitute.For<ILogger>();
		TileRegistry tileRegistry = new(logger);
		Tile airTile = new AirTile();
		
		// Act
		tileRegistry.RegisterTile(airTile);
		tileRegistry.TryGetTile(airTile.TileId, out _);
		
		// Assert
		logger.DidNotReceive().Warning(Arg.Is<string>(x => x.Contains("Unable to get tile")), Arg.Any<string>());
	}

	[TestMethod]
	public void TryGetTile_LoggerWarns_WhenTileHasNotBeenRegistered()
	{
		// Arrange
		ILogger logger = Substitute.For<ILogger>();
		TileRegistry tileRegistry = new(logger);
		Tile airTile = new AirTile();
		
		// Act
		tileRegistry.TryGetTile(airTile.TileId, out _);
		
		// Assert
		logger.Received(1).Warning(Arg.Is<string>(x => x.Contains("Unable to get tile")), Arg.Any<string>());
	}

	#endregion

	#region GetAllTilesOfType Tests

	[TestMethod]
	public void GetAllTilesOfType_ReturnsPopulatedList_WhenTilesOfGivenTypeHaveBeenRegistered()
	{
		// Arrange
		TileRegistry tileRegistry = new(Substitute.For<ILogger>());
		
		// Act
		tileRegistry.RegisterTile(new AirTile());		// Not ModelTile
		tileRegistry.RegisterTile(new BushTile());		// ModelTile
		tileRegistry.RegisterTile(new GrassTile());		// ModelTile
		tileRegistry.RegisterTile(new SmallRockTile()); // ModelTile
		List<ModelTile> tiles = tileRegistry.GetAllTilesOfType<ModelTile>();
		
		// Assert
		tiles.Count.Should().Be(3);
	}

	[TestMethod]
	public void GetAllTilesOfType_ReturnsEmptyList_WhenNoTilesOfGivenTypeHaveBeenRegistered()
	{
		// Arrange
		TileRegistry tileRegistry = new(Substitute.For<ILogger>());
		
		// Act
		tileRegistry.RegisterTile(new AirTile());
		tileRegistry.RegisterTile(new BushTile());
		List<GrassTile> tiles = tileRegistry.GetAllTilesOfType<GrassTile>();
		
		// Assert
		tiles.Count.Should().Be(0);
	}

	#endregion
}