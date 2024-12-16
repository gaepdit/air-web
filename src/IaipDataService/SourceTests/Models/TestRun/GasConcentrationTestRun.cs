using System.ComponentModel.DataAnnotations;

namespace IaipDataService.SourceTests.Models.TestRun;

public record GasConcentrationTestRun : BaseTestRun
{
    [Display(Name = "Pollutant concentration")]
    public string PollutantConcentration { get; init; } = null!;

    [Display(Name = "Emission rate")]
    public string EmissionRate { get; init; } = null!;

    #region Confidential info handling

    protected override GasConcentrationTestRun RedactedTestRun() =>
        RedactedBaseTestRun<GasConcentrationTestRun>() with
        {
            PollutantConcentration = CheckConfidential(PollutantConcentration, nameof(PollutantConcentration)),
            EmissionRate = CheckConfidential(EmissionRate, nameof(EmissionRate)),
        };

    protected override void ParseConfidentialParameters()
    {
        ConfidentialParameters = new HashSet<string>();
        if (ConfidentialParametersCode == "") return;
        ParseBaseConfidentialParameters();

        AddIfConfidential(1, nameof(RunNumber));
        AddIfConfidential(2, nameof(PollutantConcentration));
        AddIfConfidential(3, nameof(EmissionRate));
    }

    #endregion
}
