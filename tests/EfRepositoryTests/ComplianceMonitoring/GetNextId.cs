using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
using AirWeb.EfRepository.Repositories;

namespace EfRepositoryTests.ComplianceMonitoring;

public class GetNextId
{
    private ComplianceWorkRepository _repository = null!;

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public void GivenExistingItems_ReturnsNull()
    {
        // Arrange
        _repository = RepositoryHelper.CreateRepositoryHelper().GetComplianceWorkRepository();

        // Act
        var result = _repository.GetNextId();

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task GivenEmptyItems_ReturnsNull()
    {
        // Arrange
        var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        _repository = repositoryHelper.GetComplianceWorkRepository();
        await repositoryHelper.ClearTableAsync<ComplianceWork, int>();

        // Act
        var result = _repository.GetNextId();

        // Assert
        result.Should().BeNull();
    }
}
