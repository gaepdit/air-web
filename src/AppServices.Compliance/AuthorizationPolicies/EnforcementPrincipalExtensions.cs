using AirWeb.AppServices.Core.AuthorizationServices;
using AirWeb.Domain.Compliance.AppRoles;
using System.Security.Principal;

namespace AirWeb.AppServices.Compliance.AuthorizationPolicies;

public static class EnforcementPrincipalExtensions
{
    extension(IPrincipal principal)
    {
        internal bool IsEnforcementReviewer() =>
            principal.IsInOneOfRoles(ComplianceRole.EnforcementReviewer, ComplianceRole.EnforcementManager);

        internal bool IsEnforcementManager() => principal.IsInRole(ComplianceRole.EnforcementManager);
    }
}
