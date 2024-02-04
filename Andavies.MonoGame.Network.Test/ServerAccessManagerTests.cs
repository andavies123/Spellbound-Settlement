using Andavies.MonoGame.Network.Server;

namespace Andavies.MonoGame.Network.Test;

[TestClass]
public class ServerAccessManagerTests
{
	#region Helpers

	private static ILogger LoggerSubstitute => Substitute.For<ILogger>();

	#endregion
	
	#region AddToWhiteList Tests

	[TestMethod]
	public void AddToWhiteList_AddsUsernameToWhiteList()
	{
		// Arrange
		ServerAccessManager serverAccessManager = new(LoggerSubstitute);
		const string userName = "test username";

		// Act
		serverAccessManager.AddToWhiteList(userName);

		// Assert
		serverAccessManager.WhiteList.Should().Contain(userName);
	}

	[TestMethod]
	public void AddToWhiteList_DoesNotAddToBlackList()
	{
		// Arrange
		ServerAccessManager serverAccessManager = new(LoggerSubstitute);
		const string userName = "test username";
		
		// Act
		serverAccessManager.AddToWhiteList(userName);
		
		// Assert
		serverAccessManager.BlackList.Should().NotContain(userName);
	}

	[TestMethod]
	public void AddToWhiteList_DoesNotAddSameUsernameToWhiteList()
	{
		// Arrange
		ServerAccessManager serverAccessManager = new(LoggerSubstitute);
		const string userName = "test username";
		
		// Act
		serverAccessManager.AddToWhiteList(userName);
		serverAccessManager.AddToWhiteList(userName);
		
		// Assert
		serverAccessManager.WhiteList.Count.Should().Be(1);
	}

	[TestMethod]
	public void AddToWhiteList_LoggerLogsInfoContainingUserName()
	{
		// Arrange
		ILogger logger = LoggerSubstitute; 
		ServerAccessManager serverAccessManager = new(logger);
		const string userName = "test username";

		// Act
		serverAccessManager.AddToWhiteList(userName);

		// Assert
		logger.Received(1).Information(Arg.Any<string>(), Arg.Is<string>(x => x == userName));
	}

	#endregion

	#region RemoveFromWhiteList Test

	[TestMethod]
	public void RemoveFromWhiteList_RemovesUsernameFromWhiteList()
	{
		// Arrange
		ServerAccessManager serverAccessManager = new(LoggerSubstitute);
		const string userName = "test username";
		
		// Act
		serverAccessManager.AddToWhiteList(userName);
		serverAccessManager.RemoveFromWhiteList(userName);
		
		// Assert
		serverAccessManager.WhiteList.Should().NotContain(userName);
	}

	[TestMethod]
	public void RemoveFromWhiteList_DoesNotRemoveUsernameFromBlackList()
	{
		// Arrange
		ServerAccessManager serverAccessManager = new(LoggerSubstitute);
		const string userName = "test username";

		// Act
		serverAccessManager.AddToBlackList(userName);
		serverAccessManager.RemoveFromWhiteList(userName);

		// Assert
		serverAccessManager.BlackList.Should().Contain(userName);
	}
	
	[TestMethod]
	public void RemoveFromWhiteList_LoggerLogsInfoContainingUserName()
	{
		// Arrange
		ILogger logger = LoggerSubstitute; 
		ServerAccessManager serverAccessManager = new(logger);
		const string userName = "test username";

		// Act
		serverAccessManager.AddToWhiteList(userName);
		logger.ClearReceivedCalls();
		serverAccessManager.RemoveFromWhiteList(userName);

		// Assert
		logger.Received(1).Information(Arg.Any<string>(), Arg.Is<string>(x => x == userName));
	}

	#endregion
	
	#region ClearWhiteList Tests

	[TestMethod]
	public void ClearWhiteList_RemovesAllUsernamesFromWhiteList()
	{
		// Arrange
		ServerAccessManager serverAccessManager = new(LoggerSubstitute);

		// Act
		serverAccessManager.AddToWhiteList("white 1");
		serverAccessManager.AddToWhiteList("white 2");
		serverAccessManager.AddToWhiteList("white 3");
		serverAccessManager.ClearWhiteList();

		// Assert
		serverAccessManager.WhiteList.Count.Should().Be(0);
	}

