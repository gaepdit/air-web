using AirWeb.Domain.BaseEntities.Interfaces;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.LocalRepository.Repositories;
using AirWeb.TestData.Enforcement;
using GaEpd.AppLibrary.Domain.Repositories;


namespace LocalRepositoryTests.EnforcementAction;

[TestFixture]
public class GetConsentOrderTest
{
    private LocalEnforcementActionRepository _repository;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetEnforcementActionRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task GetConsentOrder_WhenIdExist()
    {
        // Arrange
        var existingCO = EnforcementActionData.GetData
            .OfType<ConsentOrder>()
            .First();

        // Act
        var results = await _repository.GetConsentOrder(existingCO.Id);

        // Assert
        results.Should().BeEquivalentTo(existingCO);
    }
    [Test]
    public async Task GetConsentOrder_WhenIdDoesNotExist()
    {
        // Arrange
        var nonExistingCO = Guid.NewGuid();

        /*// Act
        var results = await _repository.GetConsentOrder(nonExistingCO);

        //Assert
        results.Should().BeNull();*/

        // Act
        Func<Task> act = async () => await _repository.GetConsentOrder(nonExistingCO);

        //Assert
        await act.Should()
            .ThrowAsync<EntityNotFoundException<EnforcementAction>>()
            .WithMessage("Entity not found. Entity type: AirWeb.Domain.EnforcementEntities.EnforcementActions.EnforcementAction, id: " + nonExistingCO);
    }

}

