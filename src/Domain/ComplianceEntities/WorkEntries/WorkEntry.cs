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

    private protected WorkEntry(int? id, ApplicationUser? user)
    {
        if (id is not null) Id = id.Value;
        SetCreator(user?.Id);
    }

    // Facility properties
    [StringLength(9)]
    public string FacilityId { get; private set; } = string.Empty;

    private Facility _facility = default!;

    [NotMapped]
    public Facility Facility
    {
        get => _facility;
        set
        {
            _facility = value;
            FacilityId = value.Id;
        }
    }

    // Basic data
    [StringLength(29)]
    public WorkEntryType WorkEntryType { get; internal init; }

    public ApplicationUser? ResponsibleStaff { get; set; }
    public DateOnly? AcknowledgmentLetterDate { get; set; }

    [StringLength(7000)]
    public string Notes { get; set; } = string.Empty;

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
