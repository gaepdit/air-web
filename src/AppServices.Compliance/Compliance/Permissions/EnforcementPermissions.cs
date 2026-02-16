using AirWeb.AppServices.Compliance.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Query;
using System.Security.Claims;

namespace AirWeb.AppServices.Compliance.Compliance.Permissions;

public static class EnforcementPermissions
{
    public static bool CanBeginEnforcement(this ClaimsPrincipal user, IComplianceWorkSummaryDto item) =>
        item is { IsComplianceEvent: true, IsDeleted: false } && user.IsComplianceStaff();
}
