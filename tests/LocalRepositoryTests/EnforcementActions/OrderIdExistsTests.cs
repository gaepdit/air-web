using AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions;
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
    public async Task WhenIdExist_AndIgnoreIdNotMatched_ReturnTrue()
    {
        // Arrange
        var existingOrder = EnforcementActionData.GetData
            .OfType<ConsentOrder>()
            .First(e => e.OrderId != null);

        // Act
        var results = await _repository.OrderIdExists(existingOrder.OrderId!.Value, ignoreActionId: Guid.NewGuid());

        // Assert
        results.Should().BeTrue();
    }

    [Test]
    public async Task WhenIdDoesNotExist_AndIgnoreIdNotMatched_ReturnFalse()
    {
        // Arrange
        const short nonExistingOrderId = 9999;

        // Act
        var results = await _repository.OrderIdExists(nonExistingOrderId, ignoreActionId: Guid.NewGuid());

        // Assert
        results.Should().BeFalse();
    }

    [Test]
    public async Task WhenIdExist_AndIgnoreIdIsNull_ReturnTrue()
    {
        // Arrange
        var existingOrder = EnforcementActionData.GetData
            .OfType<ConsentOrder>()
            .First(e => e.OrderId != null);

        // Act
        var results = await _repository.OrderIdExists(existingOrder.OrderId!.Value, ignoreActionId: null);

        // Assert
        results.Should().BeTrue();
    }

    [Test]
    public async Task WhenIdDoesNotExist_AndIgnoreIdIsNull_ReturnFalse()
    {
        // Arrange
        const short nonExistingOrderId = 9999;

        // Act
        var results = await _repository.OrderIdExists(nonExistingOrderId, ignoreActionId: null);

        // Assert
        results.Should().BeFalse();
    }

    [Test]
    public async Task WhenIdExist_AndIgnoreIdMatched_ReturnFalse()
    {
        // Arrange
        var existingOrder = EnforcementActionData.GetData
            .OfType<ConsentOrder>()
            .First(e => e.OrderId != null);

        // Act
        var results = await _repository.OrderIdExists(existingOrder.OrderId!.Value, ignoreActionId: existingOrder.Id);

        // Assert
        results.Should().BeFalse();
    }

    [Test]
    public async Task WhenIdDoesNotExist_AndIgnoreIdExists_ReturnFalse()
    {
        // Arrange
        var existingOrder = EnforcementActionData.GetData
            .OfType<ConsentOrder>()
            .First();
        const short nonExistingOrderId = 9999;

        // Act
        var results = await _repository.OrderIdExists(nonExistingOrderId, ignoreActionId: existingOrder.Id);

        // Assert
        results.Should().BeFalse();
    }
}
