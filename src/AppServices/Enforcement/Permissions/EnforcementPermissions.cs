using AirWeb.AppServices.CommonInterfaces;
using AirWeb.AppServices.Permissions.Helpers;
using Microsoft.Identity.Web;
using System.Security.Claims;

namespace AirWeb.AppServices.Enforcement.Permissions;

public static class EnforcementPermissions
{
    public static bool CanAddComment(this ClaimsPrincipal user, IIsDeleted item) =>
        user.IsComplianceStaff() && !item.IsDeleted;

    public static bool CanClose(this ClaimsPrincipal user, IIsClosedAndIsDeleted item) =>
        CanCloseOrReopen(user, item) && !item.IsClosed;

    private static bool CanCloseOrReopen(this ClaimsPrincipal user, IIsDeleted item) =>
        user.IsComplianceStaff() && !item.IsDeleted;

    public static bool CanDelete(this ClaimsPrincipal user, IDeletable item) =>
        CanManageDeletions(user) && !item.IsDeleted;

    public static bool CanDeleteComment(this ClaimsPrincipal user, IHasOwnerAndDeletable item) =>
        (CanManageDeletions(user) || IsOwner(user, item)) && !item.IsDeleted;

    public static bool CanEdit(this ClaimsPrincipal user, IIsClosedAndIsDeleted item) =>
        user.IsComplianceStaff() && item is { IsClosed: false, IsDeleted: false };

    public static bool CanManageDeletions(this ClaimsPrincipal user) => user.IsComplianceManager();

    public static bool CanReopen(this ClaimsPrincipal user, IIsClosedAndIsDeleted item) =>
        CanCloseOrReopen(user, item) && item.IsClosed;

    public static bool CanRestore(this ClaimsPrincipal user, IDeletable item) =>
        CanManageDeletions(user) && item.IsDeleted;

    public static bool CanView(this ClaimsPrincipal user, IIsClosedAndIsDeleted item) =>
        CanManageDeletions(user) ||
        !item.IsDeleted && user.IsComplianceStaff() ||
        item.IsClosed && user.IsStaff();

    private static bool IsOwner(this ClaimsPrincipal user, IHasOwner item) =>
        item.OwnerId.Equals(user.GetNameIdentifierId());
}
