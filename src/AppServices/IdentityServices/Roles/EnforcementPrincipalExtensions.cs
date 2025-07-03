using AirWeb.Domain.Identity;
using System.Security.Principal;

namespace AirWeb.AppServices.IdentityServices.Roles;

public static class EnforcementPrincipalExtensions
{
    internal static bool IsEnforcementManager(this IPrincipal principal) =>
        principal.IsInRole(RoleName.EnforcementManager);
}
