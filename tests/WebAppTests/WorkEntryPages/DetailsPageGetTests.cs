using MyApp.AppServices.WorkEntries;
using MyApp.AppServices.WorkEntries.Permissions;
using MyApp.AppServices.WorkEntries.QueryDto;

namespace WebAppTests.WorkEntryPages;

public class DetailsPageGetTests
{
    private static readonly WorkEntryViewDto ItemTest = new() { Id = Guid.NewGuid() };

    [Test]
    public async Task OnGetReturnsWithCorrectPermissions()
    {
        // Arrange
        var workEntryService = Substitute.For<IWorkEntryService>();
        workEntryService.FindAsync(Arg.Any<Guid>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(ItemTest);

        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Any<object?>(),
                Arg.Is<IAuthorizationRequirement[]>(x => x.Contains(WorkEntryOperation.ManageDeletions)))
            .Returns(AuthorizationResult.Success());
        authorizationMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Any<object?>(),
                Arg.Is<IAuthorizationRequirement[]>(x => !x.Contains(WorkEntryOperation.ManageDeletions)))
            .Returns(AuthorizationResult.Failed());

        var page = PageModelHelpers.BuildDetailsPageModel(workEntryService, authorizationService: authorizationMock);
        page.TempData = WebAppTestsSetup.PageTempData();
        page.PageContext = WebAppTestsSetup.PageContextWithUser();

        // Act
        await page.OnGetAsync(ItemTest.Id);

        // Assert
        using var scope = new AssertionScope();
        page.ItemView.Should().BeEquivalentTo(ItemTest);
        page.UserCan.Should().NotBeEmpty();
        page.UserCan[WorkEntryOperation.ManageDeletions].Should().BeTrue();
        page.UserCan[WorkEntryOperation.ViewDeletedActions].Should().BeFalse();
    }

    [Test]
    public async Task OnGetAsync_NullId_ReturnsRedirectToPageResult()
    {
        var page = PageModelHelpers.BuildDetailsPageModel();
        var result = await page.OnGetAsync(null);
        result.Should().BeOfType<RedirectToPageResult>();
    }

    [Test]
    public async Task OnGetAsync_CaseNotFound_ReturnsNotFoundResult()
    {
        // Arrange
        var id = Guid.NewGuid();

        var workEntryService = Substitute.For<IWorkEntryService>();
        workEntryService.FindAsync(id).Returns((WorkEntryViewDto?)null);

        var page = PageModelHelpers.BuildDetailsPageModel(workEntryService);

        // Act
        var result = await page.OnGetAsync(id);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}
