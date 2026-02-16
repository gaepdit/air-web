using AirWeb.AppServices.Compliance.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.Enforcement;
using AirWeb.AppServices.Compliance.Enforcement.EnforcementActionCommand;
using AirWeb.AppServices.Compliance.Enforcement.EnforcementActionQuery;
using AirWeb.AppServices.Compliance.Enforcement.Permissions;
using AirWeb.AppServices.Core.EntityServices.Staff;
using AirWeb.Domain.Compliance.AppRoles;
using AirWeb.WebApp.Models;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.WebApp.Pages.Enforcement;

[Authorize(Policy = nameof(CompliancePolicies.ComplianceStaff))]
public class RequestReviewModel(
    IEnforcementActionService actionService,
    IStaffService staffService,
    IValidator<EnforcementActionRequestReviewDto> validator) : PageModel, ISubmitCancelButtons
{
    [FromRoute]
    public Guid Id { get; set; } // Enforcement Action ID

    [BindProperty]
    public EnforcementActionRequestReviewDto ItemReview { get; set; } = null!;

    public IActionViewDto ItemView { get; private set; } = null!;
    public SelectList StaffSelectList { get; private set; } = null!;

    // Form buttons
    public string SubmitText => "Request Review";
    public string CancelRoute => "Details";
    public string RouteId => ItemView.CaseFileId.ToString();

    [TempData]
    public Guid? HighlightEnforcementId { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken token)
    {
        var itemView = await actionService.FindAsync(Id, token);
        if (itemView is null) return NotFound();
        if (!User.CanRequestReview(itemView)) return Forbid();

        ItemView = itemView;
        ItemReview = new EnforcementActionRequestReviewDto();

        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        var itemView = await actionService.FindAsync(Id, token);
        if (itemView is null || !User.CanRequestReview(itemView)) return BadRequest();

        await validator.ApplyValidationAsync(ItemReview, ModelState);
        if (!ModelState.IsValid)
        {
            ItemView = itemView;
            await PopulateSelectListsAsync();
            return Page();
        }

        await actionService.RequestReviewAsync(Id, ItemReview, token);

        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success,
            $"{itemView.ActionType.GetDisplayName()} review requested.");
        HighlightEnforcementId = Id;

        return RedirectToPage("Details", pageHandler: null, routeValues: new { Id = itemView.CaseFileId },
            fragment: Id.ToString());
    }

    private async Task PopulateSelectListsAsync() =>
        StaffSelectList =
            (await staffService.GetUsersInRoleAsync(ComplianceRole.EnforcementReviewerRole)).ToSelectList();
}
