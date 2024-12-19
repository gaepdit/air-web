using IaipDataService.SourceTests.Models.TestRun;
using IaipDataService.Structs;
using System.ComponentModel.DataAnnotations;

namespace IaipDataService.SourceTests.Models;

public record SourceTestReportGasConcentration : BaseSourceTestReport
{
    // Operating data

    [Display(Name = "Maximum expected operating capacity")]
    public ValueWithUnits MaxOperatingCapacity { get; set; }

    [Display(Name = "Operating capacity")]
    public ValueWithUnits OperatingCapacity { get; set; }

    [Display(Name = "Allowable emission rate(s)")]
    public List<ValueWithUnits> AllowableEmissionRates { get; init; } = [];

    [Display(Name = "Control equipment and monitoring data")]
    public string ControlEquipmentInfo { get; set; } = null!;

    // Test run data

    [Display(Name = "Test runs")]
    public List<GasConcentrationTestRun> TestRuns { get; set; } = [];

    [Display(Name = "Average pollutant concentration")]
    public ValueWithUnits AvgPollutantConcentration { get; set; }

    [Display(Name = "Average emission rate")]
    public ValueWithUnits AvgEmissionRate { get; set; }

    [Display(Name = "Percent allowable")]
    public string PercentAllowable { get; set; } = null!;

    #region Confidential info handling

    public override SourceTestReportGasConcentration RedactedStackTestReport() =>
        RedactedBaseStackTestReport<SourceTestReportGasConcentration>() with
        {
            MaxOperatingCapacity = CheckConfidential(MaxOperatingCapacity, nameof(MaxOperatingCapacity)),
            OperatingCapacity = CheckConfidential(OperatingCapacity, nameof(OperatingCapacity)),
            ControlEquipmentInfo = CheckConfidential(ControlEquipmentInfo, nameof(ControlEquipmentInfo)),
            AvgPollutantConcentration = CheckConfidential(AvgPollutantConcentration, nameof(AvgPollutantConcentration)),
            AvgEmissionRate = CheckConfidential(AvgEmissionRate, nameof(AvgEmissionRate)),
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
        AddIfConfidential(32, nameof(ControlEquipmentInfo));
        AddIfConfidential(43, nameof(AvgPollutantConcentration));
        AddIfConfidential(45, nameof(AvgEmissionRate));
        AddIfConfidential(46, nameof(PercentAllowable));
        AddIfConfidential(47, nameof(Comments));
    }

    #endregion
}
