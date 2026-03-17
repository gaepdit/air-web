using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable IdentifierTypo

namespace IaipDataService.Facilities;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum AirProgramClassification
{
    [Display(Name = "NSR/PSD Major")] NsrMajor = 1,
    [Display(Name = "HAPs Major")] HapMajor = 2,
}
