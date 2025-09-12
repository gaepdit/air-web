using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.EfRepository.Repositories;
using AirWeb.LocalRepository.Repositories;
using AirWeb.TestData.Enforcement;
using GaEpd.AppLibrary.Domain.Repositories;

namespace EfRepositoryTests.EnforcementActions;

[TestFixture]
public class OrderIdExistTest
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
        var existingCO = EnforcementActionData.GetData
            .OfType<ConsentOrder>()
            .First();

        // Act
        var results = await _repository.OrderIdExists(existingCO.OrderId, Guid.NewGuid());

        // Assert
        results.Should().BeTrue();
    }
    [Test]
    public async Task OrderIdExist_WhenIdDoesNotExist_ReturnFalse()
    {
        // Arrange
        short nonExistingOrderId = 9999;

        // Act
        var results = await _repository.OrderIdExists(nonExistingOrderId, Guid.NewGuid());

        // Assert
        results.Should().BeFalse();
    }

}

