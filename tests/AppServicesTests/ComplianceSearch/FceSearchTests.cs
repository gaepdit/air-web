using AirWeb.AppServices.Compliance.Search;
using AirWeb.AppServices.UserServices;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.Domain.Search;
using AirWeb.TestData.Compliance;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;
using System.Security.Claims;

namespace AppServicesTests.ComplianceSearch;

public class FceSearchTests
{
    private readonly PaginatedRequest _paging = new(pageNumber: 1, pageSize: 100);

    [Test]
    public async Task WhenFceItemsExist_ReturnsPagedList()
    {
        // Arrange
        var searchDto = new FceSearchDto();
        var entries = FceData.GetData.Where(fce => !fce.IsDeleted).ToList();

        var searchRepoMock = Substitute.For<IComplianceSearchRepository>();
        searchRepoMock.CountRecordsAsync(Arg.Any<Expression<Func<Fce, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(entries.Count);
        searchRepoMock.GetFilteredRecordsAsync(Arg.Any<Expression<Func<Fce, bool>>>(),
                Arg.Any<PaginatedRequest>(), Arg.Any<CancellationToken>())
            .Returns(entries);

        var authMock = Substitute.For<IAuthorizationService>();
        authMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var service = new ComplianceSearchService(searchRepoMock, Substitute.For<IFacilityRepository>(),
            AppServicesTestsSetup.Mapper!,
            Substitute.For<IUserService>(), authMock);

        // Act
        var result = await service.SearchFcesAsync(searchDto, _paging);

        // Assert
        using var scope = new AssertionScope();
        result.Items.Should().BeEquivalentTo(entries);
        result.CurrentCount.Should().Be(entries.Count);
    }

    [Test]
    public async Task WhenNoFceItemsExist_ReturnsEmptyPagedList()
    {
        // Arrange
        var searchDto = new FceSearchDto();

        var searchRepoMock = Substitute.For<IComplianceSearchRepository>();
        searchRepoMock.CountRecordsAsync(Arg.Any<Expression<Func<Fce, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(0);
        searchRepoMock.GetFilteredRecordsAsync(Arg.Any<Expression<Func<Fce, bool>>>(),
                Arg.Any<PaginatedRequest>(), Arg.Any<CancellationToken>())
            .Returns([]);

        var authMock = Substitute.For<IAuthorizationService>();
        authMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var service = new ComplianceSearchService(searchRepoMock, Substitute.For<IFacilityRepository>(),
            AppServicesTestsSetup.Mapper!, Substitute.For<IUserService>(), authMock);

        // Act
        var result = await service.SearchFcesAsync(searchDto, _paging);

        // Assert
        using var scope = new AssertionScope();
        result.Items.Should().BeEmpty();
        result.CurrentCount.Should().Be(expected: 0);
    }
}
