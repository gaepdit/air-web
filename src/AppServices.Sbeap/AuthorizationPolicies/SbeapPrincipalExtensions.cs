using AirWeb.Domain.Sbeap.AppRoles;
using System.Security.Principal;

namespace AirWeb.AppServices.Sbeap.AuthorizationPolicies;

public static class SbeapPrincipalExtensions
{
    extension(IPrincipal user)
    {
        internal bool IsSbeapAdmin() => user.IsInRole(SbeapRole.SbeapAdmin);
        public bool IsSbeapStaff() => user.IsInRole(SbeapRole.SbeapStaff) || user.IsSbeapAdmin();
        internal bool IsSbeapSiteMaintainer() => user.IsInRole(SbeapRole.SbeapSiteMaintenance);
        internal bool IsSbeapStaffOrMaintainer() => user.IsSbeapStaff() || user.IsSbeapSiteMaintainer();
    }
}
