using MyApp.AppServices.WorkEntries;
using MyApp.AppServices.WorkEntries.QueryDto;
using MyApp.Domain.Entities.EntryActions;
using MyApp.Domain.Entities.WorkEntries;
using MyApp.LocalRepository.Repositories;

namespace LocalRepositoryTests.WorkEntries;

public class SearchSpec
{
    private LocalWorkEntryRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = new LocalWorkEntryRepository(Substitute.For<IEntryActionRepository>());

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
        var expected = _repository.Items.Where(entry => entry is { IsDeleted: false });
        results.Should().BeEquivalentTo(expected);
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
        var expected = _repository.Items.Where(entry => entry is { IsDeleted: false, Closed: true });
        results.Should().BeEquivalentTo(expected);
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
        var expected = _repository.Items.Where(entry => entry is { IsDeleted: true });
        results.Should().BeEquivalentTo(expected);
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
        var expected = _repository.Items;
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task ReceivedDateSpec_ReturnsFilteredList()
    {
        // Arrange
        var referenceItem = _repository.Items.First();

        var spec = new WorkEntrySearchDto
        {
            ReceivedFrom = DateOnly.FromDateTime(referenceItem.ReceivedDate.Date),
            ReceivedTo = DateOnly.FromDateTime(referenceItem.ReceivedDate.Date),
        };

        var predicate = WorkEntryFilters.SearchPredicate(spec);

        // Act
        var results = await _repository.GetListAsync(predicate);

        // Assert
        var expected = _repository.Items
            .Where(entry => entry is { IsDeleted: false } && entry.ReceivedDate.Date == referenceItem.ReceivedDate.Date);
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task ReceivedBySpec_ReturnsFilteredList()
    {
        // Arrange
        var referenceItem = _repository.Items.First(entry => entry.ReceivedBy != null);
        var spec = new WorkEntrySearchDto { ReceivedBy = referenceItem.ReceivedBy!.Id };
        var predicate = WorkEntryFilters.SearchPredicate(spec);

        // Act
        var results = await _repository.GetListAsync(predicate);

        // Assert
        var expected = _repository.Items
            .Where(entry => entry is { IsDeleted: false } && entry.ReceivedBy == referenceItem.ReceivedBy);
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task EntryTypeSpec_ReturnsFilteredList()
    {
        // Arrange
        var referenceItem = _repository.Items.First(entry => entry.EntryType != null);
        var spec = new WorkEntrySearchDto { EntryType = referenceItem.EntryType!.Id };
        var predicate = WorkEntryFilters.SearchPredicate(spec);

        // Act
        var results = await _repository.GetListAsync(predicate);

        // Assert
        var expected = _repository.Items
            .Where(entry => entry is { IsDeleted: false, EntryType: not null } &&
                            entry.EntryType.Id == referenceItem.EntryType.Id);
        results.Should().BeEquivalentTo(expected);
    }
}
