﻿using AirWeb.Domain.Identity;

namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public class SourceTestReview : ComplianceEvent
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private SourceTestReview() { }

    internal SourceTestReview(int? id, ApplicationUser? user, FacilityId facilityId) : base(id, facilityId)
    {
        WorkEntryType = WorkEntryType.SourceTestReview;
        Close(user);
    }

    // Properties

    public int ReferenceNumber { get; set; }
    public DateOnly ReceivedByCompliance { get; set; }
    public DateOnly? DueDate { get; set; }
    public bool FollowupTaken { get; set; }
}
