using AirWeb.AppServices.DomainEntities.Facilities;
using AirWeb.Domain.ExternalEntities.SourceTests;
using AirWeb.Domain.ValueObjects;
using GaEpd.AppLibrary.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AirWeb.AppServices.DomainEntities.SourceTests;

public record SourceTestBasicViewDto
{
    // Basic test report info

    [Display(Name = "Reference Number")]
    public int ReferenceNumber { get; init; }

    [JsonIgnore]
    public DocumentType DocumentType { get; init; } = DocumentType.Unassigned;

    [Display(Name = "Document type")]
    public string DocumentTypeName => DocumentType.GetDescription();

    [Display(Name = "Facility")]
    public FacilityViewDto Facility { get; set; } = default!;

    [Display(Name = "Pollutant determined")]
    public string Pollutant { get; init; } = "";

    [Display(Name = "Source tested")]
    public string Source { get; init; } = "";

    [JsonIgnore]
    public ReportType ReportType { get; init; }

    [Display(Name = "Report type")]
    public string ReportTypeName => ReportType.GetDescription();

    [JsonIgnore]
    public string ReportTypeSubject => ReportType switch
    {
        ReportType.MonitorCertification => "Monitor Certification",
        ReportType.PemsDevelopment => "PEMS Development Report Review",
        ReportType.RataCems => "Relative Accuracy Test Audit Report Review",
        ReportType.SourceTest => "Source Test Report Review",
        _ => "N/A",
    };

    [Display(Name = "Applicable requirement")]
    public string ApplicableRequirement { get; init; } = "";

    [Display(Name = "Other information")]
    public string Comments { get; set; } = "";

    [Display(Name = "Report statement")]
    public string ReportStatement { get; init; } = "";

    // Test report routing

    [Display(Name = "Date(s) of test")]
    public DateRange TestDates { get; set; }

    [Display(Name = "Date received by APB")]
    public DateOnly DateReceivedByApb { get; init; }

    [Display(Name = "Report reviewed by")]
    public PersonName ReviewedByStaff { get; set; }

    [Display(Name = "Test witnessed by")]
    public List<PersonName> WitnessedByStaff { get; init; } = [];

    [Display(Name = "Compliance manager")]
    public PersonName ComplianceManager { get; set; }

    [Display(Name = "Testing unit manager")]
    public PersonName TestingUnitManager { get; set; }

    [Display(Name = "Director")]
    public string EpdDirector { get; init; } = string.Empty;
}
