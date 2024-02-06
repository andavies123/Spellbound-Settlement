using Andavies.MonoGame.Utilities.StateMachines;

namespace Andavies.MonoGame.Utilities.Test.StateMachines;

[TestClass]
public class StateMachineTests
{
	#region Helpers

	private static StateMachine<IState> NewStateMachine => new();
	private static IState NewStateSubstitute => Substitute.For<IState>();

	#endregion
    
	#region SetCurrentState Tests

	[TestMethod]
	public void SetCurrentState_CallsEndOnPreviousState()
	{
		// Arrange
		StateMachine<IState> stateMachine = NewStateMachine;
		IState previousState = NewStateSubstitute;
		IState newState = NewStateSubstitute;
		
		// Act
		stateMachine.SetCurrentState(previousState);
		previousState.ClearReceivedCalls();
		stateMachine.SetCurrentState(newState);
		
		// Assert
		previousState.Received(1).End();
	}

	[TestMethod]
	public void SetCurrentState_SetsTheCurrentStateToTheStatePassed()
	{
		// Arrange
		StateMachine<IState> stateMachine = NewStateMachine;
		IState state = NewStateSubstitute;
		
		// Act
		stateMachine.SetCurrentState(state);
		
		// Assert
		stateMachine.CurrentState.Should().Be(state);
	}

	[TestMethod]
	public void SetCurrentState_CallsBeginOnNewState()
	{
		// Arrange
		StateMachine<IState> stateMachine = new();
		IState state = NewStateSubstitute;
		
		// Act
		stateMachine.SetCurrentState(state);
		
		// Assert
		state.Received(1).Begin();
	}

	#endregion

	#region UpdateCurrentState Tests

	[TestMethod]
	public void UpdateCurrentState_CallsUpdateOnTheCurrentState()
	{
		// Arrange
		StateMachine<IState> stateMachine = NewStateMachine;
		IState state = NewStateSubstitute;

		// Act
		stateMachine.SetCurrentState(state);
		stateMachine.UpdateCurrentState(0f);

		// Assert
		state.Received(1).Update(Arg.Any<float>());
	}

	[TestMethod]
	public void UpdateCurrentState_PassesTheGivenDeltaTimeToTheCurrentState()
	{
		// Arrange
		StateMachine<IState> stateMachine = NewStateMachine;
		IState state = NewStateSubstitute;
		const float deltaTimeSeconds = 1.34f;
		
		// Act
		stateMachine.SetCurrentState(state);
		stateMachine.UpdateCurrentState(deltaTimeSeconds);
		
		// Assert
		state.Received(1).Update(deltaTimeSeconds);
	}

	#endregion
}