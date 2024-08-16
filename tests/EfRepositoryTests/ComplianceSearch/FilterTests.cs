using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.Search;
using AirWeb.TestData.Entities;
using GaEpd.AppLibrary.Pagination;
using System.Linq.Expressions;

namespace EfRepositoryTests.ComplianceSearch;

public class FilterTests
{
    private IComplianceSearchRepository _repository = default!;
    private readonly Expression<Func<WorkEntry, bool>> _workEntryTrueExpression = f => true;
    private readonly Expression<Func<WorkEntry, bool>> _workEntryNotDeletedExpression = f => !f.IsDeleted;
    private readonly Expression<Func<Fce, bool>> _fceTrueExpression = f => true;
    private readonly Expression<Func<Fce, bool>> _fceNotDeletedExpression = f => !f.IsDeleted;
    private readonly PaginatedRequest _unlimitedPaging = new(pageNumber: 1, pageSize: 100);

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetComplianceSearchRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task SearchWorkEntries_WithNeutralSpec_ReturnsAll()
    {
        // Arrange
        var expected = WorkEntryData.GetData;

        // Act
        var result = await _repository.GetFilteredRecordsAsync(_workEntryTrueExpression, _unlimitedPaging);

        // Assert
        result.Should().BeEquivalentTo(expected,
            options => options.Excluding(entry => entry.Facility).Excluding(entry => entry.Comments)
        );
    }

    [Test]
    public async Task SearchWorkEntries_ExcludingDeleted_ReturnsAllNonDeleted()
    {
        // Arrange
        var expected = WorkEntryData.GetData.Where(entry => !entry.IsDeleted);

        // Act
        var result = await _repository.GetFilteredRecordsAsync(_workEntryNotDeletedExpression, _unlimitedPaging);

        // Assert
        result.Should().BeEquivalentTo(expected,
            options => options.Excluding(entry => entry.Facility).Excluding(entry => entry.Comments)
        );
    }

    [Test]
    public async Task SearchFCEs_WithNeutralSpec_ReturnsAll()
    {
        // Arrange
        var expected = FceData.GetData;

        // Act
        var result = await _repository.GetFilteredRecordsAsync(_fceTrueExpression, _unlimitedPaging);

        // Assert
        result.Should().BeEquivalentTo(expected,
            options => options.Excluding(fce => fce.Facility).Excluding(fce => fce.Comments)
        );
    }

    [Test]
    public async Task SearchFCEs_ExcludingDeleted_ReturnsAllNonDeleted()
    {
        // Arrange
        var expected = FceData.GetData.Where(fce => !fce.IsDeleted);

        // Act
        var result = await _repository.GetFilteredRecordsAsync(_fceNotDeletedExpression, _unlimitedPaging);

        // Assert
        result.Should().BeEquivalentTo(expected,
            options => options.Excluding(fce => fce.Facility).Excluding(fce => fce.Comments)
        );
    }
}
