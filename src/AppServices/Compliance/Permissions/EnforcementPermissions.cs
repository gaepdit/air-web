using AirWeb.AppServices.AuthenticationServices.Roles;
using AirWeb.AppServices.Compliance.ComplianceMonitoring.ComplianceWorkDto.Query;
using System.Security.Claims;

namespace AirWeb.AppServices.Compliance.Permissions;

public static class EnforcementPermissions
{
    public static bool CanBeginEnforcement(this ClaimsPrincipal user, IComplianceWorkSummaryDto item) =>
        item is { IsComplianceEvent: true, IsDeleted: false } && user.IsComplianceStaff();
}