	[TestMethod]
	public void ClearWhiteList_DoesNotRemoveAnyUsernamesFromBlackList()
	{
		// Arrange
		ServerAccessManager serverAccessManager = new(LoggerSubstitute);

		// Act
		serverAccessManager.AddToWhiteList("white 1");
		serverAccessManager.AddToWhiteList("white 2");
		serverAccessManager.AddToBlackList("black 1");
		serverAccessManager.AddToBlackList("black 2");
		serverAccessManager.ClearWhiteList();

		// Assert
		serverAccessManager.BlackList.Count.Should().Be(2);
	}

	[TestMethod]
	public void ClearWhiteList_LoggerLogsInfoContainingNumberOfRemovedUsernames()
	{
		// Arrange
		ILogger logger = LoggerSubstitute;
		ServerAccessManager serverAccessManager = new(logger);

		// Act
		serverAccessManager.AddToWhiteList("white 1");
		serverAccessManager.AddToWhiteList("white 2");
		serverAccessManager.AddToWhiteList("white 3");
		serverAccessManager.ClearWhiteList();

		// Assert
		logger.Received(1).Information(Arg.Any<string>(), Arg.Is(3));
	}
	
	#endregion
	
	#region AddToBlackList Tests

	[TestMethod]
	public void AddToBlackList_AddsUsernameToBlackList()
	{
		// Arrange
		ServerAccessManager serverAccessManager = new(LoggerSubstitute);
		const string userName = "test username";

		// Act
		serverAccessManager.AddToBlackList(userName);

		// Assert
		serverAccessManager.BlackList.Should().Contain(userName);
	}

	[TestMethod]
	public void AddToBlackList_DoesNotAddToWhiteList()
	{
		// Arrange
		ServerAccessManager serverAccessManager = new(LoggerSubstitute);
		const string userName = "test username";
		
		// Act
		serverAccessManager.AddToBlackList(userName);
		
		// Assert
		serverAccessManager.WhiteList.Should().NotContain(userName);
	}

	[TestMethod]
	public void AddToBlackList_DoesNotAddSameUsernameToBlackList()
	{
		// Arrange
		ServerAccessManager serverAccessManager = new(LoggerSubstitute);
		const string userName = "test username";
		
		// Act
		serverAccessManager.AddToBlackList(userName);
		serverAccessManager.AddToBlackList(userName);
		
		// Assert
		serverAccessManager.BlackList.Count.Should().Be(1);
	}

	[TestMethod]
	public void AddToBlackList_LoggerLogsInfoContainingUserName()
	{
		// Arrange
		ILogger logger = LoggerSubstitute; 
		ServerAccessManager serverAccessManager = new(logger);
		const string userName = "test username";

		// Act
		serverAccessManager.AddToBlackList(userName);

		// Assert
		logger.Received(1).Information(Arg.Any<string>(), Arg.Is<string>(x => x == userName));
	}

	#endregion

	#region RemoveFromBlackList Test

	[TestMethod]
	public void RemoveFromBlackList_RemovesUsernameFromBlackList()
	{
		// Arrange
		ServerAccessManager serverAccessManager = new(LoggerSubstitute);
		const string userName = "test username";
		
		// Act
		serverAccessManager.AddToBlackList(userName);
		serverAccessManager.RemoveFromBlackList(userName);
		
		// Assert
		serverAccessManager.BlackList.Should().NotContain(userName);
	}

	[TestMethod]
	public void RemoveFromBlackList_DoesNotRemoveUsernameFromWhiteList()
	{
		// Arrange
		ServerAccessManager serverAccessManager = new(LoggerSubstitute);
		const string userName = "test username";

		// Act
		serverAccessManager.AddToWhiteList(userName);
		serverAccessManager.RemoveFromBlackList(userName);

		// Assert
		serverAccessManager.WhiteList.Should().Contain(userName);
	}
	
	[TestMethod]
	public void RemoveFromBlackList_LoggerLogsInfoContainingUserName()
	{
		// Arrange
		ILogger logger = LoggerSubstitute; 
		ServerAccessManager serverAccessManager = new(logger);
		const string userName = "test username";

		// Act
		serverAccessManager.AddToBlackList(userName);
		logger.ClearReceivedCalls();
		serverAccessManager.RemoveFromBlackList(userName);

		// Assert
		logger.Received(1).Information(Arg.Any<string>(), Arg.Is<string>(x => x == userName));
	}

