﻿using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;

public record SourceTestReviewViewDto : WorkEntryViewDto
{
    [Display(Name = "Reference Number")]
    public int ReferenceNumber { get; init; }

    [Display(Name = "Date Received")]
    public DateOnly ReceivedByCompliance { get; init; }

    [Display(Name = "Test Due Date")]
    public DateOnly? DueDate { get; init; }

    [Display(Name = "Follow-up Action Taken")]
    public bool FollowupTaken { get; init; }

    public override bool HasPrintout => IsClosed;
    public override string PrintoutUrl => $"https://air.gaepd.org/facility/{Facility.Id}/stack-test/{ReferenceNumber}";
}
