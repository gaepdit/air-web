using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;
using AirWeb.AppServices.Core.Utilities;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AirWeb.Domain.Core.Data.DataAttributes;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Reports;

public abstract record ReportCommandDto : ComplianceWorkCommandDto, IReportCommandDto
{
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Date Received")]
    [MaxDate]
    public DateOnly ReceivedDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [Display(Name = "Type")]
    public ReportingPeriodType ReportingPeriodType { get; init; } = ReportingPeriodType.Other;

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Start")]
    [MaxDate]
    public DateOnly ReportingPeriodStart { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "End")]
    [MaxDate]
    public DateOnly ReportingPeriodEnd { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [Display(Name = "Reporting Period Comment")]
    public string? ReportingPeriodComment { get; init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Report Due Date")]
    [MaxDate(365)]
    public DateOnly? DueDate { get; init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Date Sent By Facility")]
    [MaxDate]
    public DateOnly? SentDate { get; init; }

    [Display(Name = "Report Is Complete")]
    public bool ReportComplete { get; init; }

    [Display(Name = "Deviations Reported")]
    public bool ReportsDeviations { get; init; }

    [Display(Name = "Enforcement Needed")]
    public bool EnforcementNeeded { get; init; }
}
