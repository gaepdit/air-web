using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Enforcement.EnforcementActionQuery;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using Microsoft.Identity.Web;
using System.Security.Claims;

namespace AirWeb.AppServices.Enforcement.Permissions;

public static class EnforcementActionPermissions
{
    extension(IActionViewDto item)
    {
        public bool CanAddResponse() =>
            item is { IsDeleted: false, Status: EnforcementActionStatus.Issued, IsUnderReview: false }
                and ResponseRequestedViewDto { IsResponseReceived: false };

        public bool CanBeAppealed() =>
            item is { IsIssued: true, IsCanceled: false, IsDeleted: false, IsUnderReview: false }
                and IIsExecuted { IsExecuted: true }
                and IIsAppealed { IsAppealed: false };

        public bool CanBeExecuted() =>
            item is { IsCanceled: false, IsDeleted: false, IsUnderReview: false } and IIsExecuted { IsExecuted: false };
    }

    extension(ClaimsPrincipal user)
    {
        public bool CanDeleteAction(IActionViewDto item) =>
            !item.IsDeleted && user.CanManageCaseFileDeletions();

        public bool CanEdit(IActionViewDto item) =>
            item is { IsCanceled: false, IsDeleted: false } && user.IsComplianceStaff();

        public bool CanEditStipulatedPenalties(IActionViewDto item) =>
            user.CanEdit(item) &&
            item is { ActionType: EnforcementActionType.ConsentOrder, IsUnderReview: false }
                and IIsExecuted { IsExecuted: true };

        public bool CanFinalizeAction(IActionViewDto item) =>
            item is { IsIssued: false, IsCanceled: false, IsUnderReview: false } &&
            (item is not IIsExecuted executed || executed.IsExecuted) &&
            user.CanFinalize(item);

        public bool CanProceedToCo(IActionViewDto item) =>
            user.CanEdit(item) &&
            item is
            {
                Status: EnforcementActionStatus.Issued,
                ActionType: EnforcementActionType.ProposedConsentOrder,
                IsUnderReview: false,
            };

        public bool CanResolve(IActionViewDto item) =>
            user.CanEdit(item) &&
            item is { Status: EnforcementActionStatus.Issued, IsUnderReview: false }
                and IIsResolved { IsResolved: false };

        public bool CanResolveWithNfa(IActionViewDto item) =>
            user.CanEdit(item) &&
            item is
            {
                Status: EnforcementActionStatus.Issued,
                ActionType: EnforcementActionType.NoticeOfViolation,
                IsUnderReview: false,
            };

        public bool CanRequestReview(IActionViewDto item) =>
            user.CanEdit(item) && item is { Status: EnforcementActionStatus.Draft, IsUnderReview: false };

        public bool CanReview(IActionViewDto item) =>
            user.CanEdit(item) && item is { Status: EnforcementActionStatus.ReviewRequested, IsUnderReview: true } &&
            (user.IsReviewerFor(item) || user.IsEnforcementReviewer());

        private bool IsReviewerFor(IActionViewDto item) =>
            item.CurrentReviewer != null && item.CurrentReviewer.Id.Equals(user.GetNameIdentifierId());
    }

    // Finalize = Issue or Cancel
}
