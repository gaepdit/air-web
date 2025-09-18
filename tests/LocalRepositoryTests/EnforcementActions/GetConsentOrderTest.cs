using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.LocalRepository.Repositories;
using AirWeb.TestData.Enforcement;

namespace LocalRepositoryTests.EnforcementActions;

[TestFixture]
public class GetConsentOrderTest
{
    private LocalEnforcementActionRepository _repository;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetEnforcementActionRepository();

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
        var act = async () => await _repository.GetConsentOrder(nonExistingCO);

        //Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
