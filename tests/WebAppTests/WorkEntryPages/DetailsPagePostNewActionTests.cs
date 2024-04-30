using MyApp.AppServices.EntryActions.Dto;
using MyApp.AppServices.WorkEntries;
using MyApp.AppServices.WorkEntries.QueryDto;

namespace WebAppTests.WorkEntryPages;

public class DetailsPagePostNewActionTests
{
    [Test]
    public async Task OnPostAsync_NullId_ReturnsRedirectToPageResult()
    {
        // Arrange
        var id = Guid.NewGuid();
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
        var id = Guid.NewGuid();
        var dto = new EntryActionCreateDto(id);
        var workEntryService = Substitute.For<IWorkEntryService>();
        workEntryService.FindAsync(id).Returns((WorkEntryViewDto?)null);
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
        var id = Guid.NewGuid();
        var dto = new EntryActionCreateDto(id);
        var page = PageModelHelpers.BuildDetailsPageModel(Substitute.For<IWorkEntryService>());

        // Act
        var result = await page.OnPostNewActionAsync(Guid.NewGuid(), dto, CancellationToken.None);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }
}
