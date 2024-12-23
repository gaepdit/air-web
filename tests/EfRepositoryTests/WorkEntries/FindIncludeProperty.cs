using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.EfRepository.Repositories;
using AirWeb.TestData.Compliance;

namespace EfRepositoryTests.WorkEntries;

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
        var result = await _repository.FindAsync<Notification>(expected.Id, includeComments: true);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(expected);
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
        var result = await _repository.FindAsync(expected!.Id, includeProperties: []);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(expected, options => options.Excluding(entry => entry.Comments));
        result!.Comments.Count.Should().Be(expected: 0);
    }

    [Test]
    public async Task GetWithoutComments_ReturnsEntityWithoutComments()
    {
        // Arrange
        var expected = WorkEntryData.GetData.FirstOrDefault(entry =>
            entry is { WorkEntryType: WorkEntryType.Notification, Comments.Count: > 0 });
        if (expected is null) Assert.Inconclusive("Test can only run if at least one Work Entry has comments.");

        // Act
        var result = await _repository.FindAsync<Notification>(expected.Id, includeComments: false);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(expected, options => options.Excluding(entry => entry.Comments));
        result!.Comments.Count.Should().Be(expected: 0);
    }
}
