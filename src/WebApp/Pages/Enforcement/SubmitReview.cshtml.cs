using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.Enforcement;
using AirWeb.AppServices.Enforcement.EnforcementActionCommand;
using AirWeb.AppServices.Enforcement.EnforcementActionQuery;
using AirWeb.AppServices.Enforcement.Permissions;
using AirWeb.AppServices.Staff;
using AirWeb.Domain.Roles;
using AirWeb.WebApp.Models;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.WebApp.Pages.Enforcement;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public class SubmitReviewModel(
    IEnforcementActionService actionService,
    IStaffService staffService,
    IValidator<EnforcementActionSubmitReviewDto> validator) : PageModel, ISubmitCancelButtons
{
    [FromRoute]
    public Guid Id { get; set; } // Enforcement Action ID

    [BindProperty]
    public EnforcementActionSubmitReviewDto ItemReview { get; set; } = null!;

    public IActionViewDto ItemView { get; private set; } = null!;
    public SelectList StaffSelectList { get; private set; } = null!;

    // Form buttons
    public string SubmitText => "Submit Review";
    public string CancelRoute => "Details";
    public string RouteId => ItemView.CaseFileId.ToString();

    [TempData]
    public Guid? HighlightEnforcementId { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken token)
    {
        if (Id == Guid.Empty) return RedirectToPage("Index");

        var itemView = await actionService.FindAsync(Id, token);
        if (itemView is null) return NotFound();
        if (!User.CanReview(itemView)) return Forbid();

        ItemView = itemView;
        ItemReview = new EnforcementActionSubmitReviewDto();

        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        var itemView = await actionService.FindAsync(Id, token);
        if (itemView is null || !User.CanReview(itemView)) return BadRequest();

        await validator.ApplyValidationAsync(ItemReview, ModelState);
        if (!ModelState.IsValid)
        {
            ItemView = itemView;
            await PopulateSelectListsAsync();
            return Page();
        }

        await actionService.SubmitReviewAsync(Id, ItemReview, token);

        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success,
            $"{itemView.ActionType.GetDisplayName()} review submitted.");
        HighlightEnforcementId = Id;

        return RedirectToPage("Details", pageHandler: null, routeValues: new { Id = itemView.CaseFileId },
            fragment: Id.ToString());
    }

    private async Task PopulateSelectListsAsync() => StaffSelectList =
        (await staffService.GetUsersInRoleAsync(AppRole.EnforcementReviewerRole)).ToSelectList();
}
