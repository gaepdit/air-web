using AirWeb.AppServices.DomainEntities.NotificationTypes;
using AirWeb.AppServices.DomainEntities.WorkEntries;
using AirWeb.AppServices.DomainEntities.WorkEntries.BaseWorkEntryDto;
using AirWeb.AppServices.Permissions;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.WebApp.Pages.Staff.WorkEntries;

[Authorize(Policy = nameof(Policies.StaffUser))]
public class AddModel(
    IWorkEntryService workEntryService,
    INotificationTypeService notificationTypeService,
    IValidator<BaseWorkEntryCreateDto> validator) : PageModel
{
    [BindProperty]
    public BaseWorkEntryCreateDto Item { get; set; } = default!;

    public SelectList EntryTypesSelectList { get; private set; } = default!;

    public async Task OnGetAsync()
    {
        Item = new BaseWorkEntryCreateDto();
        await PopulateSelectListsAsync();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        await validator.ApplyValidationAsync(Item, ModelState);

        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync();
            return Page();
        }

        var createResult = await workEntryService.CreateAsync(Item, token);

        TempData.SetDisplayMessage(
            createResult.HasWarnings ? DisplayMessage.AlertContext.Warning : DisplayMessage.AlertContext.Success,
            "WorkEntry successfully created.", createResult.Warnings);

        return RedirectToPage("Details", new { id = createResult.WorkEntryId });
    }

    private async Task PopulateSelectListsAsync() =>
        EntryTypesSelectList = (await notificationTypeService.GetAsListItemsAsync()).ToSelectList();
}
