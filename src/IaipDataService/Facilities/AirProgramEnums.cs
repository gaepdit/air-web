using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace IaipDataService.Facilities;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum AirProgram
{
    [Display(Name = "SIP")] SIP = 1,
    [Display(Name = "Federal SIP")] FederalSIP = 2,
    [Display(Name = "Non-Federal SIP")] NonFederalSIP = 3,
    [Display(Name = "CFC Tracking")] CfcTracking = 4,
    [Display(Name = "PSD")] PSD = 5,
    [Display(Name = "NSR")] NSR = 6,
    [Display(Name = "NESHAP")] NESHAP = 7,
    [Display(Name = "NSPS")] NSPS = 8,
    [Display(Name = "FESOP")] FESOP = 9,
    [Display(Name = "Acid Precipitation")] AcidPrecipitation = 10,
    [Display(Name = "Native American")] NativeAmerican = 11,
    [Display(Name = "MACT")] MACT = 12,
    [Display(Name = "Title V")] TitleV = 13,
    [Display(Name = "Risk Management Plan")] RMP = 14,
}

public enum AirProgramClassification
{
    [Display(Name = "NSR/PSD Major")] NsrMajor = 1,
    [Display(Name = "HAPs Major")] HapMajor = 2,
}
