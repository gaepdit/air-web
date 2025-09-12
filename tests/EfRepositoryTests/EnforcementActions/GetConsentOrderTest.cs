using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.EfRepository.Repositories;
using AirWeb.LocalRepository.Repositories;
using AirWeb.TestData.Enforcement;
using GaEpd.AppLibrary.Domain.Repositories;

namespace EfRepositoryTests.EnforcementActions;

[TestFixture]
public class GetConsentOrderTest
{
    private EnforcementActionRepository _repository;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementActionRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task GetConsentOrder_WhenIdExist_ReturnConsentOrder()
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
    public async Task GetConsentOrder_WhenIdDoesNotExist_ThrowAsync()
    {
        // Arrange
        var nonExistingCO = Guid.NewGuid();

        // Act
        Func<Task> act = async () => await _repository.GetConsentOrder(nonExistingCO);

        //Assert
        await act.Should()
            .ThrowAsync<EntityNotFoundException<EnforcementAction>>()
            .WithMessage("Entity not found. Entity type: AirWeb.Domain.EnforcementEntities.EnforcementActions.EnforcementAction, id: " + nonExistingCO);
    }
}
