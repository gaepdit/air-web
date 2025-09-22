using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.LocalRepository.Repositories;
using AirWeb.TestData.Enforcement;

namespace LocalRepositoryTests.EnforcementActions;

[TestFixture]
public class FindConsentOrderTests
{
    private LocalEnforcementActionRepository _repository;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetEnforcementActionRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task FindConsentOrder_WhenIdExist_ReturnConsentOrder()
    {
        // Arrange
        var existingOrder = EnforcementActionData.GetData
            .OfType<ConsentOrder>()
            .First();

        // Act
        var results = await _repository.FindConsentOrder(existingOrder.Id);

        // Assert
        results.Should().BeEquivalentTo(existingOrder);
    }

    [Test]
    public async Task FindConsentOrder_WhenIdDoesNotExist_ReturnsNull()
    {
        // Arrange
        var nonExistingOrderId = Guid.NewGuid();

        // Act
        var results = await _repository.FindConsentOrder(nonExistingOrderId);

        //Assert
        results.Should().BeNull();
    }
}
