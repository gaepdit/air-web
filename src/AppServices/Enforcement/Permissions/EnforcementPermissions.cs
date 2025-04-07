using AirWeb.AppServices.Enforcement.EnforcementActionQuery;
using AirWeb.AppServices.Permissions.ComplianceStaff;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using System.Security.Claims;

namespace AirWeb.AppServices.Enforcement.Permissions;

public static class EnforcementPermissions
{
    public static bool CanAddResponse(this IActionViewDto item) =>
        item is { IsDeleted: false, Status: EnforcementActionStatus.Issued }
            and ResponseRequestedViewDto { IsResponseReceived: false };

    public static bool CanBeExecuted(this IActionViewDto item) =>
        item is { IsCanceled: false, IsDeleted: false } and IIsExecuted { IsExecuted: false };

    public static bool CanBeAppealed(this IActionViewDto item) =>
        item is { IsIssued: true, IsCanceled: false, IsDeleted: false }
            and IIsExecuted { IsExecuted: true }
            and IIsAppealed { IsAppealed: false };

    public static bool CanEdit(this ClaimsPrincipal user, IActionViewDto item) =>
        item is { IsCanceled: false, IsDeleted: false } && user.IsComplianceStaff();

    public static bool CanFinalizeAction(this ClaimsPrincipal user, IActionViewDto item) =>
        item is { IsIssued: false, IsCanceled: false } &&
        (item is not IIsExecuted executed || executed.IsExecuted) &&
        user.CanFinalize(item);

    public static bool CanResolve(this ClaimsPrincipal user, IActionViewDto item) =>
        user.CanEdit(item) && item.Status == EnforcementActionStatus.Issued &&
        item is IIsResolved { IsResolved: false };

    public static bool CanSubmitForReview(this ClaimsPrincipal user, IActionViewDto item) =>
        item is { IsDeleted: false, IsCanceled: false, Status: EnforcementActionStatus.Draft } &&
        user.IsComplianceStaff();
}
