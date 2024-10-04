using IaipDataService.SourceTests.Models.TestRun;
using IaipDataService.Structs;
using System.ComponentModel.DataAnnotations;

namespace IaipDataService.SourceTests.Models;

public record SourceTestReportPondTreatment : BaseSourceTestReport
{
    // Operating data

    [Display(Name = "Maximum expected operating capacity")]
    public ValueWithUnits MaxOperatingCapacity { get; init; }

    [Display(Name = "Operating capacity")]
    public ValueWithUnits OperatingCapacity { get; init; }

    [Display(Name = "Control equipment and monitoring data")]
    public string ControlEquipmentInfo { get; init; } = "";

    // Test run data

    [Display(Name = "Test runs")]
    public List<PondTreatmentTestRun> TestRuns { get; set; } = [];

    [Display(Name = "Average pollutant collection rate")]
    public ValueWithUnits AvgPollutantCollectionRate { get; init; }

    [Display(Name = "Average treatment rate")]
    public ValueWithUnits AvgTreatmentRate { get; init; }

    [Display(Name = "Destruction efficiency")]
    public string DestructionEfficiency { get; init; } = "";

    #region Confidential info handling

    public override SourceTestReportPondTreatment RedactedStackTestReport() =>
        RedactedBaseStackTestReport<SourceTestReportPondTreatment>() with
        {
            MaxOperatingCapacity = CheckConfidential(MaxOperatingCapacity, nameof(MaxOperatingCapacity)),
            OperatingCapacity = CheckConfidential(OperatingCapacity, nameof(OperatingCapacity)),
            ControlEquipmentInfo = CheckConfidential(ControlEquipmentInfo, nameof(ControlEquipmentInfo)),
            AvgPollutantCollectionRate =
            CheckConfidential(AvgPollutantCollectionRate, nameof(AvgPollutantCollectionRate)),
            AvgTreatmentRate = CheckConfidential(AvgTreatmentRate, nameof(AvgTreatmentRate)),
            DestructionEfficiency = CheckConfidential(DestructionEfficiency, nameof(DestructionEfficiency)),
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
        AddIfConfidential(43, nameof(AvgPollutantCollectionRate));
        AddIfConfidential(45, nameof(AvgTreatmentRate));
        AddIfConfidential(46, nameof(DestructionEfficiency));
        AddIfConfidential(47, nameof(Comments));
    }

    #endregion
}
