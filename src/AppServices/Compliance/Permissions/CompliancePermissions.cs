using AirWeb.AppServices.CommonInterfaces;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;
using AirWeb.AppServices.Permissions.Helpers;
using Microsoft.Identity.Web;
using System.Security.Claims;

namespace AirWeb.AppServices.Compliance.Permissions;

public static class CompliancePermissions
{
    internal static bool CanAddComment(this ClaimsPrincipal user, IDeletable item) =>
        user.IsComplianceStaff() && !item.IsDeleted;

    public static bool CanBeginEnforcement(this ClaimsPrincipal user, IWorkEntrySummaryDto item) =>
        item is { IsComplianceEvent: true, IsDeleted: false } && user.IsOwnerOrManager(item);

    public static bool CanClose(this ClaimsPrincipal user, IIsClosedAndIsDeleted item) =>
        user.CanCloseOrReopen(item) && !item.IsClosed;

    private static bool CanCloseOrReopen(this ClaimsPrincipal user, IIsDeleted item) =>
        user.IsComplianceStaff() && !item.IsDeleted;

    public static bool CanDelete(this ClaimsPrincipal user, IDeletable item) =>
        user.CanManageDeletions() && !item.IsDeleted;

    internal static bool CanDeleteComment(this ClaimsPrincipal user, IHasOwnerAndDeletable item) =>
        (user.CanManageDeletions() || user.IsOwner(item)) && !item.IsDeleted;

    public static bool CanEdit(this ClaimsPrincipal user, IIsClosedAndIsDeleted item) =>
        user.IsComplianceStaff() && item is { IsClosed: false, IsDeleted: false };

    public static bool CanEdit(this ClaimsPrincipal user, IIsDeleted item) =>
        user.IsComplianceStaff() && !item.IsDeleted;

    public static bool CanManageDeletions(this ClaimsPrincipal user) => user.IsComplianceManager();

    public static bool CanReopen(this ClaimsPrincipal user, IIsClosedAndIsDeleted item) =>
        user.CanCloseOrReopen(item) && item.IsClosed;

    public static bool CanRestore(this ClaimsPrincipal user, IDeletable item) =>
        user.CanManageDeletions() && item.IsDeleted;

    private static bool IsOwner(this ClaimsPrincipal user, IHasOwner item) =>
        item.OwnerId.Equals(user.GetNameIdentifierId());

    private static bool IsOwnerOrManager(this ClaimsPrincipal user, IHasOwner item) =>
        user.IsOwner(item) || user.IsComplianceManager();
}
