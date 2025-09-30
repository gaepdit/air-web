using AirWeb.AppServices.Lookups.Offices;
using AirWeb.TestData.SampleData;
using AirWeb.WebApp.Pages.Admin.Maintenance.Offices;
using AirWeb.WebApp.Platform.PageModelHelpers;

namespace WebAppTests.MaintenancePages.Offices;

public class IndexTests
{
    private static readonly List<OfficeViewDto> ListTest =
    [
        new OfficeViewDto { Id = Guid.NewGuid(), Name = SampleText.ValidName, Active = true },
    ];

    [Test]
    public async Task OnGet_ReturnsWithList()
    {
        // Arrange
        var serviceMock = Substitute.For<IOfficeService>();
        serviceMock.GetListAsync(Arg.Any<CancellationToken>()).Returns(ListTest);

        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(user: Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var page = new OfficeIndexModel { TempData = WebAppTestsSetup.PageTempData() };

        // Act
        await page.OnGetAsync(serviceMock, authorizationMock);

        // Assert
        using var scope = new AssertionScope();
        page.Items.Should().BeEquivalentTo(ListTest);
        page.TempData.GetDisplayMessages().Should().BeNull();
        page.HighlightId.Should().BeNull();
    }
}
