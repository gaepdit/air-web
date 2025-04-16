using AirWeb.AppServices.CommonInterfaces;
using AirWeb.AppServices.Permissions.Helpers;
using System.Security.Claims;

namespace AirWeb.AppServices.Permissions.ComplianceStaff;

public static class CommonComplianceStaffPermissions
{
    public static bool CanAddComment(this ClaimsPrincipal user, IIsDeleted item) =>
        !item.IsDeleted && user.IsComplianceStaff();

    public static bool CanClose(this ClaimsPrincipal user, IIsClosedAndIsDeleted item) =>
        !item.IsClosed && user.CanFinalize(item);

    public static bool CanDelete(this ClaimsPrincipal user, IIsDeleted item) =>
        !item.IsDeleted && user.CanManageDeletions();

    public static bool CanDeleteComment(this ClaimsPrincipal user, IHasOwnerAndDeletable item) =>
        !item.IsDeleted && (user.CanManageDeletions() || user.IsOwner(item));

    public static bool CanEdit(this ClaimsPrincipal user, IIsClosedAndIsDeleted item) =>
        item is { IsClosed: false, IsDeleted: false } && user.IsComplianceStaff();

    public static bool CanEdit(this ClaimsPrincipal user, IIsDeleted item) =>
        !item.IsDeleted && user.IsComplianceStaff();

    public static bool CanFinalize(this ClaimsPrincipal user, IIsDeleted item) =>
        !item.IsDeleted && user.IsComplianceStaff();

    public static bool CanManageDeletions(this ClaimsPrincipal user) =>
        user.IsComplianceManager();

    public static bool CanReopen(this ClaimsPrincipal user, IIsClosedAndIsDeleted item) =>
        item.IsClosed && user.CanFinalize(item);

    public static bool CanRestore(this ClaimsPrincipal user, IIsDeleted item) =>
        item.IsDeleted && user.CanManageDeletions();

    public static bool CanView(this ClaimsPrincipal user, IIsClosedAndIsDeleted item) =>
        user.CanManageDeletions() ||
        !item.IsDeleted && user.IsComplianceStaff() ||
        item.IsClosed && user.IsStaff();
}
