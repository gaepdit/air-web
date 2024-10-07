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
        Pollutant = report.Pollutant;
        Source = report.Source;
        ReportType = report.ReportType;
        ApplicableRequirement = report.ApplicableRequirement;
        ReportClosed = report.ReportClosed;
        TestDates = report.TestDates;
        DateReceivedByApb = report.DateReceivedByApb;
        ReviewedByStaff = report.ReviewedByStaff;
        TestingUnitManager = report.TestingUnitManager;
    }

    [Display(Name = "Reference Number")]
    public int ReferenceNumber { get; init; }

    public DocumentType DocumentType { get; init; } = DocumentType.Unassigned;

    [Display(Name = "Document type")]
    public string DocumentTypeName => DocumentType.GetDescription();

    [Display(Name = "Pollutant determined")]
    public string Pollutant { get; init; } = "";

    [Display(Name = "Source tested")]
    public string Source { get; init; } = "";

    public ReportType ReportType { get; init; }

    [Display(Name = "Report type")]
    public string ReportTypeName => ReportType.GetDescription();

    [Display(Name = "Applicable requirement")]
    public string ApplicableRequirement { get; init; } = "";

    public bool ReportClosed { get; init; }

    [Display(Name = "Date(s) of test")]
    public DateRange TestDates { get; set; }

    [Display(Name = "Date received by APB")]
    public DateTime DateReceivedByApb { get; init; }

    [Display(Name = "Report reviewed by")]
    public PersonName ReviewedByStaff { get; set; }

    [Display(Name = "Testing unit manager")]
    public PersonName TestingUnitManager { get; set; }
}
