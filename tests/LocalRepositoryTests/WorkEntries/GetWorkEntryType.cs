using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.LocalRepository.Repositories;
using AirWeb.TestData.Entities;

namespace LocalRepositoryTests.WorkEntries;

public class GetWorkEntryType
{
    private LocalWorkEntryRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetWorkEntryRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [TestCase(WorkEntryType.Notification)]
    [TestCase(WorkEntryType.ComplianceEvent)]
    [TestCase(WorkEntryType.PermitRevocation)]
    public async Task GivenExistingItem_ReturnsValue(WorkEntryType type)
    {
        // Arrange
        var entry = WorkEntryData.GetData.First(entry => entry.WorkEntryType.Equals(type));

        // Act
        var result = await _repository.GetWorkEntryTypeAsync(entry.Id);

        // Assert
        result.Should().Be(type);
    }


    [Test]
    public async Task GivenNonexistentId_ReturnsNull()
    {
        // Act
        var func = async () => await _repository.GetWorkEntryTypeAsync(id: 0);

        // Assert
        await func.Should().ThrowAsync<InvalidOperationException>();
    }
}
