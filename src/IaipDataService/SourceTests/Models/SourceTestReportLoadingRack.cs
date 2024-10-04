using IaipDataService.Structs;
using System.ComponentModel.DataAnnotations;

namespace IaipDataService.SourceTests.Models;

public record SourceTestReportLoadingRack : BaseSourceTestReport
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

    // Test run data

    [Display(Name = "Test duration")]
    public ValueWithUnits TestDuration { get; init; }

    [Display(Name = "Pollutant concentration")]
    public ValueWithUnits PollutantConcentrationIn { get; init; }

    public ValueWithUnits PollutantConcentrationOut { get; init; }

    [Display(Name = "Emission rate")]
    public ValueWithUnits EmissionRate { get; init; }

    [Display(Name = "Destruction reduction efficiency")]
    public ValueWithUnits DestructionReduction { get; init; }

    #region Confidential info handling

    public override SourceTestReportLoadingRack RedactedStackTestReport() =>
        RedactedBaseStackTestReport<SourceTestReportLoadingRack>() with
        {
            MaxOperatingCapacity = CheckConfidential(MaxOperatingCapacity, nameof(MaxOperatingCapacity)),
            OperatingCapacity = CheckConfidential(OperatingCapacity, nameof(OperatingCapacity)),
            ControlEquipmentInfo = CheckConfidential(ControlEquipmentInfo, nameof(ControlEquipmentInfo)),
            TestDuration = CheckConfidential(TestDuration, nameof(TestDuration)),
            PollutantConcentrationIn = CheckConfidential(PollutantConcentrationIn, nameof(PollutantConcentrationIn)),
            PollutantConcentrationOut = CheckConfidential(PollutantConcentrationOut, nameof(PollutantConcentrationOut)),
            EmissionRate = CheckConfidential(EmissionRate, nameof(EmissionRate)),
            DestructionReduction = CheckConfidential(DestructionReduction, nameof(DestructionReduction)),
        };

    public override void ParseConfidentialParameters()
    {
        ConfidentialParameters = new HashSet<string>();
        if (ConfidentialParametersCode == "" || ConfidentialParametersCode[0] == '0') return;
        ParseBaseConfidentialParameters();

        AddIfConfidential(26, nameof(MaxOperatingCapacity));
        AddIfConfidential(27, nameof(OperatingCapacity));
        AddIfConfidential(32, nameof(ControlEquipmentInfo));
        AddIfConfidential(33, nameof(TestDuration));
        AddIfConfidential(34, nameof(PollutantConcentrationIn));
        AddIfConfidential(35, nameof(PollutantConcentrationOut));
        AddIfConfidential(36, nameof(DestructionReduction));
        AddIfConfidential(37, nameof(EmissionRate));
        AddIfConfidential(38, nameof(Comments));
    }

    #endregion
}
