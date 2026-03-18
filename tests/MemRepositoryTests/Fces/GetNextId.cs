using AirWeb.MemRepository.ComplianceRepositories;

namespace MemRepositoryTests.Fces;

public class GetNextId
{
    private FceMemRepository _repository = null!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetFceRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public void GivenExistingItems_ReturnsNextIdNumber()
    {
        // Arrange
        var maxId = _repository.Items.Max(item => item.Id);

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
