﻿using AirWeb.AppServices.Staff.Dto;

namespace AirWeb.AppServices.Compliance.Fces.SupportingData;

public record AccSummaryDto
{
    [Display(Name = "Tracking #")]
    public int Id { get; init; }

    [Display(Name = "Comments:")]
    public string Notes { get; init; } = null!;

    [Display(Name = "Date Received")]
    public DateOnly ReceivedDate { get; init; }

    [Display(Name = "Reporting Year")]
    public int AccReportingYear { get; init; }

    [Display(Name = "Deviations Reported")]
    public bool ReportsDeviations { get; init; }

    [Display(Name = "Reviewer")]
    public StaffViewDto? ResponsibleStaff { get; init; }
}
