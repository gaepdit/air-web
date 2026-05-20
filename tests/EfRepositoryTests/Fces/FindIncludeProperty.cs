using AirWeb.Domain.Compliance.ComplianceEntities.Fces;
using AirWeb.EfRepository.ComplianceRepositories;
using AirWeb.TestData.Compliance;

namespace EfRepositoryTests.Fces;

public class FindIncludeProperty
{
    private FceRepository _repository = null!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetFceRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenRequestingProperty_ReturnsEntityWithProperty()
    {
        // Arrange
        var expected = FceData.GetData.FirstOrDefault(fce => fce.Comments.Count > 0);
        if (expected is null) Assert.Inconclusive("Test can only run if at least one FCE has comments.");

        // Act
        var result = await _repository.FindAsync(expected.Id, IFceRepository.IncludeComments);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(expected);
        result.Comments.Should().HaveCount(expected.Comments.Count);
    }

    [Test]
    public async Task GetWithExtras_ReturnsEntityWithAdditionalProperties()
    {
        // Arrange
        var expected = FceData.GetData.FirstOrDefault(fce => fce.Comments.Count > 0);
        if (expected is null) Assert.Inconclusive("Test can only run if at least one FCE has comments.");

        // Act
        var result = await _repository.FindWithDetailsAsync(expected.Id);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(expected);
        result.Comments.Should().HaveCount(expected.Comments.Count);
    }

    [Test]
    public async Task WhenNotRequestingExtras_ReturnsEntityWithoutAdditionalProperties()
    {
        // Arrange
        var expected = FceData.GetData.FirstOrDefault(fce => fce.Comments.Count > 0);
        if (expected is null) Assert.Inconclusive("Test can only run if at least one FCE has comments.");

        // Act
        var result = await _repository.FindAsync(expected.Id, []);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(expected,
            options => options.Excluding(fce => fce.Comments));
        result.Comments.Should().BeEmpty();
    }
}
