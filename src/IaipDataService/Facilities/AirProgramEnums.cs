using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace IaipDataService.Facilities;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum AirProgram
{
    [Description("SIP")] SIP = 1,
    [Description("Federal SIP")] FederalSIP = 2,
    [Description("Non-Federal SIP")] NonFederalSIP = 3,
    [Description("CFC Tracking")] CfcTracking = 4,
    [Description("PSD")] PSD = 5,
    [Description("NSR")] NSR = 6,
    [Description("NESHAP")] NESHAP = 7,
    [Description("NSPS")] NSPS = 8,
    [Description("FESOP")] FESOP = 9,
    [Description("Acid Precipitation")] AcidPrecipitation = 10,
    [Description("Native American")] NativeAmerican = 11,
    [Description("MACT")] MACT = 12,
    [Description("Title V")] TitleV = 13,
    [Description("Risk Management Plan")] RMP = 14,
}

public enum AirProgramClassifications
{
    [Description("NSR/PSD Major")] NsrMajor = 1,
    [Description("HAPs Major")] HapMajor = 2,
}
