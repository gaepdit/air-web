using AirWeb.LocalRepository.Repositories;

namespace LocalRepositoryTests.Fces;

public class GetNextId
{
    private LocalFceRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetFceRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public void GivenExistingItems_ReturnsNextIdNumber()
    {
        // Arrange
        var maxId = _repository.Items.Max(fce => fce.Id);

        // Act
        var result = _repository.GetNextId();

        // Assert
        result.Should().Be(maxId + 1);
    }

    [Test]
    public void GivenEmptyItems_ReturnsOne()
    {
        // Arrange
        _repository.Items.Clear();

        // Act
        var result = _repository.GetNextId();

        // Assert
        result.Should().Be(1);
    }
}
