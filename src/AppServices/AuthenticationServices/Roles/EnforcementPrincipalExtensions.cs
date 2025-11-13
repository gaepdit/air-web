using AirWeb.Domain.Identity;
using System.Security.Principal;

namespace AirWeb.AppServices.AuthenticationServices.Roles;

public static class EnforcementPrincipalExtensions
{
    extension(IPrincipal principal)
    {
        internal bool IsEnforcementReviewer() =>
            principal.IsInOneOfRoles(RoleName.EnforcementReviewer, RoleName.EnforcementManager);

        internal bool IsEnforcementManager() => principal.IsInRole(RoleName.EnforcementManager);
    }
}
