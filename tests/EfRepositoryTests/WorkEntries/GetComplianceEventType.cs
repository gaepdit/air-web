using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.TestData.Entities;

namespace EfRepositoryTests.WorkEntries;

public class GetComplianceEventType
{
    private IWorkEntryRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetWorkEntryRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [TestCase(ComplianceEventType.AnnualComplianceCertification)]
    [TestCase(ComplianceEventType.SourceTestReview)]
    public async Task GivenExistingItem_ReturnsValue(ComplianceEventType type)
    {
        // Arrange
        var entry = WorkEntryData.GetData.First(entry =>
            entry.WorkEntryType.Equals(WorkEntryType.ComplianceEvent) &&
            ((ComplianceEvent)entry).ComplianceEventType == type);

        // Act
        var result = await _repository.GetComplianceEventTypeAsync(entry.Id);

        // Assert
        result.Should().Be(type);
    }

    [Test]
    public async Task GivenNonexistentId_ReturnsNull()
    {
        // Act
        var func = async () => await _repository.GetComplianceEventTypeAsync(id: 0);

        // Assert
        await func.Should().ThrowAsync<InvalidOperationException>();
    }
}
