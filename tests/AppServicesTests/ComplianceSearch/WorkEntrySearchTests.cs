using AirWeb.AppServices.Compliance.Search;
using AirWeb.AppServices.UserServices;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.Domain.Search;
using AirWeb.TestData.Entities;
using AirWeb.TestData.SampleData;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;
using System.Security.Claims;

namespace AppServicesTests.ComplianceSearch;

public class WorkEntrySearchTests
{
    private readonly PaginatedRequest _paging = new(pageNumber: 1, pageSize: 100);

    [Test]
    public async Task WhenWorkEntriesItemsExist_ReturnsPagedList()
    {
        // Arrange
        var searchDto = new WorkEntrySearchDto();
        var entries = WorkEntryData.GetData.Where(entry => !entry.IsDeleted).ToList();

        var searchRepoMock = Substitute.For<ISearchRepository>();
        searchRepoMock.CountRecordsAsync(Arg.Any<Expression<Func<WorkEntry, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(entries.Count);
        searchRepoMock.GetFilteredRecordsAsync(Arg.Any<Expression<Func<WorkEntry, bool>>>(),
                Arg.Any<PaginatedRequest>(), Arg.Any<CancellationToken>())
            .Returns(entries);

        var facilityRepoMock = Substitute.For<IFacilityRepository>();
        facilityRepoMock.GetFacilityNameAsync(Arg.Any<FacilityId>(), Arg.Any<CancellationToken>())
            .Returns(SampleText.ValidName);

        var authMock = Substitute.For<IAuthorizationService>();
        authMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var service = new ComplianceSearchService(searchRepoMock, facilityRepoMock, AppServicesTestsSetup.Mapper!,
            Substitute.For<IUserService>(), authMock);

        // Act
        var result = await service.SearchWorkEntriesAsync(searchDto, _paging);

        // Assert
        using var scope = new AssertionScope();
        result.Items.Should().BeEquivalentTo(entries);
        result.CurrentCount.Should().Be(entries.Count);
    }

    [Test]
    public async Task WhenNoWorkEntryItemsExist_ReturnsEmptyPagedList()
    {
        // Arrange
        var searchDto = new WorkEntrySearchDto();

        var searchRepoMock = Substitute.For<ISearchRepository>();
        searchRepoMock.CountRecordsAsync(Arg.Any<Expression<Func<WorkEntry, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(0);
        searchRepoMock.GetFilteredRecordsAsync(Arg.Any<Expression<Func<WorkEntry, bool>>>(),
                Arg.Any<PaginatedRequest>(), Arg.Any<CancellationToken>())
            .Returns([]);

        var facilityRepoMock = Substitute.For<IFacilityRepository>();
        facilityRepoMock.GetFacilityNameAsync(Arg.Any<FacilityId>(), Arg.Any<CancellationToken>())
            .Returns(SampleText.ValidName);

        var authMock = Substitute.For<IAuthorizationService>();
        authMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var service = new ComplianceSearchService(searchRepoMock, facilityRepoMock, AppServicesTestsSetup.Mapper!,
            Substitute.For<IUserService>(), authMock);

        // Act
        var result = await service.SearchWorkEntriesAsync(searchDto, _paging);

        // Assert
        using var scope = new AssertionScope();
        result.Items.Should().BeEmpty();
        result.CurrentCount.Should().Be(expected: 0);
    }

    [Test]
    public async Task WhenFceItemsExist_ReturnsPagedList()
    {
        // Arrange
        var searchDto = new FceSearchDto();
        var entries = FceData.GetData.Where(entry => !entry.IsDeleted).ToList();

        var searchRepoMock = Substitute.For<ISearchRepository>();
        searchRepoMock.CountRecordsAsync(Arg.Any<Expression<Func<Fce, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(entries.Count);
        searchRepoMock.GetFilteredRecordsAsync(Arg.Any<Expression<Func<Fce, bool>>>(),
                Arg.Any<PaginatedRequest>(), Arg.Any<CancellationToken>())
            .Returns(entries);

        var facilityRepoMock = Substitute.For<IFacilityRepository>();
        facilityRepoMock.GetFacilityNameAsync(Arg.Any<FacilityId>(), Arg.Any<CancellationToken>())
            .Returns(SampleText.ValidName);

        var authMock = Substitute.For<IAuthorizationService>();
        authMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var service = new ComplianceSearchService(searchRepoMock, facilityRepoMock, AppServicesTestsSetup.Mapper!,
            Substitute.For<IUserService>(), authMock);

        // Act
        var result = await service.SearchFcesAsync(searchDto, _paging);

        // Assert
        using var scope = new AssertionScope();
        result.Items.Should().BeEquivalentTo(entries);
        result.Items[0].FacilityName.Should().Be(expected: SampleText.ValidName);
        result.CurrentCount.Should().Be(entries.Count);
    }

    [Test]
    public async Task WhenNoFceItemsExist_ReturnsEmptyPagedList()
    {
        // Arrange
        var searchDto = new FceSearchDto();

        var searchRepoMock = Substitute.For<ISearchRepository>();
        searchRepoMock.CountRecordsAsync(Arg.Any<Expression<Func<Fce, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(0);
        searchRepoMock.GetFilteredRecordsAsync(Arg.Any<Expression<Func<Fce, bool>>>(),
                Arg.Any<PaginatedRequest>(), Arg.Any<CancellationToken>())
            .Returns([]);

        var facilityRepoMock = Substitute.For<IFacilityRepository>();
        facilityRepoMock.GetFacilityNameAsync(Arg.Any<FacilityId>(), Arg.Any<CancellationToken>())
            .Returns(SampleText.ValidName);

        var authMock = Substitute.For<IAuthorizationService>();
        authMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var service = new ComplianceSearchService(searchRepoMock, facilityRepoMock, AppServicesTestsSetup.Mapper!,
            Substitute.For<IUserService>(), authMock);

        // Act
        var result = await service.SearchFcesAsync(searchDto, _paging);

        // Assert
        using var scope = new AssertionScope();
        result.Items.Should().BeEmpty();
        result.CurrentCount.Should().Be(expected: 0);
    }
}
