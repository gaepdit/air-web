using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.EfRepository.Repositories;

namespace EfRepositoryTests.WorkEntries;

public class GetNextId
{
    private WorkEntryRepository _repository = null!;

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
        await repositoryHelper.ClearTableAsync<WorkEntry, int>();

        // Act
        var result = _repository.GetNextId();

        // Assert
        result.Should().BeNull();
    }
}
