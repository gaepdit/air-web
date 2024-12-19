using AirWeb.AppServices.CommonInterfaces;
using Microsoft.Identity.Web;
using System.Security.Claims;

namespace AirWeb.AppServices.Permissions.Helpers;

// Operation requirement handlers
public static class GeneralOperationHandlers
{
    internal static bool CanAddComment(this ClaimsPrincipal user, IIsDeleted item) =>
        user.IsComplianceStaff() && !item.IsDeleted;

    internal static bool CanClose(this ClaimsPrincipal user, IIsClosedAndIsDeleted item) =>
        CanCloseOrReopen(user, item) && !item.IsClosed;

    private static bool CanCloseOrReopen(this ClaimsPrincipal user, IIsDeleted item) =>
        user.IsComplianceStaff() && !item.IsDeleted;

    internal static bool CanDelete(this ClaimsPrincipal user, IDeletable item) =>
        CanManageDeletions(user) && !item.IsDeleted;

    internal static bool CanDeleteComment(this ClaimsPrincipal user, IHasOwnerAndDeletable item) =>
        (CanManageDeletions(user) || IsOwner(user, item)) && !item.IsDeleted;

    internal static bool CanEdit(this ClaimsPrincipal user, IIsClosedAndIsDeleted item) =>
        user.IsComplianceStaff() && item is { IsClosed: false, IsDeleted: false };

    internal static bool CanEdit(this ClaimsPrincipal user, IIsDeleted item) =>
        user.IsComplianceStaff() && !item.IsDeleted;

    internal static bool CanManageDeletions(this ClaimsPrincipal user) => user.IsComplianceManager();

    internal static bool CanReopen(this ClaimsPrincipal user, IIsClosedAndIsDeleted item) =>
        CanCloseOrReopen(user, item) && item.IsClosed;

    internal static bool CanRestore(this ClaimsPrincipal user, IDeletable item) =>
        CanManageDeletions(user) && item.IsDeleted;

    private static bool IsOwner(this ClaimsPrincipal user, IHasOwner item) =>
        item.OwnerId.Equals(user.GetNameIdentifierId());
}
