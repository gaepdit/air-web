using AirWeb.EfRepository.ComplianceRepositories;
using AirWeb.TestData.Compliance;

namespace EfRepositoryTests.ComplianceMonitoring;

public class FindComplianceWork
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
        var expected = ComplianceWorkData.GetData.First();

        // Act
        var result = await _repository.FindAsync(expected.Id);

        // Assert
        result.Should().BeEquivalentTo(expected, options => options.Excluding(work => work.Comments));
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
