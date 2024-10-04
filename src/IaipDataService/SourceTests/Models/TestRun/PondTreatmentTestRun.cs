using System.ComponentModel.DataAnnotations;

namespace IaipDataService.SourceTests.Models.TestRun;

public record PondTreatmentTestRun : BaseTestRun
{
    [Display(Name = "Pollutant collection rate")]
    public string PollutantCollectionRate { get; init; } = "";

    [Display(Name = "Treatment rate")]
    public string TreatmentRate { get; init; } = "";

    #region Confidential info handling

    protected override PondTreatmentTestRun RedactedTestRun() =>
        RedactedBaseTestRun<PondTreatmentTestRun>() with
        {
            PollutantCollectionRate = CheckConfidential(PollutantCollectionRate, nameof(PollutantCollectionRate)),
            TreatmentRate = CheckConfidential(TreatmentRate, nameof(TreatmentRate)),
        };

    protected override void ParseConfidentialParameters()
    {
        ConfidentialParameters = new HashSet<string>();
        if (ConfidentialParametersCode == "") return;
        ParseBaseConfidentialParameters();

        AddIfConfidential(1, nameof(RunNumber));
        AddIfConfidential(2, nameof(PollutantCollectionRate));
        AddIfConfidential(3, nameof(TreatmentRate));
    }

    #endregion
}
