using AirWeb.Domain.ComplianceEntities.ComplianceWork;
using AirWeb.EfRepository.Repositories;
using AirWeb.TestData.Compliance;

namespace EfRepositoryTests.ComplianceWork;

public class GetWorkEntryType
{
    private WorkEntryRepository _repository = null!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetWorkEntryRepository();

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
        var result = await _repository.GetWorkEntryTypeAsync(entry.Id);

        // Assert
        result.Should().Be(type);
    }

    [Test]
    public async Task GivenNonexistentId_Throws()
    {
        // Act
        var func = async () => await _repository.GetWorkEntryTypeAsync(id: 0);

        // Assert
        await func.Should().ThrowAsync<InvalidOperationException>();
    }
}
