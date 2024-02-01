using Andavies.SpellboundSettlement.GameWorld.Wizards;

namespace Andavies.SpellboundSettlement.GameWorld.Test;

[TestClass]
public class WizardManagerTests
{
	#region AddOrUpdateWizard Tests

	[TestMethod]
	public void AddOrUpdateWizard_AddsNewWizardToInternalCollection()
	{
		// Arrange
		WizardManager wizardManager = new();
		Wizard wizard = new EarthWizard();
		
		// Act
		wizardManager.AddOrUpdateWizard(wizard);

		// Assert
		wizardManager.AllWizards.Should().ContainKey(wizard.Data.Id);
	}

	[TestMethod]
	public void AddOrUpdateWizard_UpdatesExistingWizardInInternalCollection()
	{
		// Arrange
		WizardManager wizardManager = new();
		Wizard wizard1 = new EarthWizard();
		const string changedName = "Changed Name";
		
		// Act
		wizardManager.AddOrUpdateWizard(wizard1);
		wizard1.Data.Name = changedName;
		wizardManager.AddOrUpdateWizard(wizard1);

		// Assert
		wizardManager.AllWizards[wizard1.Data.Id].Data.Name.Should().Be(changedName);
	}
	
	[TestMethod]
	public void AddOrUpdateWizard_RaisesWizardUpdatedEvent()
	{
		// Arrange
		WizardManager wizardManager = new();
		Wizard wizard = new EarthWizard();
		bool eventRaised = false;
		wizardManager.WizardUpdated += _ => eventRaised = true; 
		
		// Act
		wizardManager.AddOrUpdateWizard(wizard);

		// Assert
		eventRaised.Should().BeTrue();
	}

	[TestMethod]
	public void AddOrUpdateWizard_WizardUpdatedEventContainsUpdatedWizard()
	{
		// Arrange
		WizardManager wizardManager = new();
		Wizard wizard = new EarthWizard();
		Wizard wizardFromEvent = new EarthWizard();
		wizardManager.WizardUpdated += x => wizardFromEvent = x;
		
		// Act
		wizardManager.AddOrUpdateWizard(wizard);
		
		// Assert
		wizardFromEvent.Should().Be(wizard);
	}
	
	#endregion

	#region RemoveWizard Tests

	[TestMethod]
	public void RemoveWizard_RemovesWizardFromInternalCollection()
	{
		// Arrange
		WizardManager wizardManager = new();
		Wizard wizard = new EarthWizard();
		
		// Act
		wizardManager.AddOrUpdateWizard(wizard);
		wizardManager.RemoveWizard(wizard.Data.Id);
		
		// Assert
		wizardManager.AllWizards.Should().NotContainKey(wizard.Data.Id);
	}

	[TestMethod]
	public void RemoveWizard_RaisesWizardRemoved_WhenWizardIsRemovedFromCollection()
	{
		// Arrange
		WizardManager wizardManager = new();
		Wizard wizard = new EarthWizard();
		bool eventRaised = false;
		wizardManager.WizardRemoved += _ => eventRaised = true;
		
		// Act
		wizardManager.AddOrUpdateWizard(wizard);
		wizardManager.RemoveWizard(wizard.Data.Id);
		
		// Assert
		eventRaised.Should().BeTrue();
	}

	[TestMethod]
	public void RemoveWizard_DoesNotRaiseWizardRemoved_WhenWizardIsNotRemovedFromCollection()
	{
		// Arrange
		WizardManager wizardManager = new();
		Wizard wizard = new EarthWizard();
		Wizard notAddedWizard = new EarthWizard();
		bool eventRaised = false;
		wizardManager.WizardRemoved += _ => eventRaised = true;
		
		// Act
		wizardManager.AddOrUpdateWizard(wizard);
		wizardManager.RemoveWizard(notAddedWizard.Data.Id);
		
		// Assert
		eventRaised.Should().BeFalse();
	}

	[TestMethod]
	public void RemoveWizard_WizardRemovedEventContainsTheRemovedWizard()
	{
		// Arrange
		WizardManager wizardManager = new();
		Wizard wizard = new EarthWizard();
		Wizard wizardFromEvent = new EarthWizard();
		wizardManager.WizardRemoved += x => wizardFromEvent = x;
		
		// Act
		wizardManager.AddOrUpdateWizard(wizard);
		wizardManager.RemoveWizard(wizard.Data.Id);
		
		// Assert
		wizardFromEvent.Should().Be(wizard);
	}

	#endregion

	#region WizardUpdated Event Raised Tests

	[TestMethod]
	public void WizardUpdated_RaisedWhenAddedWizardIsUpdated()
	{
		// Arrange
		WizardManager wizardManager = new();
		Wizard wizard = new EarthWizard();
		bool eventRaised = false;
		wizardManager.WizardUpdated += _ => eventRaised = true;
		
		// Act
		wizardManager.AddOrUpdateWizard(wizard);
		eventRaised = false; // Reset the flag
		wizard.Data.Name = nameof(WizardUpdated_RaisedWhenAddedWizardIsUpdated); // Unique name to this test
		wizard.Update(0); // Raises the Updated event
		
		// Assert
		eventRaised.Should().BeTrue();
	}
	
	[TestMethod]
	public void WizardUpdated_NotRaisedWhenRemovedWizardIsUpdated()
	{
		// Arrange
		WizardManager wizardManager = new();
		Wizard wizard = new EarthWizard();
		bool eventRaised = false;
		wizardManager.WizardUpdated += _ => eventRaised = true;
		
		// Act
		wizardManager.AddOrUpdateWizard(wizard);
		eventRaised = false; // Reset the flag
		wizard.Data.Name = nameof(WizardUpdated_RaisedWhenAddedWizardIsUpdated); // Unique name to this test
		wizardManager.RemoveWizard(wizard.Data.Id); // Remove wizard
		wizard.Update(0); // Raises the Updated event
		
		// Assert
		eventRaised.Should().BeFalse();
	}

	#endregion
}