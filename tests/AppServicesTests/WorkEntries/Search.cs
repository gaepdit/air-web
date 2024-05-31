using AirWeb.AppServices.Notifications;
using AirWeb.AppServices.UserServices;
using AirWeb.AppServices.WorkEntries;
using AirWeb.AppServices.WorkEntries.Search;
using AirWeb.Domain.Entities.Facilities;
using AirWeb.Domain.Entities.NotificationTypes;
using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.TestData;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Authorization;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Security.Claims;

namespace AppServicesTests.WorkEntries;

public class Search
{
    [Test]
    public async Task WhenItemsExist_ReturnsPagedList()
    {
        // Arrange
        var itemList = new ReadOnlyCollection<BaseWorkEntry>(WorkEntryData.GetData.ToList());
        var count = WorkEntryData.GetData.Count();

        var paging = new PaginatedRequest(1, 100);

        var repoMock = Substitute.For<IWorkEntryRepository>();
        repoMock.GetPagedListAsync(Arg.Any<Expression<Func<BaseWorkEntry, bool>>>(),
                Arg.Any<PaginatedRequest>(), Arg.Any<CancellationToken>())
            .Returns(itemList);
        repoMock.CountAsync(Arg.Any<Expression<Func<BaseWorkEntry, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(count);

        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var appService = new WorkEntryService(AppServicesTestsSetup.Mapper!, repoMock,
            Substitute.For<IWorkEntryManager>(), Substitute.For<INotificationTypeRepository>(),
            Substitute.For<INotificationService>(), Substitute.For<IFacilityRepository>(),
            Substitute.For<IUserService>(), authorizationMock);

        // Act
        var result = await appService.SearchAsync(new WorkEntrySearchDto(), paging, CancellationToken.None);

        // Assert
        using var scope = new AssertionScope();
        result.Items.Should().BeEquivalentTo(itemList);
        result.CurrentCount.Should().Be(count);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsEmptyPagedList()
    {
        // Arrange
        var itemList = new ReadOnlyCollection<BaseWorkEntry>(new List<BaseWorkEntry>());
        const int count = 0;

        var paging = new PaginatedRequest(1, 100);

        var repoMock = Substitute.For<IWorkEntryRepository>();
        repoMock.GetPagedListAsync(Arg.Any<Expression<Func<BaseWorkEntry, bool>>>(),
                Arg.Any<PaginatedRequest>(), Arg.Any<CancellationToken>())
            .Returns(itemList);
        repoMock.CountAsync(
                Arg.Any<Expression<Func<BaseWorkEntry, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(count);

        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var appService = new WorkEntryService(AppServicesTestsSetup.Mapper!, repoMock,
            Substitute.For<IWorkEntryManager>(), Substitute.For<INotificationTypeRepository>(),
            Substitute.For<INotificationService>(), Substitute.For<IFacilityRepository>(),
            Substitute.For<IUserService>(), authorizationMock);

        // Act
        var result = await appService.SearchAsync(new WorkEntrySearchDto(), paging, CancellationToken.None);

        // Assert
        using var scope = new AssertionScope();
        result.Items.Should().BeEmpty();
        result.CurrentCount.Should().Be(count);
    }
}
