using AirWeb.AppServices.CommonInterfaces;
using AirWeb.AppServices.Permissions.Helpers;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Identity.Web;
using System.Security.Claims;

namespace AirWeb.AppServices.Enforcement.Permissions;

public class EnforcementOperation :
    OperationAuthorizationRequirement // implements IAuthorizationRequirement
{
    private EnforcementOperation(string name)
    {
        Name = name;
        AllOperations.Add(this);
    }

    public static List<EnforcementOperation> AllOperations { get; } = [];

    public static readonly EnforcementOperation AddComment = new(nameof(AddComment));
    public static readonly EnforcementOperation Close = new(nameof(Close));
    public static readonly EnforcementOperation Delete = new(nameof(Delete));
    public static readonly EnforcementOperation DeleteComment = new(nameof(DeleteComment));
    public static readonly EnforcementOperation Edit = new(nameof(Edit));
    public static readonly EnforcementOperation Reopen = new(nameof(Reopen));
    public static readonly EnforcementOperation Restore = new(nameof(Restore));
    public static readonly EnforcementOperation View = new(nameof(View));
    public static readonly EnforcementOperation ViewDeleted = new(nameof(ViewDeleted));

    // Operation requirement handler helpers
    internal static bool CanAddComment(ClaimsPrincipal user, IDeletable item) =>
        user.IsComplianceStaff() && !item.IsDeleted;

    internal static bool CanClose(ClaimsPrincipal user, ICloseableAndDeletable item) =>
        CanCloseOrReopen(user, item) && !item.IsClosed;

    private static bool CanCloseOrReopen(ClaimsPrincipal user, IDeletable item) =>
        user.IsComplianceStaff() && !item.IsDeleted;

    internal static bool CanDelete(ClaimsPrincipal user, IDeletable item) =>
        CanManageDeletions(user) && !item.IsDeleted;

    internal static bool CanDeleteComment(ClaimsPrincipal user, IHasOwnerAndDeletable item) =>
        (CanManageDeletions(user) || IsOwner(user, item)) && !item.IsDeleted;

    internal static bool CanEdit(ClaimsPrincipal user, ICloseableAndDeletable item) =>
        user.IsComplianceStaff() && item is { IsClosed: false, IsDeleted: false };

    internal static bool CanEdit(ClaimsPrincipal user, IDeletable item) =>
        user.IsComplianceStaff() && !item.IsDeleted;

    internal static bool CanManageDeletions(ClaimsPrincipal user) => user.IsComplianceManager();

    internal static bool CanReopen(ClaimsPrincipal user, ICloseableAndDeletable item) =>
        CanCloseOrReopen(user, item) && item.IsClosed;

    internal static bool CanRestore(ClaimsPrincipal user, IDeletable item) =>
        CanManageDeletions(user) && item.IsDeleted;

    internal static bool CanView(ClaimsPrincipal user, ICloseableAndDeletable item) =>
        CanManageDeletions(user) ||
        !item.IsDeleted && user.IsComplianceStaff() ||
        item.IsClosed && user.IsStaff();

    private static bool IsOwner(ClaimsPrincipal user, IHasOwner item) =>
        item.OwnerId.Equals(user.GetNameIdentifierId());
}
