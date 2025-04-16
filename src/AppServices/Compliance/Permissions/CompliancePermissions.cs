using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;
using AirWeb.AppServices.Permissions.ComplianceStaff;
using System.Security.Claims;

namespace AirWeb.AppServices.Compliance.Permissions;

public static class CompliancePermissions
{
    public static bool CanBeginEnforcement(this ClaimsPrincipal user, IWorkEntrySummaryDto item) =>
        item is { IsComplianceEvent: true, IsDeleted: false } && user.IsComplianceStaff();
}
