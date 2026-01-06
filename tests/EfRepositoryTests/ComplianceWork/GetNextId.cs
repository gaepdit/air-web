using AirWeb.Domain.ComplianceEntities.ComplianceWork;
using AirWeb.EfRepository.Repositories;

namespace EfRepositoryTests.ComplianceWork;

public class GetNextId
{
    private ComplianceWorkRepository _repository = null!;

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public void GivenExistingItems_ReturnsNull()
    {
        // Arrange
        _repository = RepositoryHelper.CreateRepositoryHelper().GetWorkEntryRepository();

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
        _repository = repositoryHelper.GetWorkEntryRepository();
        await repositoryHelper.ClearTableAsync<AirWeb.Domain.ComplianceEntities.ComplianceWork.ComplianceWork, int>();

        // Act
        var result = _repository.GetNextId();

        // Assert
        result.Should().BeNull();
    }
}
