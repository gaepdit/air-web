using AirWeb.AppServices.Lookups.Offices;
using AirWeb.AppServices.Staff;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Core.AppRoles;
using AirWeb.Domain.AppRoles;
using AirWeb.TestData.SampleData;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Pages.Admin.Users;
using AirWeb.WebApp.Platform.PageModelHelpers;

namespace WebAppTests.UserPages;

public class EditRolesTests
{
    private static readonly OfficeViewDto OfficeViewTest = new()
        { Id = Guid.NewGuid(), Name = SampleText.ValidName, Active = true };

    private static readonly StaffViewDto StaffViewTest = new()
    {
        Id = Guid.NewGuid().ToString(),
        FamilyName = SampleText.ValidName,
        GivenName = SampleText.ValidName,
        Email = SampleText.ValidEmail,
        Office = OfficeViewTest,
        Active = true,
    };

    private static readonly List<EditRolesModel.RoleSetting> RoleSettingsTest =
    [
        new()
        {
            Name = SampleText.ValidName,
            Category = SampleText.ValidName,
            DisplayName = SampleText.ValidName,
            Description = SampleText.ValidName,
            IsSelected = true,
        },
    ];

    [Test]
    public async Task OnGet_PopulatesThePageModel()
    {
        // Arrange
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.FindAsync(Arg.Any<string>())
            .Returns(StaffViewTest);
        staffServiceMock.GetRolesAsync(Arg.Any<string>())
            .Returns(new List<string> { ComplianceRole.ComplianceSiteMaintenance });

        var pageModel = new EditRolesModel(staffServiceMock)
            { TempData = WebAppTestsSetup.PageTempData(), Id = new Guid(StaffViewTest.Id) };

        var expectedRoleSettings = AppRole.AllRoles!
            .Select(r => new EditRolesModel.RoleSetting
            {
                Name = r.Key,
                Category = r.Value.Category,
                DisplayName = r.Value.DisplayName,
                Description = r.Value.Description,
                IsSelected = r.Key == ComplianceRole.ComplianceSiteMaintenance,
            }).ToList();

        // Act
        var result = await pageModel.OnGetAsync();

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<PageResult>();
        pageModel.DisplayStaff.Should().Be(StaffViewTest);
        pageModel.OfficeName.Should().Be(SampleText.ValidName);
        pageModel.Id.Should().Be(StaffViewTest.Id);
        pageModel.RoleSettings.Should().BeEquivalentTo(expectedRoleSettings);
    }

    [Test]
    public async Task OnGet_MissingIdReturnsNotFound()
    {
        // Arrange
        var pageModel = new EditRolesModel(Substitute.For<IStaffService>())
            { TempData = WebAppTestsSetup.PageTempData(), Id = null };

        // Act
        var result = await pageModel.OnGetAsync();

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<RedirectToPageResult>();
        ((RedirectToPageResult)result).PageName.Should().Be("Index");
    }

    [Test]
    public async Task OnGet_NonexistentIdReturnsNotFound()
    {
        // Arrange
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.FindAsync(Arg.Any<string>())
            .Returns((StaffViewDto?)null);

        var pageModel = new EditRolesModel(staffServiceMock)
            { TempData = WebAppTestsSetup.PageTempData(), Id = Guid.Empty };

        // Act
        var result = await pageModel.OnGetAsync();

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        // Arrange
        var expectedMessage =
            new DisplayMessage(DisplayMessage.AlertContext.Success, "User roles successfully updated.");

        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.UpdateRolesAsync(Arg.Any<string>(), Arg.Any<Dictionary<string, bool>>())
            .Returns(IdentityResult.Success);
        staffServiceMock.GetRolesAsync(Arg.Any<string>())
            .Returns(new List<string> { ComplianceRole.ComplianceSiteMaintenance });

        var userId = Guid.NewGuid();
        var page = new EditRolesModel(staffServiceMock)
        {
            RoleSettings = RoleSettingsTest,
            Id = userId,
            TempData = WebAppTestsSetup.PageTempData(),
        };

        // Act
        var result = await page.OnPostAsync();

        // Assert
        using var scope = new AssertionScope();
        page.ModelState.IsValid.Should().BeTrue();
        result.Should().BeOfType<RedirectToPageResult>();
        ((RedirectToPageResult)result).PageName.Should().Be("Details");
        ((RedirectToPageResult)result).RouteValues!["id"].Should().Be(userId);
        page.TempData.GetDisplayMessages().Should().BeEquivalentTo([expectedMessage]);
    }

    [Test]
    public async Task OnPost_GivenMissingUser_ReturnsBadRequest()
    {
        // Arrange
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.UpdateRolesAsync(Arg.Any<string>(), Arg.Any<Dictionary<string, bool>>())
            .Returns(IdentityResult.Failed());
        staffServiceMock.FindAsync(Arg.Any<string>())
            .Returns((StaffViewDto?)null);

        var page = new EditRolesModel(staffServiceMock)
        {
            RoleSettings = RoleSettingsTest,
            Id = Guid.NewGuid(),
            TempData = WebAppTestsSetup.PageTempData(),
            PageContext = WebAppTestsSetup.PageContextWithUser(),
        };

        // Act
        var result = await page.OnPostAsync();

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPost_GivenUpdateFailure_ReturnsPageWithInvalidModelState()
    {
        // Arrange
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.UpdateRolesAsync(Arg.Any<string>(), Arg.Any<Dictionary<string, bool>>())
            .Returns(IdentityResult.Failed(new IdentityError { Code = "CODE", Description = "DESCRIPTION" }));
        staffServiceMock.FindAsync(Arg.Any<string>())
            .Returns(StaffViewTest);
        staffServiceMock.GetRolesAsync(Arg.Any<string>())
            .Returns(new List<string> { ComplianceRole.ComplianceSiteMaintenance });

        var userId = Guid.NewGuid();
        var page = new EditRolesModel(staffServiceMock)
        {
            RoleSettings = RoleSettingsTest,
            Id = userId,
            TempData = WebAppTestsSetup.PageTempData(),
            PageContext = WebAppTestsSetup.PageContextWithUser(),
        };

        // Act
        var result = await page.OnPostAsync();

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<PageResult>();
        page.ModelState.IsValid.Should().BeFalse();
        page.ModelState[string.Empty]!.Errors[0].ErrorMessage.Should().Be("CODE: DESCRIPTION");
        page.DisplayStaff.Should().Be(StaffViewTest);
        page.Id.Should().Be(userId);
        page.RoleSettings.Should().BeEquivalentTo(RoleSettingsTest);
    }
}
