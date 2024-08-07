﻿using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public class ComplianceEvent : WorkEntry
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private protected ComplianceEvent() { }

    private protected ComplianceEvent(int? id) : base(id) { }

    // Properties

    [StringLength(29)]
    public ComplianceEventType ComplianceEventType { get; internal init; } = ComplianceEventType.Unknown;

    // FUTURE: Placeholder for managing the EPA data exchange status.
    [JsonIgnore]
    [StringLength(1)]
    public DxStatus EpaDxStatus { get; init; } = DxStatus.P;
}

public enum DxStatus
{
    [Description("Processed")] P,
    [Description("Inserted")] I,
    [Description("Updated")] U,
    [Description("Deleted")] D,
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
}
