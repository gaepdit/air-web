using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
using AirWeb.LocalRepository.Repositories;
using AirWeb.TestData.Compliance;

namespace LocalRepositoryTests.ComplianceMonitoring;

public class GetWorkEntryType
{
    private LocalComplianceWorkRepository _repository = null!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetWorkEntryRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [TestCase(ComplianceWorkType.AnnualComplianceCertification)]
    [TestCase(ComplianceWorkType.Notification)]
    [TestCase(ComplianceWorkType.PermitRevocation)]
    [TestCase(ComplianceWorkType.SourceTestReview)]
    public async Task GivenExistingItem_ReturnsValue(ComplianceWorkType type)
    {
        // Arrange
        var entry = WorkEntryData.GetData.First(entry => entry.ComplianceWorkType.Equals(type));

        // Act
        var result = await _repository.GetComplianceWorkTypeAsync(entry.Id);

        // Assert
        result.Should().Be(type);
    }

    [Test]
    public async Task GivenNonexistentId_Throws()
    {
        // Act
        var func = async () => await _repository.GetComplianceWorkTypeAsync(id: 0);

        // Assert
        await func.Should().ThrowAsync<InvalidOperationException>();
    }
}
