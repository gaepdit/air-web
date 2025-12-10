using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.ComplianceWork;
using AirWeb.AppServices.Compliance.ComplianceWork.WorkEntryDto.Command;
using AirWeb.AppServices.Compliance.ComplianceWork.WorkEntryDto.Query;
using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Staff;
using AirWeb.Domain.Identity;
using AirWeb.WebApp.Models;
using AutoMapper;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.WebApp.Pages.Compliance.Work.Edit;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public abstract class EditBase(IWorkEntryService entryService, IStaffService staffService, IMapper mapper)
    : PageModel, ISubmitCancelButtons
{
    protected readonly IMapper Mapper = mapper;

    [FromRoute]
    public int Id { get; set; }

    public IWorkEntrySummaryDto ItemView { get; protected set; } = null!;
    public SelectList StaffSelectList { get; private set; } = null!;

    // Form buttons
    public string SubmitText => "Save Changes";
    public string CancelRoute => "../Details";
    public string RouteId => Id.ToString();

    protected async Task<IActionResult> DoGetAsync(CancellationToken token)
    {
        if (Id == 0) return RedirectToPage("../Index");

        var itemView = await entryService.FindAsync(Id, false, token);
        if (itemView is null) return NotFound();
        if (!User.CanEdit(itemView)) return Forbid();

        ItemView = itemView;

        await PopulateSelectListsAsync();
        return Page();
    }

    protected async Task<IActionResult> DoPostAsync<TDto>(
        TDto item, IValidator<TDto> validator, CancellationToken token)
        where TDto : IWorkEntryCommandDto
    {
        var itemView = await entryService.FindSummaryAsync(Id, token);
        if (itemView is null || !User.CanEdit(itemView)) return BadRequest();
        await validator.ApplyValidationAsync(item, ModelState);

        if (!ModelState.IsValid)
        {
            ItemView = itemView;
            await PopulateSelectListsAsync();
            return Page();
        }

        var result = await entryService.UpdateAsync(Id, item, token);
        var entryType = await entryService.GetWorkEntryTypeAsync(Id, token);
        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success,
            $"{entryType!.Value.GetDisplayName()} successfully updated.");
        if (result.HasWarning) TempData.AddDisplayMessage(DisplayMessage.AlertContext.Warning, result.WarningMessage);
        return RedirectToPage("../Details", new { Id });
    }

    // FUTURE: Allow for editing a Work Entry previously reviewed by a currently inactive user.
    protected virtual async Task PopulateSelectListsAsync() =>
        StaffSelectList = (await staffService.GetUsersInRoleAsync(AppRole.ComplianceStaffRole)).ToSelectList();
}
