using AirWeb.AppServices.DomainEntities.WorkEntries;
using AirWeb.AppServices.DomainEntities.WorkEntries.BaseWorkEntryDto;

namespace WebAppTests.WorkEntryPages;

public class DetailsPagePostNewActionTests
{
    [Test]
    public async Task OnPostAsync_NullId_ReturnsRedirectToPageResult()
    {
        // Arrange
        const int id = 905;
        var dto = new EntryActionCreateDto(id);
        var page = PageModelHelpers.BuildDetailsPageModel();

        // Act
        var result = await page.OnPostNewActionAsync(null, dto, CancellationToken.None);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPostAsync_EntryNotFound_ReturnsBadRequestResult()
    {
        // Arrange
        const int id = 906;
        var dto = new EntryActionCreateDto(id);
        var workEntryService = Substitute.For<IWorkEntryService>();
        workEntryService.FindAsync(id).Returns((BaseWorkEntryViewDto?)null);
        var page = PageModelHelpers.BuildDetailsPageModel(workEntryService);

        // Act
        var result = await page.OnPostNewActionAsync(id, dto, CancellationToken.None);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPostAsync_MismatchedId_ReturnsBadRequest()
    {
        // Arrange
        const int id = 907;
        var dto = new EntryActionCreateDto(id);
        var page = PageModelHelpers.BuildDetailsPageModel(Substitute.For<IWorkEntryService>());

        // Act
        var result = await page.OnPostNewActionAsync(908, dto, CancellationToken.None);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }
}
