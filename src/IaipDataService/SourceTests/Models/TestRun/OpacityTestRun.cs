using System.ComponentModel.DataAnnotations;

namespace IaipDataService.SourceTests.Models.TestRun;

public record OpacityTestRun : BaseTestRun
{
    [Display(Name = "Maximum expected operating capacity")]
    public string MaxOperatingCapacity { get; init; } = "";

    [Display(Name = "Operating capacity")]
    public string OperatingCapacity { get; init; } = "";

    [Display(Name = "Allowable emission rate")]
    public string AllowableEmissionRate { get; init; } = "";

    // `Opacity` is used by "Method 9 (Single)" and "Method 9 (Multi.)"
    // but not by "Method22"
    [Display(Name = "Opacity")]
    public string Opacity { get; init; } = "";

    // `EquipmentItem` is used by "Method22"
    // but not by "Method 9 (Single)" or "Method 9 (Multi.)"
    [Display(Name = "Accumulated emission time")]
    public string AccumulatedEmissionTime { get; init; } = "";

    // `EquipmentItem` is used by "Method 9 (Multi.)"
    // but not by "Method 9 (Single)" or "Method22"
    [Display(Name = "Equipment list")]
    public string EquipmentItem { get; init; } = "";

    #region Confidential info handling

    protected override OpacityTestRun RedactedTestRun() =>
        RedactedBaseTestRun<OpacityTestRun>() with
        {
            MaxOperatingCapacity = CheckConfidential(MaxOperatingCapacity, nameof(MaxOperatingCapacity)),
            OperatingCapacity = CheckConfidential(OperatingCapacity, nameof(OperatingCapacity)),
            EquipmentItem = CheckConfidential(EquipmentItem, nameof(EquipmentItem)),
        };

    protected override void ParseConfidentialParameters()
    {
        ConfidentialParameters = new HashSet<string>();
        if (ConfidentialParametersCode == "") return;
        ParseBaseConfidentialParameters();

        AddIfConfidential(2, nameof(MaxOperatingCapacity));
        AddIfConfidential(3, nameof(OperatingCapacity));
        AddIfConfidential(4, nameof(EquipmentItem));
    }

    #endregion
}
