using AirWeb.EfRepository.Repositories;
using AirWeb.TestData.Compliance;

namespace EfRepositoryTests.WorkEntries;

public class FindWorkEntry
{
    private WorkEntryRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetWorkEntryRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task GivenExistingItem_ReturnsTrue()
    {
        // Arrange
        var expected = WorkEntryData.GetData.First();

        // Act
        var result = await _repository.FindAsync(expected.Id);

        // Assert
        result.Should().BeEquivalentTo(expected, options => options.Excluding(entry => entry.Comments));
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
