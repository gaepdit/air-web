using AirWeb.AppServices.DomainEntities.Offices;
using AirWeb.AppServices.Staff;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.TestData.SampleData;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Pages.Account;
using AirWeb.WebApp.Platform.PageModelHelpers;

namespace WebAppTests.AccountPages;

public class EditTests
{
    private static readonly StaffViewDto StaffViewTest = new()
    {
        Id = Guid.NewGuid().ToString(),
        FamilyName = SampleText.ValidName,
        GivenName = SampleText.ValidName,
        Email = SampleText.ValidEmail,
        Active = true,
    };

    private static readonly StaffUpdateDto StaffUpdateTest = new() { Active = true };

    [Test]
    public async Task OnGet_PopulatesThePageModel()
    {
        // Arrange
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.GetCurrentUserAsync()
            .Returns(StaffViewTest);

        var officeServiceMock = Substitute.For<IOfficeService>();
        officeServiceMock.GetAsListItemsAsync(Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(new List<ListItem>());

        var pageModel = new EditModel(staffServiceMock, officeServiceMock, Substitute.For<IValidator<StaffUpdateDto>>())
            { TempData = WebAppTestsSetup.PageTempData() };

        // Act
        var result = await pageModel.OnGetAsync();

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<PageResult>();
        pageModel.DisplayStaff.Should().Be(StaffViewTest);
        pageModel.UpdateStaff.Should().BeEquivalentTo(StaffViewTest.AsUpdateDto());
        pageModel.OfficeSelectList.Should().BeEmpty();
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        // Arrange
        var expectedMessage =
            new DisplayMessage(DisplayMessage.AlertContext.Success, "Successfully updated profile.", []);

        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.GetCurrentUserAsync().Returns(StaffViewTest);
        staffServiceMock.UpdateAsync(Arg.Any<string>(), Arg.Any<StaffUpdateDto>()).Returns(IdentityResult.Success);

        var validatorMock = Substitute.For<IValidator<StaffUpdateDto>>();
        validatorMock.ValidateAsync(Arg.Any<StaffUpdateDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        var page = new EditModel(staffServiceMock, Substitute.For<IOfficeService>(), validatorMock)
            { UpdateStaff = StaffUpdateTest, TempData = WebAppTestsSetup.PageTempData() };

        // Act
        var result = await page.OnPostAsync();

        // Assert
        using var scope = new AssertionScope();
        page.ModelState.IsValid.Should().BeTrue();
        result.Should().BeOfType<RedirectToPageResult>();
        ((RedirectToPageResult)result).PageName.Should().Be("Index");
        page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expectedMessage);
    }

    [Test]
    public async Task OnPost_GivenUpdateFailure_ReturnsBadRequest()
    {
        // Arrange
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.GetCurrentUserAsync().Returns(StaffViewTest);
        staffServiceMock.UpdateAsync(Arg.Any<string>(), Arg.Any<StaffUpdateDto>()).Returns(IdentityResult.Failed());

        var validatorMock = Substitute.For<IValidator<StaffUpdateDto>>();
        validatorMock.ValidateAsync(Arg.Any<StaffUpdateDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        var page = new EditModel(staffServiceMock, Substitute.For<IOfficeService>(), validatorMock)
            { UpdateStaff = StaffUpdateTest, TempData = WebAppTestsSetup.PageTempData() };

        // Act
        var result = await page.OnPostAsync();

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPost_GivenInvalidModel_ReturnsPageWithInvalidModelState()
    {
        // Arrange
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.GetCurrentUserAsync().Returns(StaffViewTest);

        var officeServiceMock = Substitute.For<IOfficeService>();
        officeServiceMock.GetAsListItemsAsync(Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(new List<ListItem>());

        var validatorMock = Substitute.For<IValidator<StaffUpdateDto>>();

        var validationFailures = new List<ValidationFailure> { new("property", "message") };
        validatorMock.ValidateAsync(Arg.Any<StaffUpdateDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(validationFailures));

        var page = new EditModel(staffServiceMock, officeServiceMock, validatorMock)
            { UpdateStaff = StaffUpdateTest, TempData = WebAppTestsSetup.PageTempData() };

        // Act
        var result = await page.OnPostAsync();

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<PageResult>();
        page.ModelState.IsValid.Should().BeFalse();
        page.DisplayStaff.Should().Be(StaffViewTest);
        page.UpdateStaff.Should().Be(StaffUpdateTest);
    }
}
