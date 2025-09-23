using IaipDataService.Structs;
using System.ComponentModel.DataAnnotations;

namespace IaipDataService.SourceTests.Models;

public record SourceTestMemorandum : BaseSourceTestReport
{
    // Base stack test report property `Comments` is repurposed to display the memorandum field, "STRMEMORANDUMFIELD"

    // Only used by MemorandumToFile
    [Display(Name = "Monitor manufacturer and model")]
    public string MonitorManufacturer { get; set; } = null!;

    [Display(Name = "Monitor serial number")]
    public string MonitorSerialNumber { get; set; } = null!;

    // Only used by PTE
    [Display(Name = "Maximum expected operating capacity")]
    public ValueWithUnits MaxOperatingCapacity { get; set; }

    [Display(Name = "Operating capacity")]
    public ValueWithUnits OperatingCapacity { get; set; }

    [Display(Name = "Allowable emission rate(s)")]
    public List<ValueWithUnits> AllowableEmissionRates { get; init; } = [];

    [Display(Name = "Control equipment and monitoring data")]
    public string ControlEquipmentInfo { get; set; } = null!;

    #region Confidential info handling

    public override SourceTestMemorandum RedactedStackTestReport() =>
        RedactedBaseStackTestReport<SourceTestMemorandum>() with
        {
            MonitorManufacturer = CheckConfidential(MonitorManufacturer, nameof(MonitorManufacturer)),
            MonitorSerialNumber = CheckConfidential(MonitorSerialNumber, nameof(MonitorSerialNumber)),
            MaxOperatingCapacity = CheckConfidential(MaxOperatingCapacity, nameof(MaxOperatingCapacity)),
            OperatingCapacity = CheckConfidential(OperatingCapacity, nameof(OperatingCapacity)),
            ControlEquipmentInfo = CheckConfidential(ControlEquipmentInfo, nameof(ControlEquipmentInfo)),
        };

    public override void ParseConfidentialParameters()
    {
        ConfidentialParameters = new HashSet<string>();

        if (NoConfidentialParameters()) return;
        ParseBaseConfidentialParameters();

        switch (DocumentType)
        {
            case DocumentType.MemorandumStandard:
                AddIfConfidential(27, nameof(Comments));
                break;

            case DocumentType.MemorandumToFile:
                AddIfConfidential(29, nameof(Comments));
                AddIfConfidential(27, nameof(MonitorManufacturer));
                AddIfConfidential(28, nameof(MonitorSerialNumber));
                break;

            case DocumentType.PTE:
                AddIfConfidential(33, nameof(Comments));
                AddIfConfidential(27, nameof(MaxOperatingCapacity));
                AddIfConfidential(28, nameof(OperatingCapacity));
                AddIfConfidential(32, nameof(ControlEquipmentInfo));
                break;
        }
    }

    #endregion
}
