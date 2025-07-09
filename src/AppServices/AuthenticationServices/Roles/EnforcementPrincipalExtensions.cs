using AirWeb.Domain.Identity;
using System.Security.Principal;

namespace AirWeb.AppServices.AuthenticationServices.Roles;

public static class EnforcementPrincipalExtensions
{
    internal static bool IsEnforcementManager(this IPrincipal principal) =>
        principal.IsInRole(RoleName.EnforcementManager);
}
