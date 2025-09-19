using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.LocalRepository.Repositories;
using AirWeb.TestData.Enforcement;

namespace LocalRepositoryTests.EnforcementActions;

[TestFixture]
public class OrderIdExistsTests
{
    private LocalEnforcementActionRepository _repository;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetEnforcementActionRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task OrderIdExist_WhenIdExist_ReturnTrue()
    {
        // Arrange
        var existingOrder = EnforcementActionData.GetData
            .OfType<ConsentOrder>()
            .First();

        // Act
        var results = await _repository.OrderIdExists(existingOrder.OrderId, Guid.NewGuid());

        // Assert
        results.Should().BeTrue();
    }

    [Test]
    public async Task OrderIdExist_WhenIdDoesNotExist_ReturnFalse()
    {
        // Arrange
        const short nonExistingOrderId = 9999;

        // Act
        var results = await _repository.OrderIdExists(nonExistingOrderId, Guid.NewGuid());

        // Assert
        results.Should().BeFalse();
    }
}
