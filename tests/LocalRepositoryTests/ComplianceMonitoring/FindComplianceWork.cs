using AirWeb.LocalRepository.Repositories;
using AirWeb.TestData.Compliance;

namespace LocalRepositoryTests.ComplianceMonitoring;

public class FindComplianceWork
{
    private LocalComplianceWorkRepository _repository = null!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetComplianceWorkRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task GivenExistingItem_ReturnsTrue()
    {
        // Arrange
        var work = ComplianceWorkData.GetData.First();

        // Act
        var result = await _repository.FindAsync(work.Id);

        // Assert
        result.Should().BeEquivalentTo(work);
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
