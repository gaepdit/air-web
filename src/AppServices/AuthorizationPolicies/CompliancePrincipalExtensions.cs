using AirWeb.AppServices.Core.AuthenticationServices;
using AirWeb.Domain.AppRoles;
using System.Security.Principal;

namespace AirWeb.AppServices.AuthorizationPolicies;

public static class CompliancePrincipalExtensions
{
    // Compliance roles
    extension(IPrincipal principal)
    {
        internal bool IsComplianceManager() =>
            principal.IsInRole(ComplianceRole.ComplianceManager);

        internal bool IsComplianceStaff() =>
            principal.IsInOneOfRoles(ComplianceRole.ComplianceStaff, ComplianceRole.ComplianceManager,
                ComplianceRole.EnforcementManager);
    }
}