	#endregion

	#region ClearBlackList Tests

	[TestMethod]
	public void ClearBlackList_RemovesAllUsernamesFromBlackList()
	{
		// Arrange
		ServerAccessManager serverAccessManager = new(LoggerSubstitute);

		// Act
		serverAccessManager.AddToBlackList("black 1");
		serverAccessManager.AddToBlackList("black 2");
		serverAccessManager.AddToBlackList("black 3");
		serverAccessManager.ClearBlackList();

		// Assert
		serverAccessManager.BlackList.Count.Should().Be(0);
	}

	[TestMethod]
	public void ClearBlackList_DoesNotRemoveAnyUsernamesFromWhiteList()
	{
		// Arrange
		ServerAccessManager serverAccessManager = new(LoggerSubstitute);

		// Act
		serverAccessManager.AddToBlackList("black 1");
		serverAccessManager.AddToBlackList("black 2");
		serverAccessManager.AddToWhiteList("white 1");
		serverAccessManager.AddToWhiteList("white 2");
		serverAccessManager.ClearBlackList();

		// Assert
		serverAccessManager.WhiteList.Count.Should().Be(2);
	}

	[TestMethod]
	public void ClearBlackList_LoggerLogsInfoContainingNumberOfRemovedUsernames()
	{
		// Arrange
		ILogger logger = LoggerSubstitute;
		ServerAccessManager serverAccessManager = new(logger);

		// Act
		serverAccessManager.AddToBlackList("black 1");
		serverAccessManager.AddToBlackList("black 2");
		serverAccessManager.AddToBlackList("black 3");
		serverAccessManager.ClearBlackList();

		// Assert
		logger.Received(1).Information(Arg.Any<string>(), Arg.Is(3));
	}
	
	#endregion

	#region IsAllowed Tests

	[TestMethod]
	public void IsAllowed_AllowsUserIfTheyAreNotAddedToEitherWhiteListOrBlackList_WhenBothListsAreEnabled()
	{
		// Arrange
		ServerAccessManager serverAccessManager = new(LoggerSubstitute);
		const string userName = "test name";
		
		// Act
		serverAccessManager.WhiteListEnabled = true;
		serverAccessManager.BlackListEnabled = true;
		
		// Assert
		serverAccessManager.IsAllowed(userName).Should().BeTrue();
	}

	[TestMethod]
	public void IsAllowed_AllowsUserIfTheyAreAddedToWhiteListAndNotBlackList_WhenBothListsAreEnabled()
	{
		// Arrange
		ServerAccessManager serverAccessManager = new(LoggerSubstitute);
		const string userName = "test name";
		
		// Act
		serverAccessManager.WhiteListEnabled = true;
		serverAccessManager.BlackListEnabled = true;
		serverAccessManager.AddToWhiteList(userName);
		
		// Assert
		serverAccessManager.IsAllowed(userName).Should().BeTrue();
	}

	[TestMethod]
	public void IsAllowed_DoesNotAllowUserIfNameAreNotOnWhiteListButOnBlackList_WhenBothListsAreEnabled()
	{
		// Arrange
		ServerAccessManager serverAccessManager = new(LoggerSubstitute);
		const string userName = "test name";
		
		// Act
		serverAccessManager.WhiteListEnabled = true;
		serverAccessManager.BlackListEnabled = true;
		serverAccessManager.AddToBlackList(userName);
		
		// Assert
		serverAccessManager.IsAllowed(userName).Should().BeFalse();
	}

	[TestMethod]
	public void IsAllowed_DoesNotAllowUserIfNameIsOnBlackListButNotWhiteList_WhenWhiteListIsEnabledAndBlackListIsDisabled()
	{
		// Arrange
		ServerAccessManager serverAccessManager = new(LoggerSubstitute);
		const string userName = "test name";
		
		// Act
		serverAccessManager.WhiteListEnabled = true;
		serverAccessManager.BlackListEnabled = false;
		serverAccessManager.AddToBlackList(userName);
		
		// Assert
		serverAccessManager.IsAllowed(userName).Should().BeFalse();
	}

	#endregion
}