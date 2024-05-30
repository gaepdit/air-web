using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.Entities.WorkEntries;

public abstract class BaseComplianceEvent(int? id) : BaseWorkEntry(id)
{
    [StringLength(29)]
    public ComplianceEventType ComplianceEventType { get; internal init; } = ComplianceEventType.Unknown;

    [StringLength(1)]
    public DxStatus EpaDxStatus { get; init; } = DxStatus.P;

    public enum DxStatus
    {
        [Description("Processed")] P,
        [Description("Inserted")] I,
        [Description("Updated")] U,
        [Description("Deleted")] D,
    }
}

// Enums

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ComplianceEventType
{
    Unknown = 0,
    Report = 1,
    Inspection = 2,
    [Description("Source Test Review")] SourceTestReview = 3,
    [Description("Annual Compliance Certification")] AnnualComplianceCertification = 4,
    [Description("RMP Inspection")] RmpInspection = 7,

    // [Obsolete("Non-compliance event work entry")] Notification = 5,
    // [Obsolete("Legacy entry number in legacy database")] AccDuplicate = 6,
    // [Obsolete("Non-compliance event work entry")] PermitRevocation = 8,
}
