using AirWeb.Domain.ComplianceEntities.ComplianceWork;
using AirWeb.EfRepository.Repositories;
using AirWeb.TestData.Compliance;

namespace EfRepositoryTests.ComplianceWork;

public class GetIncludeProperty
{
    private ComplianceWorkRepository _repository = null!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetWorkEntryRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenRequestingProperty_ReturnsEntityWithProperty()
    {
        // Arrange
        var expected = WorkEntryData.GetData.FirstOrDefault(entry => entry.Comments.Count > 0);
        if (expected is null) Assert.Inconclusive("Test can only run if at least one Work Entry has comments.");

        // Act
        var result = await _repository.GetAsync(expected.Id, IComplianceWorkRepository.IncludeComments);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(expected);
        result.Comments.Should().HaveCount(expected.Comments.Count);
    }

    [Test]
    public async Task WhenNotRequestingProperty_ReturnsEntityWithoutProperty()
    {
        // Arrange
        var expected = WorkEntryData.GetData.First();

        // Act
        var result = await _repository.GetAsync(expected.Id, []);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(expected, options => options.Excluding(entry => entry.Comments));
        result.Comments.Should().BeEmpty();
    }
}
