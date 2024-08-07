﻿namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public class RmpInspection : BaseInspection
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private RmpInspection() { }

    internal RmpInspection(int? id) : base(id) => ComplianceEventType = ComplianceEventType.RmpInspection;
}
