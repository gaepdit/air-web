using AirWeb.AppServices.AuthenticationServices.Roles;
using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Enforcement.EnforcementActionQuery;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using Microsoft.Identity.Web;
using System.Security.Claims;

namespace AirWeb.AppServices.Enforcement.Permissions;

public static class EnforcementActionPermissions
{
    public static bool CanAddResponse(this IActionViewDto item) =>
        item is { IsDeleted: false, Status: EnforcementActionStatus.Issued, IsUnderReview: false }
            and ResponseRequestedViewDto { IsResponseReceived: false };

    public static bool CanBeAppealed(this IActionViewDto item) =>
        item is { IsIssued: true, IsCanceled: false, IsDeleted: false, IsUnderReview: false }
            and IIsExecuted { IsExecuted: true }
            and IIsAppealed { IsAppealed: false };

    public static bool CanBeExecuted(this IActionViewDto item) =>
        item is { IsCanceled: false, IsDeleted: false, IsUnderReview: false } and IIsExecuted { IsExecuted: false };

    public static bool CanDeleteAction(this ClaimsPrincipal user, IActionViewDto item) =>
        !item.IsDeleted && user.IsEnforcementManager();

    public static bool CanEdit(this ClaimsPrincipal user, IActionViewDto item) =>
        item is { IsCanceled: false, IsDeleted: false } && user.IsComplianceStaff();

    public static bool CanEditStipulatedPenalties(this ClaimsPrincipal user, IActionViewDto item) =>
        user.CanEdit(item) &&
        item is { ActionType: EnforcementActionType.ConsentOrder, IsUnderReview: false }
            and IIsExecuted { IsExecuted: true };

    // Finalize = Issue or Cancel
    public static bool CanFinalizeAction(this ClaimsPrincipal user, IActionViewDto item) =>
        item is { IsIssued: false, IsCanceled: false, IsUnderReview: false } &&
        (item is not IIsExecuted executed || executed.IsExecuted) &&
        user.CanFinalize(item);

    public static bool CanProceedToCo(this ClaimsPrincipal user, IActionViewDto item) =>
        user.CanEdit(item) &&
        item is
        {
            Status: EnforcementActionStatus.Issued,
            ActionType: EnforcementActionType.ProposedConsentOrder,
            IsUnderReview: false,
        };

    public static bool CanResolve(this ClaimsPrincipal user, IActionViewDto item) =>
        user.CanEdit(item) &&
        item is { Status: EnforcementActionStatus.Issued, IsUnderReview: false }
            and IIsResolved { IsResolved: false };

    public static bool CanResolveWithNfa(this ClaimsPrincipal user, IActionViewDto item) =>
        user.CanEdit(item) &&
        item is
        {
            Status: EnforcementActionStatus.Issued,
            ActionType: EnforcementActionType.NoticeOfViolation,
            IsUnderReview: false,
        };

    public static bool CanRequestReview(this ClaimsPrincipal user, IActionViewDto item) =>
        user.CanEdit(item) && item is { Status: EnforcementActionStatus.Draft, IsUnderReview: false };

    public static bool CanReview(this ClaimsPrincipal user, IActionViewDto item) =>
        user.CanEdit(item) && item is { Status: EnforcementActionStatus.ReviewRequested, IsUnderReview: true } &&
        (user.IsReviewerFor(item) || user.IsEnforcementReviewer());

    private static bool IsReviewerFor(this ClaimsPrincipal user, IActionViewDto item) =>
        item.CurrentReviewer != null && item.CurrentReviewer.Id.Equals(user.GetNameIdentifierId());
}
