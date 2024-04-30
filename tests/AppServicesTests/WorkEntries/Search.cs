using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Authorization;
using MyApp.AppServices.Notifications;
using MyApp.AppServices.UserServices;
using MyApp.AppServices.WorkEntries;
using MyApp.AppServices.WorkEntries.QueryDto;
using MyApp.Domain.Entities.EntryTypes;
using MyApp.Domain.Entities.WorkEntries;
using MyApp.TestData;
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
        var itemList = new ReadOnlyCollection<WorkEntry>(WorkEntryData.GetData.ToList());
        var count = WorkEntryData.GetData.Count();

        var paging = new PaginatedRequest(1, 100);

        var repoMock = Substitute.For<IWorkEntryRepository>();
        repoMock.GetPagedListAsync(Arg.Any<Expression<Func<WorkEntry, bool>>>(),
                Arg.Any<PaginatedRequest>(), Arg.Any<CancellationToken>())
            .Returns(itemList);
        repoMock.CountAsync(Arg.Any<Expression<Func<WorkEntry, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(count);

        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var appService = new WorkEntryService(AppServicesTestsSetup.Mapper!, repoMock,
            Substitute.For<IEntryTypeRepository>(), Substitute.For<WorkEntryManager>(),
            Substitute.For<INotificationService>(), Substitute.For<IUserService>(), authorizationMock);

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
        var itemList = new ReadOnlyCollection<WorkEntry>(new List<WorkEntry>());
        const int count = 0;

        var paging = new PaginatedRequest(1, 100);

        var repoMock = Substitute.For<IWorkEntryRepository>();
        repoMock.GetPagedListAsync(Arg.Any<Expression<Func<WorkEntry, bool>>>(),
                Arg.Any<PaginatedRequest>(), Arg.Any<CancellationToken>())
            .Returns(itemList);
        repoMock.CountAsync(
                Arg.Any<Expression<Func<WorkEntry, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(count);

        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var appService = new WorkEntryService(AppServicesTestsSetup.Mapper!, repoMock,
            Substitute.For<IEntryTypeRepository>(), Substitute.For<WorkEntryManager>(),
            Substitute.For<INotificationService>(), Substitute.For<IUserService>(),
            authorizationMock);

        // Act
        var result = await appService.SearchAsync(new WorkEntrySearchDto(), paging, CancellationToken.None);

        // Assert
        using var scope = new AssertionScope();
        result.Items.Should().BeEmpty();
        result.CurrentCount.Should().Be(count);
    }
}
