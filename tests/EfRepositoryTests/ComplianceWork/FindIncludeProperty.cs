using AirWeb.Domain.ComplianceEntities.ComplianceWork;
using AirWeb.EfRepository.Repositories;
using AirWeb.TestData.Compliance;

namespace EfRepositoryTests.ComplianceWork;

public class FindIncludeProperty
{
    private WorkEntryRepository _repository = null!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetWorkEntryRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenRequestingProperty_ReturnsEntityWithProperty()
    {
        // Arrange
        var expected = WorkEntryData.GetData.FirstOrDefault(entry =>
            entry is { WorkEntryType: WorkEntryType.Notification, Comments.Count: > 0 });
        if (expected is null) Assert.Inconclusive("Test can only run if at least one Work Entry has comments.");

        // Act
        var result = await _repository.FindAsync(expected.Id, includeProperties: IWorkEntryRepository.IncludeComments);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(expected);
        result.Comments.Should().HaveCount(expected.Comments.Count);
    }

    [Test]
    public async Task GetWithExtras_ReturnsEntityWithAdditionalProperties()
    {
        // Arrange
        var expected = WorkEntryData.GetData.FirstOrDefault(entry =>
            entry is { WorkEntryType: WorkEntryType.Notification, Comments.Count: > 0 });
        if (expected is null) Assert.Inconclusive("Test can only run if at least one Work Entry has comments.");

        // Act
        var result = await _repository.FindAsync<Notification>(expected.Id, includeExtras: true);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(expected);
        result.Comments.Should().HaveCount(expected.Comments.Count);
    }

    [Test]
    public async Task WhenNotRequestingExtras_ReturnsEntityWithoutAdditionalProperties()
    {
        // Arrange
        var expected = WorkEntryData.GetData.FirstOrDefault(entry =>
            entry is { WorkEntryType: WorkEntryType.Notification, Comments.Count: > 0 });
        if (expected is null) Assert.Inconclusive("Test can only run if at least one Work Entry has comments.");

        // Act
        var result = await _repository.FindAsync(expected!.Id, includeProperties: []);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(expected, options => options.Excluding(entry => entry.Comments));
        result.Comments.Should().BeEmpty();
    }

    [Test]
    public async Task WhenNotRequestingExtras_ForChildEntity_ReturnsEntityWithoutAdditionalProperties()
    {
        // Arrange
        var expected = WorkEntryData.GetData.FirstOrDefault(entry =>
            entry is { WorkEntryType: WorkEntryType.Notification, Comments.Count: > 0 });
        if (expected is null) Assert.Inconclusive("Test can only run if at least one Work Entry has comments.");

        // Act
        var result = await _repository.FindAsync<Notification>(expected.Id, includeExtras: false);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(expected, options => options.Excluding(entry => entry.Comments));
        result.Comments.Should().BeEmpty();
    }
}
