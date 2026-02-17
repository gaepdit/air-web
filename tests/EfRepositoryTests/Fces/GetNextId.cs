using AirWeb.Domain.Compliance.ComplianceEntities.Fces;
using AirWeb.EfRepository.Repositories;

namespace EfRepositoryTests.Fces;

public class GetNextId
{
    private FceRepository _repository = null!;

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public void GivenExistingItems_ReturnsNull()
    {
        // Arrange
        _repository = RepositoryHelper.CreateRepositoryHelper().GetFceRepository();

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
        _repository = repositoryHelper.GetFceRepository();
        await repositoryHelper.ClearTableAsync<Fce, int>();

        // Act
        var result = _repository.GetNextId();

        // Assert
        result.Should().BeNull();
    }
}
