﻿using AirWeb.Domain.Identity;

namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public class PermitRevocation : WorkEntry
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private PermitRevocation() { }

    internal PermitRevocation(int? id, ApplicationUser? user) : base(id, user)
    {
        WorkEntryType = WorkEntryType.PermitRevocation;
    }

    // Properties

    public DateOnly ReceivedDate { get; set; }
    public DateOnly PermitRevocationDate { get; set; }
    public DateOnly? PhysicalShutdownDate { get; set; }
    public bool FollowupTaken { get; set; }
}
