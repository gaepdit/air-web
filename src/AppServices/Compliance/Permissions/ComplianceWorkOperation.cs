using AirWeb.AppServices.CommonInterfaces;
using AirWeb.AppServices.Permissions.Helpers;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Identity.Web;
using System.Security.Claims;

namespace AirWeb.AppServices.Compliance.Permissions;

public class ComplianceWorkOperation :
    OperationAuthorizationRequirement // implements IAuthorizationRequirement
{
    private ComplianceWorkOperation(string name)
    {
        Name = name;
        AllOperations.Add(this);
    }

    public static List<ComplianceWorkOperation> AllOperations { get; } = [];

    public static readonly ComplianceWorkOperation AddComment = new(nameof(AddComment));
    public static readonly ComplianceWorkOperation Close = new(nameof(Close));
    public static readonly ComplianceWorkOperation Delete = new(nameof(Delete));
    public static readonly ComplianceWorkOperation DeleteComment = new(nameof(DeleteComment));
    public static readonly ComplianceWorkOperation Edit = new(nameof(Edit));
    public static readonly ComplianceWorkOperation Reopen = new(nameof(Reopen));
    public static readonly ComplianceWorkOperation Restore = new(nameof(Restore));
    public static readonly ComplianceWorkOperation ViewDeleted = new(nameof(ViewDeleted));

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

    private static bool IsOwner(ClaimsPrincipal user, IHasOwner item) =>
        item.OwnerId.Equals(user.GetNameIdentifierId());
}
