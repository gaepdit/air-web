using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.LocalRepository.Repositories;
using GaEpd.AppLibrary.Pagination;
using System.Linq.Expressions;

namespace LocalRepositoryTests.ComplianceSearch;

public class FilterTests
{
    private LocalFceRepository _fceRepository = null!;
    private LocalWorkEntryRepository _entryRepository = null!;
    private LocalComplianceSearchRepository _repository = null!;

    private readonly Expression<Func<WorkEntry, bool>> _workEntryTrueExpression = f => true;
    private readonly Expression<Func<WorkEntry, bool>> _workEntryNotDeletedExpression = f => !f.IsDeleted;
    private readonly Expression<Func<Fce, bool>> _fceTrueExpression = f => true;
    private readonly Expression<Func<Fce, bool>> _fceNotDeletedExpression = f => !f.IsDeleted;
    private readonly PaginatedRequest _unlimitedPaging = new(pageNumber: 1, pageSize: 100);

    [SetUp]
    public void SetUp()
    {
        _fceRepository = new LocalFceRepository();
        _entryRepository = new LocalWorkEntryRepository();
        _repository = new LocalComplianceSearchRepository(_fceRepository, _entryRepository);
    }

    [TearDown]
    public void TearDown()
    {
        _repository.Dispose();
        _fceRepository.Dispose();
        _entryRepository.Dispose();
    }


    [Test]
    public async Task SearchWorkEntries_WithNeutralSpec_ReturnsAll()
    {
        // Arrange
        var expected = _entryRepository.Items;

        // Act
        var result = await _repository.GetFilteredRecordsAsync(_workEntryTrueExpression, _unlimitedPaging);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task SearchWorkEntries_ExcludingDeleted_ReturnsAllNonDeleted()
    {
        // Arrange
        var expected = _entryRepository.Items.Where(entry => !entry.IsDeleted);

        // Act
        var result = await _repository.GetFilteredRecordsAsync(_workEntryNotDeletedExpression, _unlimitedPaging);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task SearchFCEs_WithNeutralSpec_ReturnsAll()
    {
        // Arrange
        var expected = _fceRepository.Items;

        // Act
        var result = await _repository.GetFilteredRecordsAsync(_fceTrueExpression, _unlimitedPaging);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task SearchFCEs_ExcludingDeleted_ReturnsAllNonDeleted()
    {
        // Arrange
        var expected = _fceRepository.Items.Where(fce => !fce.IsDeleted);

        // Act
        var result = await _repository.GetFilteredRecordsAsync(_fceNotDeletedExpression, _unlimitedPaging);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}
