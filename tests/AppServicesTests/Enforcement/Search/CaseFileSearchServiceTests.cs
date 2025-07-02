using AirWeb.AppServices.Enforcement.Search;
using AirWeb.AppServices.Users;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.TestData.Enforcement;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;
using System.Security.Claims;

namespace AppServicesTests.Enforcement.Search;

public class CaseFileSearchServiceTests
{
    private readonly PaginatedRequest _paging = new(pageNumber: 1, pageSize: 100);

    [Test]
    public async Task WhenItemsExist_ReturnsPagedList()
    {
        // Arrange
        var searchDto = new CaseFileSearchDto();
        // Hydrate the enforcement action data
        _ = EnforcementActionData.GetData;
        var entries = CaseFileData.GetData.ToList();

        var repoMock = Substitute.For<ICaseFileRepository>();
        repoMock.CountAsync(Arg.Any<Expression<Func<CaseFile, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(entries.Count);
        repoMock.GetPagedListAsync(Arg.Any<Expression<Func<CaseFile, bool>>>(), Arg.Any<PaginatedRequest>(),
                Arg.Any<CancellationToken>())
            .Returns(entries);

        var authMock = Substitute.For<IAuthorizationService>();
        authMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var service = new CaseFileSearchService(repoMock, Substitute.For<IFacilityService>(),
            AppServicesTestsSetup.Mapper!, Substitute.For<IUserService>(), authMock);

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
        var searchDto = new CaseFileSearchDto();

        var repoMock = Substitute.For<ICaseFileRepository>();
        repoMock.CountAsync(Arg.Any<Expression<Func<CaseFile, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(0);
        repoMock.GetPagedListAsync(Arg.Any<Expression<Func<CaseFile, bool>>>(),
                Arg.Any<PaginatedRequest>(), Arg.Any<CancellationToken>())
            .Returns([]);

        var authMock = Substitute.For<IAuthorizationService>();
        authMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var service = new CaseFileSearchService(repoMock, Substitute.For<IFacilityService>(),
            AppServicesTestsSetup.Mapper!, Substitute.For<IUserService>(), authMock);

        // Act
        var result = await service.SearchAsync(searchDto, _paging);

        // Assert
        using var scope = new AssertionScope();
        result.Items.Should().BeEmpty();
        result.CurrentCount.Should().Be(expected: 0);
    }
}
