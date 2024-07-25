using AirWeb.LocalRepository.Repositories;

namespace LocalRepositoryTests.WorkEntries;

public class GetNextId
{
    private LocalWorkEntryRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetWorkEntryRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public void GivenExistingItems_ReturnsNextIdNumber()
    {
        // Arrange
        var maxId = _repository.Items.Max(entry => entry.Id);

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
