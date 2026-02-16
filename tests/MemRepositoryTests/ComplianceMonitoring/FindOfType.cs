using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AirWeb.TestData.Compliance;

namespace MemRepositoryTests.ComplianceMonitoring;

public class FindOfType
{
    [Test]
    public async Task GivenExistingItem_ReturnsTrue()
    {
        // Arrange
        await using var repository = RepositoryHelper.GetComplianceWorkRepository();
        var work = ComplianceWorkData.GetData.First(complianceWork => complianceWork.ComplianceWorkType.Equals(ComplianceWorkType.Notification));

        // Act
        var result = await repository.FindAsync<Notification>(work.Id, includeExtras: true);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(work);
        result!.ComplianceWorkType.Should().Be(ComplianceWorkType.Notification);
        result.Should().BeOfType<Notification>();
    }

    [Test]
    public async Task GivenNonexistentId_ReturnsNull()
    {
        // Arrange
        await using var repository = RepositoryHelper.GetComplianceWorkRepository();

        // Act
        var result = await repository.FindAsync<Notification>(id: 0, includeExtras: true);

        // Assert
        result.Should().BeNull();
    }
}
