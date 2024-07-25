﻿namespace AirWeb.Domain.Entities.WorkEntries;

public class Inspection : BaseInspection
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private Inspection() { }

    internal Inspection(int? id) : base(id) => ComplianceEventType = ComplianceEventType.Inspection;
}
