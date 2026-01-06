using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
using AirWeb.TestData.Compliance;

namespace LocalRepositoryTests.ComplianceMonitoring;

public class FindOfType
{
    [Test]
    public async Task GivenExistingItem_ReturnsTrue()
    {
        // Arrange
        await using var repository = RepositoryHelper.GetWorkEntryRepository();
        var entry = WorkEntryData.GetData.First(entry => entry.ComplianceWorkType.Equals(ComplianceWorkType.Notification));

        // Act
        var result = await repository.FindAsync<Notification>(entry.Id, includeExtras: true);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(entry);
        result!.ComplianceWorkType.Should().Be(ComplianceWorkType.Notification);
        result.Should().BeOfType<Notification>();
    }

    [Test]
    public async Task GivenNonexistentId_ReturnsNull()
    {
        // Arrange
        await using var repository = RepositoryHelper.GetWorkEntryRepository();

        // Act
        var result = await repository.FindAsync<Notification>(id: 0, includeExtras: true);

        // Assert
        result.Should().BeNull();
    }
}
