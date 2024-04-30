using MyApp.AppServices.WorkEntries;
using MyApp.AppServices.WorkEntries.QueryDto;
using MyApp.Domain.Entities.WorkEntries;
using MyApp.TestData;

namespace EfRepositoryTests.WorkEntries;

public class SearchSpec
{
    private IWorkEntryRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetWorkEntryRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task DefaultSpec_ReturnsAllNonDeleted()
    {
        // Arrange
        var spec = new WorkEntrySearchDto();
        var predicate = WorkEntryFilters.SearchPredicate(spec);

        // Act
        var results = await _repository.GetListAsync(predicate);

        // Assert
        var expected = WorkEntryData.GetData
            .Where(entry => entry is { IsDeleted: false });
        results.Should().BeEquivalentTo(expected, options =>
            options.Excluding(entry => entry.EntryActions));
    }

    [Test]
    public async Task ClosedStatusSpec_ReturnsFilteredList()
    {
        // Arrange
        var spec = new WorkEntrySearchDto { Status = WorkEntryStatus.Closed };
        var predicate = WorkEntryFilters.SearchPredicate(spec);

        // Act
        var results = await _repository.GetListAsync(predicate);

        // Assert
        var expected = WorkEntryData.GetData
            .Where(entry => entry is { IsDeleted: false, Closed: true });
        results.Should().BeEquivalentTo(expected, options => options.Excluding(entry => entry.EntryActions));
    }

    [Test]
    public async Task DeletedSpec_ReturnsFilteredList()
    {
        // Arrange
        var spec = new WorkEntrySearchDto { DeletedStatus = SearchDeleteStatus.Deleted };
        var predicate = WorkEntryFilters.SearchPredicate(spec);

        // Act
        var results = await _repository.GetListAsync(predicate);

        // Assert
        var expected = WorkEntryData.GetData
            .Where(entry => entry is { IsDeleted: true });
        results.Should().BeEquivalentTo(expected, options =>
            options.Excluding(entry => entry.EntryActions));
    }

    [Test]
    public async Task NeutralDeletedSpec_ReturnsAll()
    {
        // Arrange
        var spec = new WorkEntrySearchDto { DeletedStatus = SearchDeleteStatus.All };
        var predicate = WorkEntryFilters.SearchPredicate(spec);

        // Act
        var results = await _repository.GetListAsync(predicate);

        // Assert
        var expected = WorkEntryData.GetData;
        results.Should().BeEquivalentTo(expected, options =>
            options.Excluding(entry => entry.EntryActions));
    }

    [Test]
    public async Task ReceivedDateSpec_ReturnsFilteredList()
    {
        // Arrange
        var repository = RepositoryHelper.CreateSqlServerRepositoryHelper(this).GetWorkEntryRepository();

        var referenceItem = WorkEntryData.GetData.First();

        var spec = new WorkEntrySearchDto
        {
            ReceivedFrom = DateOnly.FromDateTime(referenceItem.ReceivedDate.Date),
            ReceivedTo = DateOnly.FromDateTime(referenceItem.ReceivedDate.Date),
        };

        var predicate = WorkEntryFilters.SearchPredicate(spec);

        // Act
        var results = await repository.GetListAsync(predicate);

        // Assert
        var expected = WorkEntryData.GetData
            .Where(entry => entry is { IsDeleted: false } &&
                            entry.ReceivedDate.Date == referenceItem.ReceivedDate.Date);
        results.Should().BeEquivalentTo(expected, options =>
            options.Excluding(entry => entry.EntryActions));
    }

    [Test]
    public async Task ReceivedBySpec_ReturnsFilteredList()
    {
        // Arrange
        var referenceItem = WorkEntryData.GetData.First(entry => entry.ReceivedBy != null);
        var spec = new WorkEntrySearchDto { ReceivedBy = referenceItem.ReceivedBy!.Id };
        var predicate = WorkEntryFilters.SearchPredicate(spec);

        // Act
        var results = await _repository.GetListAsync(predicate);

        // Assert
        var expected = WorkEntryData.GetData
            .Where(entry => entry is { IsDeleted: false } && entry.ReceivedBy == referenceItem.ReceivedBy);
        results.Should().BeEquivalentTo(expected, options =>
            options.Excluding(entry => entry.EntryActions));
    }

    [Test]
    public async Task EntryTypeSpec_ReturnsFilteredList()
    {
        // Arrange
        var referenceItem = WorkEntryData.GetData.First(entry => entry.EntryType != null);
        var spec = new WorkEntrySearchDto { EntryType = referenceItem.EntryType!.Id };
        var predicate = WorkEntryFilters.SearchPredicate(spec);

        // Act
        var results = await _repository.GetListAsync(predicate);

        // Assert
        var expected = WorkEntryData.GetData
            .Where(entry => entry is { IsDeleted: false, EntryType: not null } &&
                            entry.EntryType.Id == referenceItem.EntryType.Id);
        results.Should().BeEquivalentTo(expected, options =>
            options.Excluding(entry => entry.EntryActions));
    }
}
