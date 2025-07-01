using IaipDataService.Facilities;
using IaipDataService.Structs;
using IaipDataService.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IaipDataService.SourceTests.Models;

public abstract record BaseSourceTestReport
{
    // Basic test report info

    [Display(Name = "Reference Number")]
    public int ReferenceNumber { get; init; }

    [JsonIgnore]
    public DocumentType DocumentType { get; init; } = DocumentType.Unassigned;

    [Display(Name = "Document type")]
    public string DocumentTypeName => DocumentType.GetDisplayName();

    public FacilitySummary? Facility { get; set; }

    [Display(Name = "Pollutant determined")]
    public string Pollutant { get; init; } = null!;

    [Display(Name = "Source tested")]
    public string Source { get; init; } = null!;

    [JsonIgnore]
    public ReportType ReportType { get; init; }

    [Display(Name = "Report type")]
    public string ReportTypeName => ReportType.GetDisplayName();

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
    public string ApplicableRequirement { get; init; } = null!;

    [Display(Name = "Other information")]
    public string Comments { get; set; } = null!;

    public string ReportStatement { get; init; } = null!;
    public bool ReportClosed { get; init; }

    // Test report routing

    [Display(Name = "Test dates")]
    public DateRange TestDates { get; set; }

    // FUTURE: Change to DateOnly when this Dapper issue is fixed and DateOnly is supported:
    // https://github.com/DapperLib/Dapper/issues/2072
    [Display(Name = "Date received by APB")]
    public DateTime DateReceivedByApb { get; init; }

    [Display(Name = "Report reviewed by")]
    public PersonName ReviewedByStaff { get; set; }

    [Display(Name = "Test witnessed by")]
    public List<PersonName> WitnessedByStaff { get; init; } = [];

    [Display(Name = "Compliance manager")]
    public PersonName ComplianceManager { get; set; }

    [Display(Name = "Testing unit manager")]
    public PersonName TestingUnitManager { get; set; }

    [Display(Name = "Director")]
    public string EpdDirector { get; init; } = null!;

    #region Confidential info handling

    internal const string ConfidentialInfoPlaceholder = "--Conf--";

    // For documentation of the ConfidentialParametersCode string, see:
    // https://github.com/gaepdit/iaip/blob/main/IAIP/ISMP/ISMPConfidentialData.vb

    [JsonIgnore]
    public string ConfidentialParametersCode { protected get; init; } = null!;

    public ICollection<string> ConfidentialParameters { get; protected set; } = [];

    public abstract BaseSourceTestReport RedactedStackTestReport();

    protected T RedactedBaseStackTestReport<T>() where T : BaseSourceTestReport =>
        (T)this with
        {
            Pollutant = CheckConfidential(Pollutant, nameof(Pollutant)),
            Source = CheckConfidential(Source, nameof(Source)),
            Comments = CheckConfidential(Comments, nameof(Comments)),
            TestDates = CheckConfidential(TestDates, nameof(TestDates)),
            DateReceivedByApb = CheckConfidential(DateReceivedByApb, nameof(DateReceivedByApb)),
            ReviewedByStaff = CheckConfidential(ReviewedByStaff, nameof(ReviewedByStaff)),
            WitnessedByStaff = CheckConfidential(WitnessedByStaff, nameof(WitnessedByStaff)),
            ComplianceManager = CheckConfidential(ComplianceManager, nameof(ComplianceManager)),
            TestingUnitManager = CheckConfidential(TestingUnitManager, nameof(TestingUnitManager)),
        };

    protected string CheckConfidential(string input, string parameter) =>
        ConfidentialParameters.Contains(parameter)
            ? ConfidentialInfoPlaceholder
            : input;

    protected DateTime CheckConfidential(DateTime input, string parameter) =>
        ConfidentialParameters.Contains(parameter)
            ? default
            : input;

    protected DateOnly CheckConfidential(DateOnly input, string parameter) =>
        ConfidentialParameters.Contains(parameter)
            ? default
            : input;

    protected DateRange CheckConfidential(DateRange input, string parameter) =>
        ConfidentialParameters.Contains(parameter)
            ? new DateRange(default, null)
            : input;

    protected PersonName CheckConfidential(PersonName input, string parameter) =>
        ConfidentialParameters.Contains(parameter)
            ? new PersonName("", ConfidentialInfoPlaceholder)
            : input;

    protected List<PersonName> CheckConfidential(List<PersonName> input, string parameter) =>
        ConfidentialParameters.Contains(parameter)
            ? [new PersonName("", ConfidentialInfoPlaceholder)]
            : input;

    protected ValueWithUnits CheckConfidential(ValueWithUnits input, string parameter) =>
        ConfidentialParameters.Contains(parameter)
            ? input with { Value = ConfidentialInfoPlaceholder }
            : input;

    protected List<ValueWithUnits> CheckConfidential(List<ValueWithUnits> input, string parameter) =>
        ConfidentialParameters.Contains(parameter)
            ? [new ValueWithUnits(ConfidentialInfoPlaceholder, "")]
            : input;

    public abstract void ParseConfidentialParameters();

    protected void ParseBaseConfidentialParameters()
    {
        AddIfConfidential(15, nameof(Pollutant));
    }

    // Uses "ONE"-based position to better correlate with IAIP code
    protected void AddIfConfidential(int position, string parameter)
    {
        if (ConfidentialParametersCode.Length < position) return;
        if (ConfidentialParametersCode[position - 1] == '1') ConfidentialParameters.Add(parameter);
    }

    #endregion
}
