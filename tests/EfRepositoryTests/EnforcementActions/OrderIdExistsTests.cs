using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.EfRepository.Repositories;
using AirWeb.TestData.Enforcement;

namespace EfRepositoryTests.EnforcementActions;

[TestFixture]
public class OrderIdExistsTests
{
    private EnforcementActionRepository _repository;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementActionRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task OrderIdExist_WhenIdExist_ReturnTrue()
    {
        // Arrange
        var existingOrder = EnforcementActionData.GetData
            .OfType<ConsentOrder>()
            .First(e => e.OrderId != null);

        // Act
        var results = await _repository.OrderIdExists(existingOrder.OrderId!.Value, Guid.NewGuid());

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
