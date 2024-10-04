using IaipDataService.SourceTests.Models.TestRun;
using IaipDataService.Structs;
using System.ComponentModel.DataAnnotations;

namespace IaipDataService.SourceTests.Models;

public record SourceTestReportTwoStack : BaseSourceTestReport
{
    // Operating data

    [Display(Name = "Maximum expected operating capacity")]
    public ValueWithUnits MaxOperatingCapacity { get; init; }

    [Display(Name = "Operating capacity")]
    public ValueWithUnits OperatingCapacity { get; init; }

    [Display(Name = "Allowable emission rate(s)")]
    public List<ValueWithUnits> AllowableEmissionRates { get; init; } = [];

    [Display(Name = "Control equipment and monitoring data")]
    public string ControlEquipmentInfo { get; init; } = "";

    // Stacks

    public string StackOneName { get; init; } = "";
    public string StackTwoName { get; init; } = "";

    [Display(Name = "Test runs")]
    public List<TwoStackTestRun> TestRuns { get; set; } = [];

    [Display(Name = "Average pollutant concentration")]
    public ValueWithUnits StackOneAvgPollutantConcentration { get; init; }

    public ValueWithUnits StackTwoAvgPollutantConcentration { get; init; }

    [Display(Name = "Average emission rate")]
    public ValueWithUnits StackOneAvgEmissionRate { get; init; }

    public ValueWithUnits StackTwoAvgEmissionRate { get; init; }

    // `SumAvgEmissionRate` is only used by Two Stack (Standard)
    [Display(Name = "Total")]
    public ValueWithUnits SumAvgEmissionRate { get; init; }

    // `PercentAllowable` is only used by Two Stack (Standard)
    [Display(Name = "Percent allowable")]
    public string PercentAllowable { get; init; } = "";

    // `DestructionEfficiency` is only used by Two Stack (DRE)
    [Display(Name = "Destruction efficiency")]
    public string DestructionEfficiency { get; init; } = "";

    #region Confidential info handling

    public override SourceTestReportTwoStack RedactedStackTestReport() =>
        RedactedBaseStackTestReport<SourceTestReportTwoStack>() with
        {
            MaxOperatingCapacity = CheckConfidential(MaxOperatingCapacity, nameof(MaxOperatingCapacity)),
            OperatingCapacity = CheckConfidential(OperatingCapacity, nameof(OperatingCapacity)),
            ControlEquipmentInfo = CheckConfidential(ControlEquipmentInfo, nameof(ControlEquipmentInfo)),
            StackOneName = CheckConfidential(StackOneName, nameof(StackOneName)),
            StackTwoName = CheckConfidential(StackTwoName, nameof(StackTwoName)),
            StackOneAvgPollutantConcentration = CheckConfidential(StackOneAvgPollutantConcentration,
                nameof(StackOneAvgPollutantConcentration)),
            StackTwoAvgPollutantConcentration = CheckConfidential(StackTwoAvgPollutantConcentration,
                nameof(StackTwoAvgPollutantConcentration)),
            StackOneAvgEmissionRate = CheckConfidential(StackOneAvgEmissionRate, nameof(StackOneAvgEmissionRate)),
            StackTwoAvgEmissionRate = CheckConfidential(StackTwoAvgEmissionRate, nameof(StackTwoAvgEmissionRate)),
            SumAvgEmissionRate = CheckConfidential(SumAvgEmissionRate, nameof(SumAvgEmissionRate)),
            PercentAllowable = CheckConfidential(PercentAllowable, nameof(PercentAllowable)),
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
        AddIfConfidential(33, nameof(Comments));
        AddIfConfidential(34, nameof(StackOneName));
        AddIfConfidential(35, nameof(StackTwoName));
        AddIfConfidential(79, nameof(StackOneAvgPollutantConcentration));
        AddIfConfidential(80, nameof(StackTwoAvgPollutantConcentration));
        AddIfConfidential(82, nameof(StackOneAvgEmissionRate));
        AddIfConfidential(83, nameof(StackTwoAvgEmissionRate));
        AddIfConfidential(84, nameof(DestructionEfficiency));
        AddIfConfidential(87, nameof(SumAvgEmissionRate));
        AddIfConfidential(88, nameof(PercentAllowable));
    }

    #endregion
}
