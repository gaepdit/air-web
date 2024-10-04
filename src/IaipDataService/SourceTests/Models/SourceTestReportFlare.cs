using IaipDataService.SourceTests.Models.TestRun;
using IaipDataService.Structs;
using System.ComponentModel.DataAnnotations;

namespace IaipDataService.SourceTests.Models;

public record SourceTestReportFlare : BaseSourceTestReport
{
    // Operating data

    [Display(Name = "Maximum expected operating capacity")]
    public ValueWithUnits MaxOperatingCapacity { get; set; }

    [Display(Name = "Operating capacity")]
    public ValueWithUnits OperatingCapacity { get; set; }

    [Display(Name = "Allowable emission rate(s)")]
    public List<ValueWithUnits> AllowableEmissionRates { get; init; } = [];

    [Display(Name = "Control equipment and monitoring data")]
    public string ControlEquipmentInfo { get; set; } = "";

    // Test run data

    [Display(Name = "Test runs")]
    public List<FlareTestRun> TestRuns { get; set; } = [];

    [Display(Name = "Average heating value")]
    public ValueWithUnits AvgHeatingValue { get; set; }

    [Display(Name = "Average emission rate velocity")]
    public ValueWithUnits AvgEmissionRateVelocity { get; set; }

    [Display(Name = "Percent allowable")]
    public string PercentAllowable { get; set; } = "";

    #region Confidential info handling

    public override SourceTestReportFlare RedactedStackTestReport() =>
        RedactedBaseStackTestReport<SourceTestReportFlare>() with
        {
            MaxOperatingCapacity = CheckConfidential(MaxOperatingCapacity, nameof(MaxOperatingCapacity)),
            OperatingCapacity = CheckConfidential(OperatingCapacity, nameof(OperatingCapacity)),
            ControlEquipmentInfo = CheckConfidential(ControlEquipmentInfo, nameof(ControlEquipmentInfo)),
            AvgHeatingValue = CheckConfidential(AvgHeatingValue, nameof(AvgHeatingValue)),
            AvgEmissionRateVelocity = CheckConfidential(AvgEmissionRateVelocity, nameof(AvgEmissionRateVelocity)),
            PercentAllowable = CheckConfidential(PercentAllowable, nameof(PercentAllowable)),
            TestRuns = BaseTestRun.RedactedTestRuns(TestRuns),
        };

    public override void ParseConfidentialParameters()
    {
        ConfidentialParameters = new HashSet<string>();
        TestRuns = BaseTestRun.ParsedTestRuns(TestRuns);

        if (ConfidentialParametersCode == "" || ConfidentialParametersCode[0] == '0') return;
        ParseBaseConfidentialParameters();

        AddIfConfidential(26, nameof(MaxOperatingCapacity));
        AddIfConfidential(27, nameof(OperatingCapacity));
        AddIfConfidential(31, nameof(ControlEquipmentInfo));
        AddIfConfidential(42, nameof(AvgHeatingValue));
        AddIfConfidential(44, nameof(AvgEmissionRateVelocity));
        AddIfConfidential(45, nameof(PercentAllowable));
        AddIfConfidential(46, nameof(Comments));
    }

    #endregion
}
