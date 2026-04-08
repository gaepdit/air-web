using System.ComponentModel.DataAnnotations;

namespace IaipDataService.Facilities;

public enum AirProgramClassification
{
    [Display(Name = "NSR/PSD Major")] NsrMajor = 1,
    [Display(Name = "HAPs Major")] HapMajor = 2,
}
