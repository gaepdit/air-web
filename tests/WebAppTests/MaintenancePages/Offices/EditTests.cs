using MyApp.AppServices.Offices;
using MyApp.TestData.Constants;
using MyApp.WebApp.Models;
using MyApp.WebApp.Pages.Admin.Maintenance.Offices;
using MyApp.WebApp.Platform.PageModelHelpers;

namespace WebAppTests.MaintenancePages.Offices;

public class EditTests
{
    private static readonly OfficeUpdateDto ItemTest = new(TextData.ValidName, true);

    [Test]
    public async Task OnGet_ReturnsWithItem()
    {
        // Arrange
        var officeServiceMock = Substitute.For<IOfficeService>();
        officeServiceMock.FindForUpdateAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(ItemTest);

        var page = new EditModel(officeServiceMock, Substitute.For<IValidator<OfficeUpdateDto>>())
            { TempData = WebAppTestsSetup.PageTempData() };

        // Act
        await page.OnGetAsync(Guid.Empty);

        // Assert
        using var scope = new AssertionScope();
        page.Item.Should().BeEquivalentTo(ItemTest);
        page.OriginalName.Should().Be(ItemTest.Name);
        page.HighlightId.Should().Be(Guid.Empty);
    }

    [Test]
    public async Task OnGet_GivenNullId_ReturnsNotFound()
    {
        // Arrange
        var page = new EditModel(Substitute.For<IOfficeService>(), Substitute.For<IValidator<OfficeUpdateDto>>())
            { TempData = WebAppTestsSetup.PageTempData() };

        // Act
        var result = await page.OnGetAsync(null);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<RedirectToPageResult>();
        ((RedirectToPageResult)result).PageName.Should().Be("Index");
    }

    [Test]
    public async Task OnGet_GivenInvalidId_ReturnsNotFound()
    {
        // Arrange
        var officeServiceMock = Substitute.For<IOfficeService>();
        officeServiceMock.FindForUpdateAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((OfficeUpdateDto?)null);

        var page = new EditModel(officeServiceMock, Substitute.For<IValidator<OfficeUpdateDto>>())
            { TempData = WebAppTestsSetup.PageTempData() };

        // Act
        var result = await page.OnGetAsync(Guid.Empty);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        // Arrange
        var validatorMock = Substitute.For<IValidator<OfficeUpdateDto>>();
        validatorMock.ValidateAsync(Arg.Any<IValidationContext>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        var page = new EditModel(Substitute.For<IOfficeService>(), validatorMock)
            { Id = Guid.NewGuid(), Item = ItemTest, TempData = WebAppTestsSetup.PageTempData() };

        var expectedMessage =
            new DisplayMessage(DisplayMessage.AlertContext.Success, $"“{ItemTest.Name}” successfully updated.", []);

        // Act
        var result = await page.OnPostAsync();

        // Assert
        using var scope = new AssertionScope();
        page.HighlightId.Should().Be(page.Id);
        page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expectedMessage);
        result.Should().BeOfType<RedirectToPageResult>();
        ((RedirectToPageResult)result).PageName.Should().Be("Index");
    }

    [Test]
    public async Task OnPost_GivenInvalidItem_ReturnsPageWithModelErrors()
    {
        // Arrange
        var validationFailures = new List<ValidationFailure> { new("property", "message") };
        var validatorMock = Substitute.For<IValidator<OfficeUpdateDto>>();
        validatorMock.ValidateAsync(Arg.Any<IValidationContext>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(validationFailures));

        var page = new EditModel(Substitute.For<IOfficeService>(), validatorMock)
            { Id = Guid.Empty, Item = ItemTest, TempData = WebAppTestsSetup.PageTempData() };

        // Act
        var result = await page.OnPostAsync();

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<PageResult>();
        page.ModelState.IsValid.Should().BeFalse();
    }
}
