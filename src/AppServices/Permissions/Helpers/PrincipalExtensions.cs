using AirWeb.AppServices.Permissions.AppClaims;
using AirWeb.Domain.Identity;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Security.Principal;

namespace AirWeb.AppServices.Permissions.Helpers;

public static class PrincipalExtensions
{
    public static string? GetUserIdValue(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.NameIdentifier);

    public static bool HasRealClaim(this ClaimsPrincipal principal, string type, [NotNullWhen(true)] string? value) =>
        value is not null && principal.HasClaim(type, value);

    internal static bool IsActive(this ClaimsPrincipal principal) =>
        principal.HasClaim(AppClaimTypes.ActiveUser, true.ToString());

    private static bool IsInRoles(this IPrincipal principal, IEnumerable<string> roles) =>
        roles.Any(principal.IsInRole);

    internal static bool IsManager(this IPrincipal principal) =>
        principal.IsInRole(RoleName.ComplianceManager);

    internal static bool IsSiteMaintainer(this IPrincipal principal) =>
        principal.IsInRole(RoleName.ComplianceSiteMaintenance);

    internal static bool IsStaff(this IPrincipal principal) =>
        principal.IsInRoles([RoleName.ComplianceStaff, RoleName.ComplianceManager]);

    internal static bool IsUserAdmin(this IPrincipal principal) =>
        principal.IsInRole(RoleName.AppUserAdmin);
}
