using System.ComponentModel.DataAnnotations;

namespace IaipDataService.SourceTests.Models.TestRun;

public record TwoStackTestRun : BaseTestRun
{
    // `BaseTestRun` includes the `RunNumber` property.
    // The database and IAIP allow each stack to have different run numbers,
    // but only the Stack One run numbers are displayed in the report.
    // Stack Two run numbers are not used:
    // `public string StackTwoRunNumber { get; init; } = "";`

    [Display(Name = "Gas temperature")]
    public string StackOneGasTemperature { get; init; } = "";

    public string StackTwoGasTemperature { get; init; } = "";

    [Display(Name = "Gas moisture")]
    public string StackOneGasMoisture { get; init; } = "";

    public string StackTwoGasMoisture { get; init; } = "";

    [Display(Name = "Gas flow rate")]
    public string StackOneGasFlowRateAcfm { get; init; } = "";

    public string StackTwoGasFlowRateAcfm { get; init; } = "";

    [Display(Name = "Gas flow rate")]
    public string StackOneGasFlowRateDscfm { get; init; } = "";

    public string StackTwoGasFlowRateDscfm { get; init; } = "";

    [Display(Name = "Pollutant concentration")]
    public string StackOnePollutantConcentration { get; init; } = "";

    public string StackTwoPollutantConcentration { get; init; } = "";

    [Display(Name = "Emission rate")]
    public string StackOneEmissionRate { get; init; } = "";

    public string StackTwoEmissionRate { get; init; } = "";

    // `SumEmissionRate` is used by Two Stack (Standard) but not by Two Stack (DRE)
    [Display(Name = "Total")]
    public string SumEmissionRate { get; init; } = "";

    #region Confidential info handling

    protected override TwoStackTestRun RedactedTestRun() =>
        RedactedBaseTestRun<TwoStackTestRun>() with
        {
            StackOneGasTemperature = CheckConfidential(StackOneGasTemperature, nameof(StackOneGasTemperature)),
            StackOneGasMoisture = CheckConfidential(StackOneGasMoisture, nameof(StackOneGasMoisture)),
            StackOneGasFlowRateAcfm = CheckConfidential(StackOneGasFlowRateAcfm, nameof(StackOneGasFlowRateAcfm)),
            StackOneGasFlowRateDscfm = CheckConfidential(StackOneGasFlowRateDscfm, nameof(StackOneGasFlowRateDscfm)),
            StackOnePollutantConcentration =
            CheckConfidential(StackOnePollutantConcentration, nameof(StackOnePollutantConcentration)),
            StackOneEmissionRate = CheckConfidential(StackOneEmissionRate, nameof(StackOneEmissionRate)),

            StackTwoGasTemperature = CheckConfidential(StackTwoGasTemperature, nameof(StackTwoGasTemperature)),
            StackTwoGasMoisture = CheckConfidential(StackTwoGasMoisture, nameof(StackTwoGasMoisture)),
            StackTwoGasFlowRateAcfm = CheckConfidential(StackTwoGasFlowRateAcfm, nameof(StackTwoGasFlowRateAcfm)),
            StackTwoGasFlowRateDscfm = CheckConfidential(StackTwoGasFlowRateDscfm, nameof(StackTwoGasFlowRateDscfm)),
            StackTwoPollutantConcentration =
            CheckConfidential(StackTwoPollutantConcentration, nameof(StackTwoPollutantConcentration)),
            StackTwoEmissionRate = CheckConfidential(StackTwoEmissionRate, nameof(StackTwoEmissionRate)),

            SumEmissionRate = CheckConfidential(SumEmissionRate, nameof(SumEmissionRate)),
        };

    protected override void ParseConfidentialParameters()
    {
        ConfidentialParameters = new HashSet<string>();
        if (ConfidentialParametersCode == "") return;
        ParseBaseConfidentialParameters();

        AddIfConfidential(2, nameof(StackOneGasTemperature));
        AddIfConfidential(3, nameof(StackOneGasMoisture));
        AddIfConfidential(4, nameof(StackOneGasFlowRateAcfm));
        AddIfConfidential(5, nameof(StackOneGasFlowRateDscfm));
        AddIfConfidential(6, nameof(StackOnePollutantConcentration));
        AddIfConfidential(7, nameof(StackOneEmissionRate));

        AddIfConfidential(8, nameof(StackTwoGasTemperature));
        AddIfConfidential(9, nameof(StackTwoGasMoisture));
        AddIfConfidential(10, nameof(StackTwoGasFlowRateAcfm));
        AddIfConfidential(11, nameof(StackTwoGasFlowRateDscfm));
        AddIfConfidential(12, nameof(StackTwoPollutantConcentration));
        AddIfConfidential(13, nameof(StackTwoEmissionRate));
        AddIfConfidential(14, nameof(SumEmissionRate));
    }

    #endregion
}
