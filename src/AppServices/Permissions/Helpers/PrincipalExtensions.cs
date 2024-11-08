using AirWeb.AppServices.Permissions.AppClaims;
using AirWeb.Domain.Identity;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Security.Principal;

namespace AirWeb.AppServices.Permissions.Helpers;

public static class PrincipalExtensions
{
    public static string? GetEmail(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.Email);

    public static string GetGivenName(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.GivenName) ?? string.Empty;

    public static string GetFamilyName(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.Surname) ?? string.Empty;

    public static bool HasRealClaim(this ClaimsPrincipal principal, string type, [NotNullWhen(true)] string? value) =>
        value is not null && principal.HasClaim(type, value);

    internal static bool IsActive(this ClaimsPrincipal principal) =>
        principal.HasClaim(AppClaimTypes.ActiveUser, true.ToString());

    private static bool IsInRoles(this IPrincipal principal, IEnumerable<string> roles) =>
        roles.Any(principal.IsInRole);

    // General staff
    internal static bool IsStaff(this IPrincipal principal) =>
        principal.IsInRoles([RoleName.GeneralStaff, RoleName.SiteMaintenance, RoleName.AppUserAdmin]) ||
        principal.IsAnyCompliance();

    // Admin roles
    internal static bool IsSiteMaintainer(this IPrincipal principal) =>
        principal.IsInRole(RoleName.SiteMaintenance);

    internal static bool IsUserAdmin(this IPrincipal principal) =>
        principal.IsInRole(RoleName.AppUserAdmin);

    // Compliance roles
    private static bool IsAnyCompliance(this IPrincipal principal) =>
        principal.IsInRoles([RoleName.ComplianceStaff, RoleName.ComplianceManager, RoleName.ComplianceSiteMaintenance]);

    internal static bool IsComplianceManager(this IPrincipal principal) =>
        principal.IsInRole(RoleName.ComplianceManager);

    internal static bool IsComplianceSiteMaintainer(this IPrincipal principal) =>
        principal.IsInRole(RoleName.ComplianceSiteMaintenance);

    internal static bool IsComplianceStaff(this IPrincipal principal) =>
        principal.IsInRoles([RoleName.ComplianceStaff, RoleName.ComplianceManager]);
}
