using FluentValidation;
using GaEpd.AppLibrary.ListItems;
using MyApp.AppServices.EntryTypes;
using MyApp.AppServices.Permissions;
using MyApp.AppServices.Permissions.Helpers;
using MyApp.AppServices.WorkEntries;
using MyApp.AppServices.WorkEntries.CommandDto;
using MyApp.AppServices.WorkEntries.Permissions;
using MyApp.WebApp.Models;
using MyApp.WebApp.Platform.PageModelHelpers;

namespace MyApp.WebApp.Pages.Staff.WorkEntries;

[Authorize(Policy = nameof(Policies.StaffUser))]
public class EditModel(
    IWorkEntryService workEntryService,
    IEntryTypeService entryTypeService,
    IValidator<WorkEntryUpdateDto> validator,
    IAuthorizationService authorization) : PageModel
{
    [FromRoute]
    public Guid Id { get; set; }

    [BindProperty]
    public WorkEntryUpdateDto Item { get; set; } = default!;

    public SelectList EntryTypesSelectList { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("Index");
        var item = await workEntryService.FindForUpdateAsync(id.Value);
        if (item is null) return NotFound();
        if (!await UserCanEditAsync(item)) return Forbid();

        Id = id.Value;
        Item = item;

        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var original = await workEntryService.FindForUpdateAsync(Id);
        if (original is null || !await UserCanEditAsync(original)) return BadRequest();

        await validator.ApplyValidationAsync(Item, ModelState);

        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync();
            return Page();
        }

        await workEntryService.UpdateAsync(Id, Item);

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Work Entry successfully updated.");
        return RedirectToPage("Details", new { Id });
    }

    private async Task PopulateSelectListsAsync() => 
        EntryTypesSelectList = (await entryTypeService.GetAsListItemsAsync()).ToSelectList();

    private Task<bool> UserCanEditAsync(WorkEntryUpdateDto item) =>
        authorization.Succeeded(User, item, new WorkEntryUpdateRequirements());
}
