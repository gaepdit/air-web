using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.EnforcementEntities.CaseFiles;

namespace AirWeb.Domain.DataExchange;

public static class Identifiers
{
    // CONCAT('GA000000', SUBSTRING(STRAIRSNUMBER, 3, 10)) AS AirFacilityId,
    public static string EpaFacilityId(this FacilityId facilityId) => $"GA00000013{facilityId.Id}";

    public static string EpaFacilityId(this Facility facility) => facility.Id.EpaFacilityId();

    // CONCAT('GA000A0000', SUBSTRING(STRAIRSNUMBER, 3, 10), dbo.LPAD(STRAFSACTIONNUMBER, 5, '0')) AS ComplianceMonitoringId,
    public static string EpaActionId(this FacilityId facilityId, ushort? actionNumber) =>
        actionNumber is null ? string.Empty : $"GA000A000013{facilityId.Id}{actionNumber:D5}";

    public static string EpaActionId(string facilityId, ushort? actionNumber) =>
        actionNumber is null || !FacilityId.IsValidFormat(facilityId)
            ? string.Empty
            : $"GA000A000013{facilityId}{actionNumber:D5}";

    public static string EpaActionId(this IDataExchange dx, FacilityId facilityId) =>
        facilityId.EpaActionId(dx.ActionNumber);

    public static string EpaActionId(this CaseFile caseFile) => EpaActionId(caseFile.FacilityId, caseFile.ActionNumber);

    public static string EpaActionId(this ComplianceEvent complianceEvent) =>
        EpaActionId(complianceEvent.FacilityId, complianceEvent.ActionNumber);
}
