using AirWeb.AppServices.CommonInterfaces;
using AirWeb.AppServices.Permissions.Helpers;
using Microsoft.Identity.Web;
using System.Security.Claims;

namespace AirWeb.AppServices.Enforcement.Permissions;

public static class EnforcementPermissions
{
    public static bool CanAddComment(this ClaimsPrincipal user, IIsDeleted item) =>
        !item.IsDeleted && user.IsComplianceStaff();

    public static bool CanClose(this ClaimsPrincipal user, IIsClosedAndIsDeleted item) =>
        !item.IsClosed && CanCloseOrReopen(user, item);

    private static bool CanCloseOrReopen(this ClaimsPrincipal user, IIsDeleted item) =>
        !item.IsDeleted && user.IsComplianceStaff();

    public static bool CanDelete(this ClaimsPrincipal user, IDeletable item) =>
        !item.IsDeleted && CanManageDeletions(user);

    public static bool CanDeleteComment(this ClaimsPrincipal user, IHasOwnerAndDeletable item) =>
        !item.IsDeleted && (CanManageDeletions(user) || IsOwner(user, item));

    public static bool CanEdit(this ClaimsPrincipal user, IIsClosedAndIsDeleted item) =>
        item is { IsClosed: false, IsDeleted: false } && user.IsComplianceStaff();

    public static bool CanEdit(this ClaimsPrincipal user, IIsDeleted item) =>
        !item.IsDeleted && user.IsComplianceStaff();

    public static bool CanManageDeletions(this ClaimsPrincipal user) =>
        user.IsComplianceManager();

    public static bool CanReopen(this ClaimsPrincipal user, IIsClosedAndIsDeleted item) =>
        item.IsClosed && CanCloseOrReopen(user, item);

    public static bool CanRestore(this ClaimsPrincipal user, IDeletable item) =>
        item.IsDeleted && CanManageDeletions(user);

    public static bool CanView(this ClaimsPrincipal user, IIsClosedAndIsDeleted item) =>
        CanManageDeletions(user) ||
        !item.IsDeleted && user.IsComplianceStaff() ||
        item.IsClosed && user.IsStaff();

    private static bool IsOwner(this ClaimsPrincipal user, IHasOwner item) =>
        item.OwnerId.Equals(user.GetNameIdentifierId());
}
