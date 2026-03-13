using AirWeb.Domain.Compliance.EnforcementEntities.CaseFiles;
using AirWeb.EfRepository.ComplianceRepositories;

namespace EfRepositoryTests.CaseFiles;

public class GetNextId
{
    private CaseFileRepository _repository = null!;

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public void GivenExistingItems_ReturnsNull()
    {
        // Arrange
        _repository = RepositoryHelper.CreateRepositoryHelper().GetCaseFileRepository();

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
        _repository = repositoryHelper.GetCaseFileRepository();
        await repositoryHelper.ClearTableAsync<CaseFile, int>();

        // Act
        var result = _repository.GetNextId();

        // Assert
        result.Should().BeNull();
    }
}
