using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.TestData.Entities;

namespace LocalRepositoryTests.WorkEntries;

public class FindOfType
{
    [Test]
    public async Task GivenExistingItem_ReturnsTrue()
    {
        // Arrange
        await using var repository = RepositoryHelper.GetWorkEntryRepository();
        var entry = WorkEntryData.GetData.First(entry => entry.WorkEntryType.Equals(WorkEntryType.Notification));

        // Act
        var result = await repository.FindAsync<Notification>(entry.Id);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(entry);
        result!.WorkEntryType.Should().Be(WorkEntryType.Notification);
        result.Should().BeOfType<Notification>();
    }

    [Test]
    public async Task GivenNonexistentId_ReturnsNull()
    {
        // Arrange
        await using var repository = RepositoryHelper.GetWorkEntryRepository();

        // Act
        var result = await repository.FindAsync<Notification>(id: 0);

        // Assert
        result.Should().BeNull();
    }
}
