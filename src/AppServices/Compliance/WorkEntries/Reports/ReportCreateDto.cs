﻿using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.WorkEntries.Reports;

public record ReportCreateDto : WorkEntryCreateDto, IReportCommandDto
{
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    [Display(Name = "Date Received")]
    public DateOnly ReceivedDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [Display(Name = "Type")]
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
}
