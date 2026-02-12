using AirWeb.AppServices.DtoInterfaces;
using AirWeb.Domain.Roles;
using Microsoft.Identity.Web;
using System.Security.Claims;
using System.Security.Principal;

namespace AirWeb.AppServices.AuthenticationServices.Roles;

public static class PrincipalExtensions
{
    extension(IPrincipal principal)
    {
        internal bool IsInOneOfRoles(params IEnumerable<string> roles) => roles.Any(principal.IsInRole);

        // General staff
        internal bool IsStaff() => principal.IsInRole(RoleName.GeneralStaff) || principal.IsAnyCompliance();

        // Admin roles
        internal bool IsSiteMaintainer() =>
            principal.IsInOneOfRoles(RoleName.SiteMaintenance, RoleName.ComplianceSiteMaintenance);

        internal bool IsUserAdmin() => principal.IsInRole(RoleName.AppUserAdmin);
    }

    public static bool IsOwner(this ClaimsPrincipal user, IHasOwner item) =>
        item.OwnerId.Equals(user.GetNameIdentifierId());
}
