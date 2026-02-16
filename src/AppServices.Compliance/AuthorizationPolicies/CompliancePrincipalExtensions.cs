using AirWeb.AppServices.Core.AuthorizationServices;
using AirWeb.Domain.Compliance.AppRoles;
using System.Security.Principal;

namespace AirWeb.AppServices.Compliance.AuthorizationPolicies;

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
