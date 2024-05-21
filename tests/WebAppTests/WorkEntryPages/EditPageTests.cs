using AirWeb.AppServices.EntryTypes;
using AirWeb.AppServices.Staff;
using AirWeb.AppServices.WorkEntries;
using AirWeb.AppServices.WorkEntries.CommandDto;
using AirWeb.WebApp.Pages.Staff.WorkEntries;

namespace WebAppTests.WorkEntryPages;

[TestFixture]
public class EditPageTests
{
    private IWorkEntryService _workEntryService = null!;
    private IStaffService _staffService = null!;
    private IEntryTypeService _entryTypeService = null!;

    [SetUp]
    public void Setup()
    {
        _workEntryService = Substitute.For<IWorkEntryService>();
        _staffService = Substitute.For<IStaffService>();
        _entryTypeService = Substitute.For<IEntryTypeService>();
        _entryTypeService.GetAsListItemsAsync(Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(new List<ListItem>());
        _staffService.GetAsListItemsAsync(Arg.Any<bool>()).Returns(new List<ListItem<string>>());
    }

    [TearDown]
    public void Teardown()
    {
        _workEntryService.Dispose();
        _staffService.Dispose();
        _entryTypeService.Dispose();
    }

    [Test]
    public async Task OnGet_ReturnsPage()
    {
        // Arrange
        const int id = 909;
        var dto = new BaseWorkEntryUpdateDto();

        var workEntryService = Substitute.For<IWorkEntryService>();
        workEntryService.FindForUpdateAsync(id).Returns(dto);

        var authorization = Substitute.For<IAuthorizationService>();
        authorization.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Any<object?>(),
                Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var page = new EditModel(workEntryService, _entryTypeService, Substitute.For<IValidator<BaseWorkEntryUpdateDto>>(),
            authorization);

        // Act
        var result = await page.OnGetAsync(id);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<PageResult>();
        page.Item.Should().BeOfType<BaseWorkEntryUpdateDto>();
        page.Item.Should().Be(dto);
    }

    [Test]
    public async Task OnPost_ReturnsRedirectResultWhenModelIsValid()
    {
        // Arrange
        var validator = Substitute.For<IValidator<BaseWorkEntryUpdateDto>>();
        var authorization = Substitute.For<IAuthorizationService>();
        var page = new EditModel(_workEntryService, _entryTypeService, validator, authorization)
        {
            Id = 910,
            Item = new BaseWorkEntryUpdateDto(),
            TempData = WebAppTestsSetup.PageTempData(),
        };

        _workEntryService.FindForUpdateAsync(page.Id).Returns(page.Item);

        authorization.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Any<object?>(),
                Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        validator.ValidateAsync(Arg.Any<BaseWorkEntryUpdateDto>())
            .Returns(new ValidationResult());

        // Act
        var result = await page.OnPostAsync();

        // Assert
        result.Should().BeOfType<RedirectToPageResult>();
    }

    [Test]
    public async Task OnPost_ReturnsBadRequestWhenOriginalEntryIsNull()
    {
        // Arrange
        var validator = Substitute.For<IValidator<BaseWorkEntryUpdateDto>>();
        var authorization = Substitute.For<IAuthorizationService>();
        var page = new EditModel(_workEntryService, _entryTypeService, validator, authorization)
        {
            Id = 911,
            TempData = WebAppTestsSetup.PageTempData(),
        };

        _workEntryService.FindForUpdateAsync(page.Id).Returns((BaseWorkEntryUpdateDto?)null);

        // Act
        var result = await page.OnPostAsync();

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPost_ReturnsBadRequestWhenUserCannotEdit()
    {
        // Arrange
        var validator = Substitute.For<IValidator<BaseWorkEntryUpdateDto>>();
        var authorization = Substitute.For<IAuthorizationService>();
        var page = new EditModel(_workEntryService, _entryTypeService, validator, authorization)
            { Id = 912 };

        _workEntryService.FindForUpdateAsync(page.Id)
            .Returns(new BaseWorkEntryUpdateDto());
        authorization.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Any<object?>(),
                Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Failed());
        validator.ValidateAsync(Arg.Any<BaseWorkEntryUpdateDto>())
            .Returns(new ValidationResult());

        // Act
        var result = await page.OnPostAsync();

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPost_ReturnsPageResultWhenModelStateIsNotValid()
    {
        // Arrange
        var validator = Substitute.For<IValidator<BaseWorkEntryUpdateDto>>();
        var authorization = Substitute.For<IAuthorizationService>();
        var page = new EditModel(_workEntryService, _entryTypeService, validator, authorization)
            { Id = 913 };

        page.ModelState.AddModelError("test", "test error");

        _workEntryService.FindForUpdateAsync(page.Id)
            .Returns(new BaseWorkEntryUpdateDto());
        authorization.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Any<object?>(),
                Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());
        validator.ValidateAsync(Arg.Any<BaseWorkEntryUpdateDto>())
            .Returns(new ValidationResult());

        // Act
        var result = await page.OnPostAsync();

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<PageResult>();
        page.ModelState.IsValid.Should().BeFalse();
    }
}
