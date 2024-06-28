namespace EfRepositoryTests.WorkEntries;

public class SearchSpec
{
    // private IWorkEntryRepository _repository = default!;
    //
    // [SetUp]
    // public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetWorkEntryRepository();
    //
    // [TearDown]
    // public void TearDown() => _repository.Dispose();
    //
    // [Test]
    // public async Task DefaultSpec_ReturnsAllNonDeleted()
    // {
    //     // Arrange
    //     var spec = new WorkEntrySearchDto();
    //     var predicate = WorkEntryFilters.SearchPredicate(spec);
    //
    //     // Act
    //     var results = await _repository.GetListAsync(predicate);
    //
    //     // Assert
    //     var expected = WorkEntryData.GetData
    //         .Where(entry => entry is { IsDeleted: false });
    //     results.Should().BeEquivalentTo(expected, options =>
    //         options.Excluding(entry => entry.Comments));
    // }
    //
    // [Test]
    // public async Task ClosedStatusSpec_ReturnsFilteredList()
    // {
    //     // Arrange
    //     var spec = new WorkEntrySearchDto();
    //     var predicate = WorkEntryFilters.SearchPredicate(spec);
    //
    //     // Act
    //     var results = await _repository.GetListAsync(predicate);
    //
    //     // Assert
    //     var expected = WorkEntryData.GetData
    //         .Where(entry => entry is { IsDeleted: false, IsClosed: true });
    //     results.Should().BeEquivalentTo(expected, options => options.Excluding(entry => entry.Comments));
    // }
    //
    // [Test]
    // public async Task DeletedSpec_ReturnsFilteredList()
    // {
    //     // Arrange
    //     var spec = new WorkEntrySearchDto { DeletedStatus = SearchDeleteStatus.Deleted };
    //     var predicate = WorkEntryFilters.SearchPredicate(spec);
    //
    //     // Act
    //     var results = await _repository.GetListAsync(predicate);
    //
    //     // Assert
    //     var expected = WorkEntryData.GetData
    //         .Where(entry => entry is { IsDeleted: true });
    //     results.Should().BeEquivalentTo(expected, options =>
    //         options.Excluding(entry => entry.Comments));
    // }
    //
    // [Test]
    // public async Task NeutralDeletedSpec_ReturnsAll()
    // {
    //     // Arrange
    //     var spec = new WorkEntrySearchDto { DeletedStatus = SearchDeleteStatus.All };
    //     var predicate = WorkEntryFilters.SearchPredicate(spec);
    //
    //     // Act
    //     var results = await _repository.GetListAsync(predicate);
    //
    //     // Assert
    //     var expected = WorkEntryData.GetData;
    //     results.Should().BeEquivalentTo(expected, options =>
    //         options.Excluding(entry => entry.Comments));
    // }
    //
    // [Test]
    // public async Task ClosedDateSpec_ReturnsFilteredList()
    // {
    //     // Arrange
    //     var repository = RepositoryHelper.CreateSqlServerRepositoryHelper(this).GetWorkEntryRepository();
    //
    //     var referenceItem = WorkEntryData.GetData.First(entry => entry.ClosedDate != null);
    //
    //     var spec = new WorkEntrySearchDto
    //     {
    //         ReceivedFrom = DateOnly.FromDateTime(referenceItem.ClosedDate!.Value.Date),
    //         ReceivedTo = DateOnly.FromDateTime(referenceItem.ClosedDate.Value.Date),
    //     };
    //
    //     var predicate = WorkEntryFilters.SearchPredicate(spec);
    //
    //     // Act
    //     var results = await repository.GetListAsync(predicate);
    //
    //     // Assert
    //     var expected = WorkEntryData.GetData
    //         .Where(entry => entry is { IsDeleted: false } &&
    //                         entry.ClosedDate!.Value.Date == referenceItem.ClosedDate.Value.Date);
    //     results.Should().BeEquivalentTo(expected, options =>
    //         options.Excluding(entry => entry.Comments));
    // }
    //
    // [Test]
    // public async Task ReceivedBySpec_ReturnsFilteredList()
    // {
    //     // Arrange
    //     var referenceItem = WorkEntryData.GetData.First(entry => entry.ResponsibleStaff != null);
    //     var spec = new WorkEntrySearchDto { ReceivedBy = referenceItem.ResponsibleStaff!.Id };
    //     var predicate = WorkEntryFilters.SearchPredicate(spec);
    //
    //     // Act
    //     var results = await _repository.GetListAsync(predicate);
    //
    //     // Assert
    //     var expected = WorkEntryData.GetData
    //         .Where(entry => entry is { IsDeleted: false } && entry.ResponsibleStaff == referenceItem.ResponsibleStaff);
    //     results.Should().BeEquivalentTo(expected, options =>
    //         options.Excluding(entry => entry.Comments));
    // }
}
