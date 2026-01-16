using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable IdentifierTypo

namespace IaipDataService.Facilities;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum AirProgram
{
    [Display(Name = "SIP")] CAASIP,
    [Display(Name = "Federal SIP")] CAAFIP,
    [Display(Name = "Non-Federal SIP")] CAANFRP,
    [Display(Name = "CFC Tracking")] CAACFC,
    [Display(Name = "PSD")] CAAPSD,
    [Display(Name = "NSR")] CAANSR,
    [Display(Name = "NESHAP")] CAANESH,
    [Display(Name = "NSPS")] CAANSPS,
    [Display(Name = "Acid Precipitation")] CAAAR,
    [Display(Name = "Federally-Enforceable Requirement")] CAAFENF,
    [Display(Name = "FESOP")] CAAFESOP,
    [Display(Name = "Greenhouse Gas Reporting Rule")] CAAGHG,
    [Display(Name = "Native American")] CAANAM,
    [Display(Name = "MACT")] CAAMACT,
    [Display(Name = "Prevention of Accidental Release")] CAAPARGDC,
    [Display(Name = "Tribal Implementation Plan (TIP)")] CAATIP,
    [Display(Name = "Title V")] CAATVP,
    [Display(Name = "40 CFR Part 63 Area Sources")] CAAGACTM,
    [Display(Name = "NSPS (Non-Major)")] CAANSPSM,
    [Display(Name = "Risk Management Program")] CAARMP,
}

// ICIS Air Program Codes (original from `airbranch.dbo.LK_ICIS_PROGRAM`
// 
// | LGCY_PROGRAM_CODE | ICIS_PROGRAM_CODE | ICIS_PROGRAM_DESC                                                        |
// |-------------------|-------------------|--------------------------------------------------------------------------|
// | 0                 | CAASIP            | State Implementation Plan (SIP)                                          |
// | 1                 | CAAFIP            | Federal Implementation Plan (FIP)                                        |
// | 3                 | CAANFRP           | Non-Federally Enforceable Program                                        |
// | 4                 | CAACFC            | Chlorofluorocarbons (CFC) Tracking (Stratospheric Ozone Protection)      |
// | 6                 | CAAPSD            | Prevention of Significant Deterioration (PSD)                            |
// | 7                 | CAANSR            | New Source Review (NSR)                                                  |
// | 8                 | CAANESH           | National Emission Standards for Hazardous Air Pollutants (NESHAPs)       |
// | 9                 | CAANSPS           | New Source Performance Standards (NSPS)                                  |
// | A                 | CAAAR             | Acid Precipitation Control Deposition (CAA Title IV)                     |
// | E                 | CAAFENF           | Federally-Enforceable Requirement                                        |
// | F                 | CAAFESOP          | Federally Enforceable State Operating Permit – Non Title V (FESOP)       |
// | G                 | CAAGHG            | Part 98, The Mandatory Greenhouse Gas (GHG)Reporting Rule                |
// | I                 | CAANAM            | Native American                                                          |
// | M                 | CAAMACT           | National Emission Standards for Hazardous Air Pollutants (NESHAPs)(MACT) |
// | R                 | CAAPARGDC         | Prevention of Accidental Release/General Duty Clause                     |
// | T                 | CAATIP            | Tribal Implementation Plan (TIP)                                         |
// | V                 | CAATVP            | Title V Permits                                                          |
// | <null>            | CAAGACTM          | 40 CFR Part 63 Area Sources                                              |
// | <null>            | CAANSPSM          | New Source Performance Standards (Non-Major)                             |
// | <null>            | CAARMP            | Risk Management Program                                                  |

public enum AirProgramClassification
{
    [Display(Name = "NSR/PSD Major")] NsrMajor = 1,
    [Display(Name = "HAPs Major")] HapMajor = 2,
}
