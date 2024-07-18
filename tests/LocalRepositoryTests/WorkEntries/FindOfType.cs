using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.LocalRepository.Repositories;
using AirWeb.TestData.Entities;

namespace LocalRepositoryTests.WorkEntries;

public class FindOfType
{
    private LocalWorkEntryRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetWorkEntryRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task GivenExistingItem_ReturnsTrue()
    {
        // Arrange
        var entry = WorkEntryData.GetData.First(entry => entry.WorkEntryType.Equals(WorkEntryType.Notification));

        // Act
        var result = await _repository.FindAsync<Notification>(entry.Id);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(entry);
        result!.WorkEntryType.Should().Be(WorkEntryType.Notification);
        result.Should().BeOfType<Notification>();
    }


    [Test]
    public async Task GivenNonexistentId_ReturnsNull()
    {
        // Act
        var result = await _repository.FindAsync<Notification>(id: 0);

        // Assert
        result.Should().BeNull();
    }
}
