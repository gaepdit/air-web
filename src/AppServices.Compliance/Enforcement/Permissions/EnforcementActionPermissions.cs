using AirWeb.AppServices.Compliance.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.Enforcement.EnforcementActionQuery;
using AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions;
using Microsoft.Identity.Web;
using System.Security.Claims;

namespace AirWeb.AppServices.Compliance.Enforcement.Permissions;

public static class EnforcementActionPermissions
{
    extension(IActionViewDto item)
    {
        public bool CanAddResponse() =>
            item is { IsIssued: true, IsDeleted: false }
                and ResponseRequestedViewDto { IsResponseReceived: false };

        public bool CanBeAppealed() =>
            item is { IsIssued: true, IsDeleted: false }
                and IIsExecuted { IsExecuted: true }
                and IIsAppealed { IsAppealed: false };

        public bool CanBeExecuted() =>
            item is { IsCanceled: false, IsDeleted: false, IsUnderReview: false }
                and IIsExecuted { IsExecuted: false };
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

        // Finalize = Issue or Cancel
        public bool CanFinalizeAction(IActionViewDto item) =>
            user.CanEdit(item) &&
            item is { IsIssued: false, IsUnderReview: false } &&
            (item is not IIsExecuted executed || executed.IsExecuted);

        public bool CanProposeCo(IActionViewDto item) =>
            user.CanEdit(item) &&
            item is { ActionType: EnforcementActionType.NoticeOfViolation, IsIssued: true };

        public bool CanProceedToCo(IActionViewDto item) =>
            user.CanEdit(item) &&
            item is { ActionType: EnforcementActionType.ProposedConsentOrder, IsIssued: true };

        public bool CanResolve(IActionViewDto item) =>
            user.CanEdit(item) &&
            item is { IsIssued: true }
                and IIsResolved { IsResolved: false };

        public bool CanResolveWithNfa(IActionViewDto item) =>
            user.CanEdit(item) &&
            item is { ActionType: EnforcementActionType.NoticeOfViolation, IsIssued: true };

        public bool CanRequestReview(IActionViewDto item) =>
            user.CanEdit(item) &&
            item is { Status: EnforcementActionStatus.Draft };

        public bool CanReview(IActionViewDto item) =>
            user.CanEdit(item) &&
            item.IsUnderReview &&
            (user.IsReviewerFor(item) || user.IsEnforcementReviewer());

        public bool CanWithdrawReviewRequest(IActionViewDto item) =>
            item.IsUnderReview &&
            (user.IsResponsibleStaff(item) || user.IsEnforcementReviewer());

        private bool IsResponsibleStaff(IActionViewDto item) =>
            item.CaseFileResponsibleStaff?.Id.Equals(user.GetNameIdentifierId()) ?? false;

        private bool IsReviewerFor(IActionViewDto item) =>
            item.CurrentReviewer != null && item.CurrentReviewer.Id.Equals(user.GetNameIdentifierId());
    }
}
