using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AirWeb.MemRepository.Repositories;
using AirWeb.TestData.Compliance;

namespace MemRepositoryTests.ComplianceMonitoring;

public class GetComplianceWorkType
{
    private ComplianceWorkMemRepository _repository = null!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetComplianceWorkRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [TestCase(ComplianceWorkType.AnnualComplianceCertification)]
    [TestCase(ComplianceWorkType.Notification)]
    [TestCase(ComplianceWorkType.PermitRevocation)]
    [TestCase(ComplianceWorkType.SourceTestReview)]
    public async Task GivenExistingItem_ReturnsValue(ComplianceWorkType type)
    {
        // Arrange
        var work = ComplianceWorkData.GetData.First(complianceWork => complianceWork.ComplianceWorkType.Equals(type));

        // Act
        var result = await _repository.GetComplianceWorkTypeAsync(work.Id);

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
