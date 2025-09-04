using AirWeb.AppServices.DtoInterfaces;
using AirWeb.Domain.Identity;
using Microsoft.Identity.Web;
using System.Security.Claims;
using System.Security.Principal;

namespace AirWeb.AppServices.AuthenticationServices.Roles;

public static class PrincipalExtensions
{
    internal static bool IsInOneOfRoles(this IPrincipal principal, IEnumerable<string> roles) =>
        roles.Any(principal.IsInRole);

    // General staff
    internal static bool IsStaff(this IPrincipal principal) =>
        principal.IsInRole(RoleName.GeneralStaff) || principal.IsAnyCompliance();

    // Admin roles
    internal static bool IsSiteMaintainer(this IPrincipal principal) =>
        principal.IsInOneOfRoles([RoleName.SiteMaintenance, RoleName.ComplianceSiteMaintenance]);

    internal static bool IsUserAdmin(this IPrincipal principal) =>
        principal.IsInRole(RoleName.AppUserAdmin);

    public static bool IsOwner(this ClaimsPrincipal user, IHasOwner item) =>
        item.OwnerId.Equals(user.GetNameIdentifierId());
}
