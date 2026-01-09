using AirWeb.Domain.Identity;
using System.Security.Principal;

namespace AirWeb.AppServices.AuthenticationServices.Roles;

public static class CompliancePrincipalExtensions
{
    // Compliance roles
    extension(IPrincipal principal)
    {
        internal bool IsAnyCompliance() =>
            principal.IsInOneOfRoles(RoleName.ComplianceStaff, RoleName.ComplianceManager, RoleName.EnforcementManager);

        internal bool IsComplianceManager() =>
            principal.IsInRole(RoleName.ComplianceManager);

        internal bool IsComplianceSiteMaintainer() =>
            principal.IsInRole(RoleName.ComplianceSiteMaintenance);

        internal bool IsComplianceStaff() =>
            principal.IsInOneOfRoles(RoleName.ComplianceStaff, RoleName.ComplianceManager, RoleName.EnforcementManager);
    }
}
