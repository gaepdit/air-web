using AirWeb.AppServices.Compliance.WorkEntries.Search;
using AirWeb.AppServices.Users;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.TestData.Compliance;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;
using System.Security.Claims;

namespace AppServicesTests.WorkEntries.Search;

public class WorkEntrySearchTests
{
    private readonly PaginatedRequest _paging = new(pageNumber: 1, pageSize: 100);

    [Test]
    public async Task WhenWorkEntriesItemsExist_ReturnsPagedList()
    {
        // Arrange
        var searchDto = new WorkEntrySearchDto();
        var entries = WorkEntryData.GetData.Where(entry => !entry.IsDeleted).ToList();

        var searchRepoMock = Substitute.For<IWorkEntryRepository>();
        searchRepoMock.CountAsync(Arg.Any<Expression<Func<WorkEntry, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(entries.Count);
        searchRepoMock.GetPagedListAsync(Arg.Any<Expression<Func<WorkEntry, bool>>>(),
                Arg.Any<PaginatedRequest>(), Arg.Any<CancellationToken>())
            .Returns(entries);

        var authMock = Substitute.For<IAuthorizationService>();
        authMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var service = new WorkEntrySearchService(searchRepoMock, Substitute.For<IFacilityService>(),
            AppServicesTestsSetup.Mapper!,
            Substitute.For<IUserService>(), authMock);

        // Act
        var result = await service.SearchAsync(searchDto, _paging);

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

        var searchRepoMock = Substitute.For<IWorkEntryRepository>();
        searchRepoMock.CountAsync(Arg.Any<Expression<Func<WorkEntry, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(0);
        searchRepoMock.GetPagedListAsync(Arg.Any<Expression<Func<WorkEntry, bool>>>(),
                Arg.Any<PaginatedRequest>(), Arg.Any<CancellationToken>())
            .Returns([]);

        var authMock = Substitute.For<IAuthorizationService>();
        authMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var service = new WorkEntrySearchService(searchRepoMock, Substitute.For<IFacilityService>(),
            AppServicesTestsSetup.Mapper!,
            Substitute.For<IUserService>(), authMock);

        // Act
        var result = await service.SearchAsync(searchDto, _paging);

        // Assert
        using var scope = new AssertionScope();
        result.Items.Should().BeEmpty();
        result.CurrentCount.Should().Be(expected: 0);
    }
}
