using IaipDataService.SourceTests.Models.TestRun;
using System.ComponentModel.DataAnnotations;

namespace IaipDataService.SourceTests.Models;

public record SourceTestReportOpacity : BaseSourceTestReport
{
    // Operating data

    [Display(Name = "Control equipment and monitoring data")]
    public string ControlEquipmentInfo { get; set; } = null!;

    // `OpacityStandard` is used by "Method9Single" and "Method9Multi"
    // but not by "Method22"
    [Display(Name = "Opacity standard")]
    public string OpacityStandard { get; set; } = null!;

    // `EquipmentItem` is used by "Method9Single" and "Method22"
    // but not by "Method9Multi"
    [Display(Name = "Test duration")]
    public string TestDuration { get; set; } = null!;

    // Test run data

    [Display(Name = "Maximum expected operating capacity")]
    public string MaxOperatingCapacityUnits { get; set; } = null!;

    [Display(Name = "Operating capacity")]
    public string OperatingCapacityUnits { get; set; } = null!;

    // `AllowableEmissionRateUnits` is used by "Method9Single" and "Method9Multi"
    // but not by "Method22"
    [Display(Name = "Allowable emission rate(s)")]
    public string AllowableEmissionRateUnits { get; set; } = null!;

    [Display(Name = "Test runs")]
    public List<OpacityTestRun> TestRuns { get; set; } = [];

    #region Confidential info handling

    public override SourceTestReportOpacity RedactedStackTestReport() =>
        RedactedBaseStackTestReport<SourceTestReportOpacity>() with
        {
            ControlEquipmentInfo = CheckConfidential(ControlEquipmentInfo, nameof(ControlEquipmentInfo)),
            TestDuration = CheckConfidential(TestDuration, nameof(TestDuration)),
            TestRuns = BaseTestRun.RedactedTestRuns(TestRuns),
        };

    public override void ParseConfidentialParameters()
    {
        ConfidentialParameters = new HashSet<string>();
        TestRuns = BaseTestRun.ParsedTestRuns(TestRuns);

        if (ConfidentialParametersCode == "" || ConfidentialParametersCode[0] == '0') return;
        ParseBaseConfidentialParameters();

        switch (DocumentType)
        {
            case DocumentType.Method9Multi:
                AddIfConfidential(45, nameof(ControlEquipmentInfo));
                AddIfConfidential(51, nameof(Comments));
                break;

            case DocumentType.Method9Single:
                AddIfConfidential(30, nameof(ControlEquipmentInfo));
                AddIfConfidential(31, nameof(TestDuration));
                AddIfConfidential(33, nameof(Comments));
                break;

            case DocumentType.Method22:
                AddIfConfidential(30, nameof(TestDuration));
                AddIfConfidential(32, nameof(Comments));
                break;
        }
    }

    #endregion
}
