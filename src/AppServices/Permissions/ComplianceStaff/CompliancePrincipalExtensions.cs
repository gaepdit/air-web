using AirWeb.AppServices.CommonInterfaces;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.Domain.Identity;
using System.Security.Claims;
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

    internal static bool IsComplianceStaff(this IPrincipal principal) =>
        principal.IsInRoles([RoleName.ComplianceStaff, RoleName.ComplianceManager]);

    internal static bool IsComplianceManager(this IPrincipal principal) =>
        principal.IsInRole(RoleName.ComplianceManager);

    internal static bool IsEnforcementManager(this IPrincipal principal) =>
        principal.IsInRole(RoleName.EnforcementManager);

    internal static bool IsComplianceSiteMaintainer(this IPrincipal principal) =>
        principal.IsInRole(RoleName.ComplianceSiteMaintenance);

    public static bool IsOwnerOrComplianceManager(this ClaimsPrincipal user, IHasOwner item) =>
        user.IsOwner(item) || user.IsComplianceManager();
}
