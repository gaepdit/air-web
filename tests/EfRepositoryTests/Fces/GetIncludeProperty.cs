using AirWeb.Domain.Compliance.ComplianceEntities.Fces;
using AirWeb.EfRepository.Repositories;
using AirWeb.TestData.Compliance;

namespace EfRepositoryTests.Fces;

public class GetIncludeProperty
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
        var result = await _repository.GetAsync(expected.Id, IFceRepository.IncludeComments);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(expected);
        result.Comments.Should().HaveCount(expected.Comments.Count);
    }

    [Test]
    public async Task WhenNotRequestingProperty_ReturnsEntityWithoutProperty()
    {
        // Arrange
        var expected = FceData.GetData.First();

        // Act
        var result = await _repository.GetAsync(expected.Id, []);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(expected,
            options => options.Excluding(fce => fce.Comments));
        result.Comments.Should().BeEmpty();
    }
}
