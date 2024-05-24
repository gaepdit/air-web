namespace AirWeb.Domain.Entities.WorkEntries;

public class RmpInspection : BaseWorkEntry
{
    internal RmpInspection(int? id) : base(id) => WorkEntryType = WorkEntryType.RmpInspection;

    public InspectionReason? InspectionReason { get; init; }
    public ComplianceStatus ComplianceStatus { get; init; }
}
