using AirWeb.Core.AppRoles;
using System.Security.Principal;

namespace AirWeb.AppServices.Core.AuthenticationServices;

public static class PrincipalExtensions
{
    extension(IPrincipal principal)
    {
        public bool IsInOneOfRoles(params IEnumerable<string> roles) => roles.Any(principal.IsInRole);

        public bool IsStaff() => principal.IsInOneOfRoles(AppRole.StaffRoles);
        internal bool IsSiteMaintainer() => principal.IsInOneOfRoles(AppRole.SiteMaintenanceRoles);
        internal bool IsUserAdmin() => principal.IsInOneOfRoles(AppRole.UserAdminRoles);
    }
}
