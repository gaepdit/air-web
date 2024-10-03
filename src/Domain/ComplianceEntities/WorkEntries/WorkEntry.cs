using AirWeb.Domain.Comments;
using AirWeb.Domain.Identity;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public abstract class WorkEntry : AuditableSoftDeleteEntity<int>, IComplianceEntity
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private protected WorkEntry() { }

    private protected WorkEntry(int? id)
    {
        if (id is not null) Id = id.Value;
    }

    // Properties: Facility

    [MaxLength(9)]
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

    // Properties: Basic data

    [StringLength(29)]
    public WorkEntryType WorkEntryType { get; internal init; }

    public ApplicationUser? ResponsibleStaff { get; set; }
    public DateOnly? AcknowledgmentLetterDate { get; set; }

    [StringLength(7000)]
    public string Notes { get; set; } = string.Empty;

    // Properties: Lists
    public List<WorkEntryComment> Comments { get; } = [];

    // Compliance Event Properties
    public bool IsComplianceEvent { get; internal init; }

    // FUTURE: Placeholder for managing the EPA data exchange status.
    [JsonIgnore]
    [StringLength(11)]
    public DxStatus EpaDxStatus { get; init; } = DxStatus.NotIncluded;

    // Properties: Closure


    public bool IsClosed { get; internal set; }
    public ApplicationUser? ClosedBy { get; internal set; }
    public DateOnly? ClosedDate { get; internal set; }

    internal void Close(ApplicationUser? user)
    {
        IsClosed = true;
        ClosedDate = DateOnly.FromDateTime(DateTime.Now);
        ClosedBy = user;
    }

    internal void Reopen()
    {
        IsClosed = false;
        ClosedDate = null;
        ClosedBy = null;
    }

    // Properties: Deletion
    public ApplicationUser? DeletedBy { get; set; }

    [StringLength(7000)]
    public string? DeleteComments { get; set; }

    internal void Delete(string? comment, ApplicationUser? user)
    {
        SetDeleted(user?.Id);
        DeletedBy = user;
        DeleteComments = comment;
    }

    internal void Undelete()
    {
        SetNotDeleted();
        DeletedBy = null;
        DeleteComments = null;
    }

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

public record WorkEntryComment : Comment
{
    [UsedImplicitly] // Used by ORM.
    private WorkEntryComment() { }

    private WorkEntryComment(Comment c) : base(c) { }
    public WorkEntryComment(Comment c, int workEntryId) : this(c) => WorkEntryId = workEntryId;
    public int WorkEntryId { get; init; }
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
    [Description("Source Test Review")] SourceTestReview,
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DxStatus
{
    [UsedImplicitly] NotIncluded,
    [UsedImplicitly] Processed,
    [UsedImplicitly] Inserted,
    [UsedImplicitly] Updated,
    [UsedImplicitly] Deleted,
}
