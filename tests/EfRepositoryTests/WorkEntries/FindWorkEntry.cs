using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.TestData.Entities;

namespace EfRepositoryTests.WorkEntries;

public class FindWorkEntry
{
    private IWorkEntryRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetWorkEntryRepository();

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
        result.Should().BeEquivalentTo(entry, options => options.Excluding(workEntry => workEntry.Facility));
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
