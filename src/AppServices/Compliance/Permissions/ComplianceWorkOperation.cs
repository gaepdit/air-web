using AirWeb.AppServices.CommonInterfaces;
using AirWeb.AppServices.Permissions.Helpers;
using Microsoft.AspNetCore.Authorization.Infrastructure;
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

    public static readonly ComplianceWorkOperation Close = new(nameof(Close));
    public static readonly ComplianceWorkOperation Reopen = new(nameof(Reopen));
    public static readonly ComplianceWorkOperation Edit = new(nameof(Edit));
    public static readonly ComplianceWorkOperation ViewDeleted = new(nameof(ViewDeleted));
    public static readonly ComplianceWorkOperation Delete = new(nameof(Delete));
    public static readonly ComplianceWorkOperation Restore = new(nameof(Restore));

    // Operation requirement handler helpers
    internal static bool CanClose(ClaimsPrincipal user, ICloseableDeletableItem item) =>
        CanCloseOrReopen(user, item) && !item.IsClosed;

    private static bool CanCloseOrReopen(ClaimsPrincipal user, IDeletableItem item) =>
        user.IsComplianceStaff() && !item.IsDeleted;

    internal static bool CanDelete(ClaimsPrincipal user, IDeletableItem item) =>
        CanManageDeletions(user) && !item.IsDeleted;

    internal static bool CanEdit(ClaimsPrincipal user, ICloseableDeletableItem item) =>
        user.IsComplianceStaff() && item is { IsClosed: false, IsDeleted: false };

    internal static bool CanEdit(ClaimsPrincipal user, IDeletableItem item) =>
        user.IsComplianceStaff() && !item.IsDeleted;

    internal static bool CanManageDeletions(ClaimsPrincipal user) => user.IsComplianceManager();

    internal static bool CanReopen(ClaimsPrincipal user, ICloseableDeletableItem item) =>
        CanCloseOrReopen(user, item) && item.IsClosed;

    internal static bool CanRestore(ClaimsPrincipal user, IDeletableItem item) =>
        CanManageDeletions(user) && item.IsDeleted;
}
