using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable IdentifierTypo

namespace IaipDataService.Facilities;

// The enum numbering matches the numbering system used in the IAIP DB `dbo.APBHEADERDATA.STRAIRPROGRAMCODES`
// and the values returned by the `air.IaipFacilityAirProgramData` stored procedure.
// The IAIP only goes up to 14; additional programs not used in the IAIP are numbered starting at 101.
// This numbering system is deprecated by PR https://github.com/gaepdit/air-web/pull/537, which
// switches from an enum to a class and rewrites the SP to return program codes rather than integers.
[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum AirProgram
{
    [Display(Name = "SIP")] CAASIP = 1,
    [Display(Name = "Federal SIP")] CAAFIP = 2,
    [Display(Name = "Non-Federal SIP")] CAANFRP = 3,
    [Display(Name = "CFC Tracking")] CAACFC = 4,
    [Display(Name = "PSD")] CAAPSD = 5,
    [Display(Name = "NSR")] CAANSR = 6,
    [Display(Name = "NESHAP")] CAANESH = 7,
    [Display(Name = "NSPS")] CAANSPS = 8,
    [Display(Name = "Acid Precipitation")] CAAAR = 10,
    [Display(Name = "Federally-Enforceable Requirement")] CAAFENF = 101,
    [Display(Name = "FESOP")] CAAFESOP = 9,
    [Display(Name = "Greenhouse Gas Reporting Rule")] CAAGHG = 102,
    [Display(Name = "Native American")] CAANAM = 11,
    [Display(Name = "MACT")] CAAMACT = 12,
    [Display(Name = "Prevention of Accidental Release")] CAAPARGDC = 103,
    [Display(Name = "Tribal Implementation Plan (TIP)")] CAATIP = 104,
    [Display(Name = "Title V")] CAATVP = 13,
    [Display(Name = "40 CFR Part 63 Area Sources")] CAAGACTM = 105,
    [Display(Name = "NSPS (Non-Major)")] CAANSPSM = 106,
    [Display(Name = "Risk Management Program")] CAARMP = 14,
}

// Original Air Program numbering from the IAIP:
//
// Public ReadOnly Property AirProgramBitPosition As New Dictionary(Of AirPrograms, Integer) From {
//     {AirPrograms.None, 0},
//     {AirPrograms.SIP, 1},
//     {AirPrograms.FederalSIP, 2},
//     {AirPrograms.NonFederalSIP, 3},
//     {AirPrograms.CfcTracking, 4},
//     {AirPrograms.PSD, 5},
//     {AirPrograms.NSR, 6},
//     {AirPrograms.NESHAP, 7},
//     {AirPrograms.NSPS, 8},
//     {AirPrograms.FESOP, 9},
//     {AirPrograms.AcidPrecipitation, 10},
//     {AirPrograms.NativeAmerican, 11},
//     {AirPrograms.MACT, 12},
//     {AirPrograms.TitleV, 13},
//     {AirPrograms.RMP, 14}
// }

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
