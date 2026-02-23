using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Search;
using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AirWeb.TestData.Compliance;
using AutoMapper;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;
using System.Security.Claims;

namespace AppServicesTests.Compliance.ComplianceMonitoring.Search;

public class ComplianceWorkSearchServiceTests
{
    private readonly PaginatedRequest _paging = new(pageNumber: 1, pageSize: 100, sorting: "Id");

    [Test]
    public async Task WhenItemsExist_ReturnsPagedList()
    {
        // Arrange
        var searchDto = new ComplianceWorkSearchDto();
        var entries = Setup.Mapper!.Map<IReadOnlyCollection<ComplianceWorkSearchResultDto>>(
            ComplianceWorkData.GetData.Where(work => !work.IsDeleted).ToList());

        var repoMock = Substitute.For<IComplianceWorkRepository>();
        repoMock.CountAsync(Arg.Any<Expression<Func<ComplianceWork, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(entries.Count);
        repoMock.GetPagedListAsync<ComplianceWorkSearchResultDto>(Arg.Any<Expression<Func<ComplianceWork, bool>>>(),
                Arg.Any<PaginatedRequest>(), Arg.Any<IMapper>(), Arg.Any<CancellationToken>())
            .Returns(entries);

        var authMock = Substitute.For<IAuthorizationService>();
        authMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var service = new ComplianceWorkSearchService(repoMock, Substitute.For<IFacilityService>(),
            Setup.Mapper, Substitute.For<IUserService>(), authMock);

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
        var searchDto = new ComplianceWorkSearchDto();

        var repoMock = Substitute.For<IComplianceWorkRepository>();
        repoMock.CountAsync(Arg.Any<Expression<Func<ComplianceWork, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(0);
        repoMock.GetPagedListAsync(Arg.Any<Expression<Func<ComplianceWork, bool>>>(),
                Arg.Any<PaginatedRequest>(), Arg.Any<CancellationToken>())
            .Returns([]);

        var authMock = Substitute.For<IAuthorizationService>();
        authMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var service = new ComplianceWorkSearchService(repoMock, Substitute.For<IFacilityService>(),
            Setup.Mapper!,
            Substitute.For<IUserService>(), authMock);

        // Act
        var result = await service.SearchAsync(searchDto, _paging);

        // Assert
        using var scope = new AssertionScope();
        result.Items.Should().BeEmpty();
        result.CurrentCount.Should().Be(expected: 0);
    }
}
