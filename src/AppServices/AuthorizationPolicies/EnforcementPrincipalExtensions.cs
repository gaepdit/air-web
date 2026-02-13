using AirWeb.AppServices.Core.AuthenticationServices;
using AirWeb.Domain.AppRoles;
using System.Security.Principal;

namespace AirWeb.AppServices.AuthorizationPolicies;

public static class EnforcementPrincipalExtensions
{
    extension(IPrincipal principal)
    {
        internal bool IsEnforcementReviewer() =>
            principal.IsInOneOfRoles(ComplianceRole.EnforcementReviewer, ComplianceRole.EnforcementManager);

        internal bool IsEnforcementManager() => principal.IsInRole(ComplianceRole.EnforcementManager);
    }
}
