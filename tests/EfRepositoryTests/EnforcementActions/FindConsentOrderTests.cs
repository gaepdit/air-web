using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.EfRepository.Repositories;
using AirWeb.TestData.Enforcement;

namespace EfRepositoryTests.EnforcementActions;

[TestFixture]
public class FindConsentOrderTests
{
    private EnforcementActionRepository _repository;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementActionRepository();

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
        using var scope = new AssertionScope();
        results.Should().NotBeNull();
        results.StipulatedPenalties.Should().BeEquivalentTo(existingOrder.StipulatedPenalties);
        results.Id.Should().Be(existingOrder.Id);
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
