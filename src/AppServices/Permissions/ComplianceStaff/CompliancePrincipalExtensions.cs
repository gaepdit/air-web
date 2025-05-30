using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.Domain.Identity;
using System.Security.Principal;

namespace AirWeb.AppServices.Permissions.ComplianceStaff;

public static class CompliancePrincipalExtensions
{
    // Compliance roles
    internal static bool IsAnyCompliance(this IPrincipal principal) =>
        principal.IsInRoles([
            RoleName.ComplianceStaff, RoleName.ComplianceManager,
            RoleName.EnforcementManager, RoleName.ComplianceSiteMaintenance,
        ]);

    internal static bool IsComplianceManager(this IPrincipal principal) =>
        principal.IsInRole(RoleName.ComplianceManager);

    internal static bool IsComplianceSiteMaintainer(this IPrincipal principal) =>
        principal.IsInRole(RoleName.ComplianceSiteMaintenance);

    internal static bool IsComplianceStaff(this IPrincipal principal) =>
        principal.IsInRoles([RoleName.ComplianceStaff, RoleName.ComplianceManager, RoleName.EnforcementManager]);

    internal static bool IsEnforcementManager(this IPrincipal principal) =>
        principal.IsInRole(RoleName.EnforcementManager);
}
