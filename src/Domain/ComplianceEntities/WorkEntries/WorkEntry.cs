using AirWeb.Domain.AuditPoints;
using AirWeb.Domain.BaseEntities;
using AirWeb.Domain.BaseEntities.Interfaces;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public abstract class WorkEntry : ClosableEntity<int>, IFacilityId, INotes
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

    public DateOnly EventDate { get; protected set; }

    [UsedImplicitly]
    public string EventDateName => WorkEntryType switch
    {
        WorkEntryType.Notification or WorkEntryType.PermitRevocation => "Date Received",
        WorkEntryType.Report or WorkEntryType.AnnualComplianceCertification => "Date Received",
        WorkEntryType.Inspection or WorkEntryType.RmpInspection => "Inspection Date",
        WorkEntryType.SourceTestReview => "Received By Compliance",
        _ => "Error",
    };

    // Comments
    public List<WorkEntryComment> Comments { get; } = [];

    // Audit Points
    public List<WorkEntryAuditPoint> AuditPoints { get; } = [];

    // Business logic
    public bool IsComplianceEvent { get; internal init; }

    // Data exchange properties
    public bool IsReportable => IsComplianceEvent && !IsDeleted && WorkEntryType != WorkEntryType.RmpInspection;
}

// Enums
public enum WorkEntryType
{
    [Display(Name = "Annual Compliance Certification")] AnnualComplianceCertification,
    [Display(Name = "Inspection")] Inspection,
    [Display(Name = "Notification")] Notification,
    [Display(Name = "Permit Revocation")] PermitRevocation,
    [Display(Name = "Report")] Report,
    [Display(Name = "RMP Inspection")] RmpInspection,
    [Display(Name = "Source Test Compliance Review")] SourceTestReview,
}
