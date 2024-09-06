using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.TestData.Compliance;

namespace EfRepositoryTests.WorkEntries;

public class FindOfType
{
    private IWorkEntryRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetWorkEntryRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task GivenExistingItem_ReturnsTrue()
    {
        // Arrange
        var expected = WorkEntryData.GetData.First(entry => entry.WorkEntryType.Equals(WorkEntryType.Notification));

        // Act
        var result = await _repository.FindAsync<Notification>(expected.Id);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(expected,
            options => options.Excluding(entry => entry.Comments).Excluding(entry => entry.Facility));
        result!.WorkEntryType.Should().Be(WorkEntryType.Notification);
        result.Should().BeOfType<Notification>();
    }

    [Test]
    public async Task GivenNonexistentId_ReturnsNull()
    {
        // Act
        var result = await _repository.FindAsync<Notification>(id: 0);

        // Assert
        result.Should().BeNull();
    }
}
