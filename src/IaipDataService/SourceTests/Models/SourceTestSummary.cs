using IaipDataService.Facilities;
using IaipDataService.Structs;
using IaipDataService.Utilities;
using System.ComponentModel.DataAnnotations;

namespace IaipDataService.SourceTests.Models;

public record SourceTestSummary
{
    public SourceTestSummary() { }

    public SourceTestSummary(BaseSourceTestReport report)
    {
        ReferenceNumber = report.ReferenceNumber;
        DocumentType = report.DocumentType;
        Facility = report.Facility;
        Pollutant = report.Pollutant;
        Source = report.Source;
        ReportType = report.ReportType;
        ApplicableRequirement = report.ApplicableRequirement;
        ReportClosed = report.ReportClosed;
        TestDates = report.TestDates;
        DateReceivedByApb = report.DateReceivedByApb;
        ReviewedByStaff = report.ReviewedByStaff;
    }

    public int ReferenceNumber { get; init; }

    private ReportType ReportType { get; init; }

    private DocumentType DocumentType { get; init; }

    [Display(Name = "Source test type")]
    public string TestType => ReportType is ReportType.SourceTest or ReportType.NA
        ? DocumentType.GetDescription()
        : ReportType.GetDescription();

    public FacilitySummary? Facility { get; set; }

    [Display(Name = "Source tested")]
    public string Source { get; init; } = null!;

    [Display(Name = "Pollutant determined")]
    public string Pollutant { get; init; } = null!;

    [Display(Name = "Applicable requirement")]
    public string ApplicableRequirement { get; init; } = null!;

    [Display(Name = "Status")]
    public bool ReportClosed { get; init; }

    // FUTURE: Change to DateOnly when this Dapper issue is fixed and DateOnly is supported:
    // https://github.com/DapperLib/Dapper/issues/2072
    [Display(Name = "Date received by APB")]
    public DateTime DateReceivedByApb { get; init; }

    [Display(Name = "Test dates")]
    public DateRange TestDates { get; set; }

    [Display(Name = "Report reviewed by")]
    public PersonName ReviewedByStaff { get; set; }

    public bool HasPrintout => ReportClosed;
}
