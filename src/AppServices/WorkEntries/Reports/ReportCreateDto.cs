﻿using AirWeb.AppServices.WorkEntries.BaseWorkEntryDto;
using AirWeb.Domain.Entities.WorkEntries;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.WorkEntries.Reports;

public record ReportCreateDto : BaseWorkEntryCreateDto
{
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    [Display(Name = "Date Received")]
    public DateOnly ReceivedDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [Display(Name = "Report Type")]
    public ReportingPeriodType ReportingPeriodType { get; init; } = ReportingPeriodType.Other;

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    [Display(Name = "Start")]
    public DateOnly ReportingPeriodStart { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    [Display(Name = "End")]
    public DateOnly ReportingPeriodEnd { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [Display(Name = "Reporting Period Comment")]
    public string? ReportingPeriodComment { get; init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    [Display(Name = "Report Due Date")]
    public DateOnly? DueDate { get; init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    [Display(Name = "Date Sent by Facility")]
    public DateOnly? SentDate { get; init; }

    [Display(Name = "Report is complete")]
    public bool ReportComplete { get; init; }

    [Display(Name = "Deviations reported")]
    public bool ReportsDeviations { get; init; }

    [Display(Name = "Enforcement needed")]
    public bool EnforcementNeeded { get; init; }

    [Display(Name = "Follow-up Action Taken")]
    public bool FollowupTaken { get; init; }
}
