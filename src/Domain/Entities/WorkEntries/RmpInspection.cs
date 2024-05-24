﻿namespace AirWeb.Domain.Entities.WorkEntries;

public class RmpInspection : BaseComplianceEvent
{
    internal RmpInspection(int? id) : base(id)
    {
        WorkEntryType = WorkEntryType.ComplianceEvent;
        ComplianceEventType = ComplianceEventType.RmpInspection;
    }

    public InspectionReason? InspectionReason { get; set; }
    public ComplianceStatus ComplianceStatus { get; set; }
}
