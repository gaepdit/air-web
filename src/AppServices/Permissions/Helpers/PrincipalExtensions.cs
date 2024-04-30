using MyApp.AppServices.Permissions.AppClaims;
using MyApp.Domain.Identity;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Security.Principal;

namespace MyApp.AppServices.Permissions.Helpers;

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
        principal.IsInRole(RoleName.Manager);

    internal static bool IsSiteMaintainer(this IPrincipal principal) =>
        principal.IsInRole(RoleName.SiteMaintenance);

    internal static bool IsStaff(this IPrincipal principal) =>
        principal.IsInRoles([RoleName.Staff, RoleName.Manager]);

    internal static bool IsUserAdmin(this IPrincipal principal) =>
        principal.IsInRole(RoleName.UserAdmin);
}
