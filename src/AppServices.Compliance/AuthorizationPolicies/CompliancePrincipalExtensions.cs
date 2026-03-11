using AirWeb.AppServices.Core.AuthorizationServices;
using AirWeb.Domain.Compliance.AppRoles;
using System.Security.Principal;

namespace AirWeb.AppServices.Compliance.AuthorizationPolicies;

public static class CompliancePrincipalExtensions
{
    // Compliance roles
    internal static bool IsComplianceStaff(this IPrincipal principal) =>
        principal.IsInOneOfRoles(ComplianceRole.ComplianceStaff, ComplianceRole.ComplianceManager,
            ComplianceRole.EnforcementManager);
}
