using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AirWeb.EfRepository.Repositories;
using AirWeb.TestData.Compliance;

namespace EfRepositoryTests.ComplianceMonitoring;

public class FindOfType
{
    private ComplianceWorkRepository _repository = null!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetComplianceWorkRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task GivenExistingItem_ReturnsTrue()
    {
        // Arrange
        var expected = ComplianceWorkData.GetData.First(work => work.ComplianceWorkType.Equals(ComplianceWorkType.Notification));

        // Act
        var result = await _repository.FindAsync<Notification>(expected.Id, includeExtras: false);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(expected, options => options.Excluding(work => work.Comments));
        result!.ComplianceWorkType.Should().Be(ComplianceWorkType.Notification);
        result.Should().BeOfType<Notification>();
    }

    [Test]
    public async Task GivenNonexistentId_ReturnsNull()
    {
        // Act
        var result = await _repository.FindAsync<Notification>(id: 0, includeExtras: false);

        // Assert
        result.Should().BeNull();
    }
}
