using AirWeb.LocalRepository.Repositories;

namespace LocalRepositoryTests.ComplianceMonitoring;

public class GetNextId
{
    private LocalComplianceWorkRepository _repository = null!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetWorkEntryRepository();

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
