using AirWeb.Domain.Identity;
using System.Security.Principal;

namespace AirWeb.AppServices.AuthenticationServices.Roles;

public static class CompliancePrincipalExtensions
{
    // Compliance roles
    internal static bool IsAnyCompliance(this IPrincipal principal) =>
        principal.IsInOneOfRoles([
            RoleName.ComplianceStaff, RoleName.ComplianceManager, RoleName.ComplianceSiteMaintenance,
        ]);

    internal static bool IsComplianceManager(this IPrincipal principal) =>
        principal.IsInRole(RoleName.ComplianceManager);

    internal static bool IsComplianceSiteMaintainer(this IPrincipal principal) =>
        principal.IsInRole(RoleName.ComplianceSiteMaintenance);

    internal static bool IsComplianceStaff(this IPrincipal principal) =>
        principal.IsInOneOfRoles([RoleName.ComplianceStaff, RoleName.ComplianceManager, RoleName.EnforcementManager]);
}
