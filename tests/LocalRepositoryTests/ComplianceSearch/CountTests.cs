using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.LocalRepository.Repositories;
using System.Linq.Expressions;

namespace LocalRepositoryTests.ComplianceSearch;

public class CountTests
{
    private LocalFceRepository _fceRepository = null!;
    private LocalWorkEntryRepository _entryRepository = null!;
    private LocalComplianceSearchRepository _repository = null!;

    private readonly Expression<Func<WorkEntry, bool>> _workEntryTrueExpression = f => true;
    private readonly Expression<Func<WorkEntry, bool>> _workEntryNotDeletedExpression = f => !f.IsDeleted;
    private readonly Expression<Func<Fce, bool>> _fceTrueExpression = f => true;
    private readonly Expression<Func<Fce, bool>> _fceNotDeletedExpression = f => !f.IsDeleted;

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
    public async Task CountWorkEntries_WithNeutralSpec_ReturnsAll()
    {
        // Arrange
        var expected = _entryRepository.Items.Count;

        // Act
        var result = await _repository.CountRecordsAsync(_workEntryTrueExpression);

        // Assert
        result.Should().Be(expected);
    }

    [Test]
    public async Task CountWorkEntries_ExcludingDeleted_ReturnsAllNonDeleted()
    {
        // Arrange
        var expected = _entryRepository.Items.Count(entry => !entry.IsDeleted);

        // Act
        var result = await _repository.CountRecordsAsync(_workEntryNotDeletedExpression);

        // Assert
        result.Should().Be(expected);
    }

    [Test]
    public async Task CountFCEs_WithNeutralSpec_ReturnsAll()
    {
        // Arrange
        var expected = _fceRepository.Items.Count;

        // Act
        var result = await _repository.CountRecordsAsync(_fceTrueExpression);

        // Assert
        result.Should().Be(expected);
    }

    [Test]
    public async Task CountFCEs_ExcludingDeleted_ReturnsAllNonDeleted()
    {
        // Arrange
        var expected = _fceRepository.Items.Count(fce => !fce.IsDeleted);

        // Act
        var result = await _repository.CountRecordsAsync(_fceNotDeletedExpression);

        // Assert
        result.Should().Be(expected);
    }
}
