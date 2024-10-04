using IaipDataService.SourceTests.Models.TestRun;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IaipDataService.SourceTests.Models;

public record SourceTestReportRata : BaseSourceTestReport
{
    [Display(Name = "Applicable standard")]
    public string ApplicableStandard { get; set; } = "";

    [Display(Name = "Diluent monitored")]
    public string Diluent { get; set; } = "";

    // Test run data

    [Display(Name = "Units")]
    public string Units { get; set; } = "";

    [Display(Name = "Test data")]
    public List<RataTestRun> TestRuns { get; set; } = [];

    [Display(Name = "Accuracy choice")] // STRACCURACYCHOICE
    [JsonIgnore]
    public string RelativeAccuracyCode { get; set; } = "";

    [Display(Name = "Relative accuracy")] // STRRELATIVEACCURACYPERCENT
    public string RelativeAccuracyPercent { get; set; } = "";

    public string RelativeAccuracyLabel =>
        RelativeAccuracyCode switch
        {
            "RefMethod" => "% (of the Reference Method)",
            "AppStandard" => "% (of the Applicable Standard)",
            "Diluent" => "% (Diluent)",
            _ => "%",
        };

    [Display(Name = "Relative accuracy required")] // STRACCURACYREQUIREDPERCENT
    public string RelativeAccuracyRequiredPercent { get; set; } = "";

    [Display(Name = "Relative accuracy required statement")] // STRACCURACYREQUIREDSTATEMENT
    public string RelativeAccuracyRequiredLabel { get; set; } = "";

    [Display(Name = "Result")]
    public string ComplianceStatus { get; set; } = "";

    #region Confidential info handling

    public override SourceTestReportRata RedactedStackTestReport() =>
        RedactedBaseStackTestReport<SourceTestReportRata>() with
        {
            TestRuns = BaseTestRun.RedactedTestRuns(TestRuns),
            ApplicableStandard = CheckConfidential(ApplicableStandard, nameof(ApplicableStandard)),
            Units = CheckConfidential(Units, nameof(Units)),
            RelativeAccuracyPercent = CheckConfidential(RelativeAccuracyPercent, nameof(RelativeAccuracyPercent)),

            // Confidentiality of both of the following are based on the same parameter (RataStatement):
            RelativeAccuracyRequiredPercent =
            CheckConfidential(RelativeAccuracyRequiredPercent, nameof(RelativeAccuracyRequiredLabel)),
            RelativeAccuracyRequiredLabel =
            CheckConfidential(RelativeAccuracyRequiredLabel, nameof(RelativeAccuracyRequiredLabel)),
        };

    public override void ParseConfidentialParameters()
    {
        ConfidentialParameters = new HashSet<string>();
        TestRuns = BaseTestRun.ParsedTestRuns(TestRuns);

        if (ConfidentialParametersCode == "" || ConfidentialParametersCode[0] == '0') return;
        ParseBaseConfidentialParameters();

        AddIfConfidential(26, nameof(ApplicableStandard));
        AddIfConfidential(53, nameof(Units));
        AddIfConfidential(54, nameof(RelativeAccuracyPercent));
        AddIfConfidential(55, nameof(RelativeAccuracyRequiredLabel));
        AddIfConfidential(56, nameof(Comments));
    }

    #endregion
}
