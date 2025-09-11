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
        ComplianceStatus = report.ComplianceStatus;
        ReportClosed = report.ReportClosed;
        DateTestReviewComplete = report.DateTestReviewComplete;
        IaipComplianceAssignment = report.IaipComplianceAssignment;
        TestDates = report.TestDates;
        DateReceivedByApb = report.DateReceivedByApb;
        ReviewedByStaff = report.ReviewedByStaff;
    }

    [Display(Name = "Reference Number")]
    public int ReferenceNumber { get; init; }

    private ReportType ReportType { get; init; }

    private DocumentType DocumentType { get; init; }

    [Display(Name = "Source Test Type")]
    public string TestType => ReportType is ReportType.SourceTest or ReportType.NA
        ? DocumentType.GetDisplayName()
        : ReportType.GetDisplayName();

    public FacilitySummary? Facility { get; set; }

    [Display(Name = "Source Tested")]
    public string Source { get; init; } = null!;

    [Display(Name = "Pollutant Determined")]
    public string Pollutant { get; init; } = null!;

    [Display(Name = "Applicable Requirement")]
    public string ApplicableRequirement { get; init; } = null!;

    [Display(Name = "Compliance Determination")]
    public string ComplianceStatus { get; init; } = null!;

    [Display(Name = "Test Report Status")]
    public bool ReportClosed { get; init; }

    public string? IaipComplianceAssignment { get; init; }

    // FUTURE: Change DateTime to DateOnly when this Dapper issue is fixed and DateOnly is supported:
    // https://github.com/DapperLib/Dapper/issues/2072
    [Display(Name = "Date Received By APB")]
    public DateTime DateReceivedByApb { get; init; }

    [Display(Name = "Date Test Review Complete")]
    public DateTime? DateTestReviewComplete { get; init; }

    [Display(Name = "Test Dates")]
    public DateRange TestDates { get; set; }

    [Display(Name = "Report Reviewed By")]
    public PersonName ReviewedByStaff { get; set; }

    public bool HasPrintout => ReportClosed;
}
