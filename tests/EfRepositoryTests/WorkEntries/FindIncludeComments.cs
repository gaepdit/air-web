using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.TestData.Entities;

namespace EfRepositoryTests.WorkEntries;

public class FindIncludeComments
{
    private IWorkEntryRepository _repository = default!;

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
        var result = await _repository.FindAsync(expected.Id, IWorkEntryRepository.IncludeComments);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(expected, options => options.Excluding(entry => entry.Facility));
        result!.Comments.Count.Should().Be(expected.Comments.Count);
    }

    [Test]
    public async Task GetWithComments_ReturnsEntityWithComments()
    {
        // Arrange
        var expected = WorkEntryData.GetData.FirstOrDefault(entry =>
            entry is { WorkEntryType: WorkEntryType.Notification, Comments.Count: > 0 });
        if (expected is null) Assert.Inconclusive("Test can only run if at least one Work Entry has comments.");

        // Act
        var result = await _repository.FindWithCommentsAsync<Notification>(expected.Id);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(expected, options => options.Excluding(entry => entry.Facility));
        result!.Comments.Count.Should().Be(expected.Comments.Count);
    }

    [Test]
    public async Task WhenNotRequestingProperty_ReturnsEntityWithoutProperty()
    {
        // Arrange
        var expected = WorkEntryData.GetData.FirstOrDefault(entry =>
            entry is { WorkEntryType: WorkEntryType.Notification, Comments.Count: > 0 });
        if (expected is null) Assert.Inconclusive("Test can only run if at least one Work Entry has comments.");

        // Act
        var result = await _repository.FindAsync(expected!.Id, []);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(expected,
            options => options.Excluding(entry => entry.Comments).Excluding(entry => entry.Facility));
        result!.Comments.Count.Should().Be(expected: 0);
    }
}
