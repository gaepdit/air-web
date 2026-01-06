using AirWeb.LocalRepository.Repositories;
using AirWeb.TestData.Compliance;

namespace LocalRepositoryTests.ComplianceMonitoring;

public class FindWorkEntry
{
    private LocalComplianceWorkRepository _repository = null!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetWorkEntryRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task GivenExistingItem_ReturnsTrue()
    {
        // Arrange
        var entry = WorkEntryData.GetData.First();

        // Act
        var result = await _repository.FindAsync(entry.Id);

        // Assert
        result.Should().BeEquivalentTo(entry);
    }

    [Test]
    public async Task GivenNonexistentId_ReturnsNull()
    {
        // Act
        var result = await _repository.FindAsync(id: 0);

        // Assert
        result.Should().BeNull();
    }
}
