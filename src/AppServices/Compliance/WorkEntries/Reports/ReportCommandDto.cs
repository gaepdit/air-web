using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;
using AirWeb.AppServices.Utilities;
using AirWeb.Domain.ComplianceEntities.WorkEntries;

namespace AirWeb.AppServices.Compliance.WorkEntries.Reports;

public abstract record ReportCommandDto : WorkEntryCommandDto, IReportCommandDto
{
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Date Received")]
    public DateOnly ReceivedDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [Display(Name = "Type")]
    public ReportingPeriodType ReportingPeriodType { get; init; } = ReportingPeriodType.Other;

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Start")]
    public DateOnly ReportingPeriodStart { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "End")]
    public DateOnly ReportingPeriodEnd { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [Display(Name = "Reporting Period Comment")]
    public string? ReportingPeriodComment { get; init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Report Due Date")]
    public DateOnly? DueDate { get; init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Date Sent By Facility")]
    public DateOnly? SentDate { get; init; }

    [Display(Name = "Report Is Complete")]
    public bool ReportComplete { get; init; }

    [Display(Name = "Deviations Reported")]
    public bool ReportsDeviations { get; init; }

    [Display(Name = "Enforcement Needed")]
    public bool EnforcementNeeded { get; init; }
}
