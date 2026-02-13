using AirWeb.AppServices.Compliance.Fces.Search;
using AirWeb.AppServices.Core.AuthenticationServices;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.TestData.Compliance;
using AutoMapper;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;
using System.Security.Claims;

namespace AppServicesTests.Fces.Search;

public class FceSearchServiceTests
{
    private readonly PaginatedRequest _paging = new(pageNumber: 1, pageSize: 100, sorting: "Id");

    [Test]
    public async Task WhenItemsExist_ReturnsPagedList()
    {
        // Arrange
        var searchDto = new FceSearchDto();
        var entries = AppServicesTestsSetup.Mapper!.Map<IReadOnlyCollection<FceSearchResultDto>>(
            FceData.GetData.Where(fce => !fce.IsDeleted).ToList());

        var repoMock = Substitute.For<IFceRepository>();
        repoMock.CountAsync(Arg.Any<Expression<Func<Fce, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(entries.Count);
        repoMock.GetPagedListAsync<FceSearchResultDto>(Arg.Any<Expression<Func<Fce, bool>>>(),
                Arg.Any<PaginatedRequest>(), Arg.Any<IMapper>(), Arg.Any<CancellationToken>())
            .Returns(entries);

        var authMock = Substitute.For<IAuthorizationService>();
        authMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var service = new FceSearchService(repoMock, Substitute.For<IFacilityService>(),
            AppServicesTestsSetup.Mapper, Substitute.For<IUserService>(), authMock);

        // Act
        var result = await service.SearchAsync(searchDto, _paging);

        // Assert
        using var scope = new AssertionScope();
        result.Items.Should().BeEquivalentTo(entries);
        result.CurrentCount.Should().Be(entries.Count);
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyPagedList()
    {
        // Arrange
        var searchDto = new FceSearchDto();

        var repoMock = Substitute.For<IFceRepository>();
        repoMock.CountAsync(Arg.Any<Expression<Func<Fce, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(0);
        repoMock.GetPagedListAsync(Arg.Any<Expression<Func<Fce, bool>>>(),
                Arg.Any<PaginatedRequest>(), Arg.Any<CancellationToken>())
            .Returns([]);

        var authMock = Substitute.For<IAuthorizationService>();
        authMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var service = new FceSearchService(repoMock, Substitute.For<IFacilityService>(),
            AppServicesTestsSetup.Mapper!, Substitute.For<IUserService>(), authMock);

        // Act
        var result = await service.SearchAsync(searchDto, _paging);

        // Assert
        using var scope = new AssertionScope();
        result.Items.Should().BeEmpty();
        result.CurrentCount.Should().Be(expected: 0);
    }
}
