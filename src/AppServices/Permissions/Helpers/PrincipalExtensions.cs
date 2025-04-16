using AirWeb.AppServices.CommonInterfaces;
using AirWeb.AppServices.Permissions.AppClaims;
using AirWeb.AppServices.Permissions.ComplianceStaff;
using AirWeb.Domain.Identity;
using Microsoft.Identity.Web;
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

    internal static bool IsInRoles(this IPrincipal principal, IEnumerable<string> roles) =>
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

    public static bool IsOwner(this ClaimsPrincipal user, IHasOwner item) =>
        item.OwnerId.Equals(user.GetNameIdentifierId());
}
