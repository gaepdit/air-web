using FluentValidation;
using GaEpd.AppLibrary.ListItems;
using AirWeb.AppServices.EntryTypes;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.WorkEntries;
using AirWeb.AppServices.WorkEntries.CommandDto;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;

namespace AirWeb.WebApp.Pages.Staff.WorkEntries;

[Authorize(Policy = nameof(Policies.StaffUser))]
public class AddModel(
    IWorkEntryService workEntryService,
    IEntryTypeService entryTypeService,
    IValidator<WorkEntryCreateDto> validator) : PageModel
{
    [BindProperty]
    public WorkEntryCreateDto Item { get; set; } = default!;

    public SelectList EntryTypesSelectList { get; private set; } = default!;

    public async Task OnGetAsync()
    {
        Item = new WorkEntryCreateDto();
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
        EntryTypesSelectList = (await entryTypeService.GetAsListItemsAsync()).ToSelectList();
}
