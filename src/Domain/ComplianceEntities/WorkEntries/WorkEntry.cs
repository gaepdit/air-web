using AirWeb.Domain.BaseEntities;
using AirWeb.Domain.Identity;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public abstract class WorkEntry : ClosableEntity<int>, IComplianceEntity
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private protected WorkEntry() { }

    private protected WorkEntry(int? id, FacilityId facilityId, ApplicationUser? user)
    {
        if (id is not null) Id = id.Value;
        SetCreator(user?.Id);
        FacilityId = facilityId;
    }

    // Properties: Basic data

    [StringLength(9)]
    public string FacilityId { get; init; } = null!;

    [StringLength(29)]
    public WorkEntryType WorkEntryType { get; internal init; }

    public ApplicationUser? ResponsibleStaff { get; set; }
    public DateOnly? AcknowledgmentLetterDate { get; set; }

    [StringLength(7000)]
    public string? Notes { get; set; }

    // Comments
    public List<WorkEntryComment> Comments { get; } = [];

    // Calculated properties
    public DateOnly EventDate { get; set; }

    [UsedImplicitly]
    public string EventDateName => WorkEntryType switch
    {
        WorkEntryType.Notification or WorkEntryType.PermitRevocation => "Date Received",
        WorkEntryType.Report or WorkEntryType.AnnualComplianceCertification => "Date Received",
        WorkEntryType.Inspection or WorkEntryType.RmpInspection => "Inspection Date",
        WorkEntryType.SourceTestReview => "Received By Compliance",
        _ => "Error",
    };

    // Business logic
    public const int EarliestWorkEntryYear = 2000;

    public static bool IsComplianceEvent(WorkEntryType workEntryType) =>
        workEntryType is WorkEntryType.AnnualComplianceCertification
            or WorkEntryType.Inspection
            or WorkEntryType.Report
            or WorkEntryType.RmpInspection
            or WorkEntryType.SourceTestReview;
}

// Enums
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WorkEntryType
{
    [Description("Annual Compliance Certification")] AnnualComplianceCertification,
    Inspection,
    Notification,
    [Description("Permit Revocation")] PermitRevocation,
    Report,
    [Description("RMP Inspection")] RmpInspection,
    [Description("Source Test Compliance Review")] SourceTestReview,
}
