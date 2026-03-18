namespace AirWeb.Domain.Compliance.Data;

public static partial class CommonData
{
    public static ICollection<AirProgram> AsAirPrograms(this List<string> airProgramIds) =>
        AllAirPrograms.Where(airProgram => airProgramIds.Contains(airProgram.Code)).ToList();

    // ReSharper disable StringLiteralTypo
    private static List<AirProgram> AllAirPrograms { get; } =
    [
        new("CAASIP", "SIP"),
        new("CAAFIP", "Federal SIP"),
        new("CAANFRP", "Non-Federal SIP"),
        new("CAACFC", "CFC Tracking"),
        new("CAAPSD", "PSD"),
        new("CAANSR", "NSR"),
        new("CAANESH", "NESHAP"),
        new("CAANSPS", "NSPS"),
        new("CAAAR", "Acid Precipitation"),
        new("CAAFENF", "Federally-Enforceable Requirement"),
        new("CAAFESOP", "FESOP"),
        new("CAAGHG", "Greenhouse Gas Reporting Rule"),
        new("CAANAM", "Native American"),
        new("CAAMACT", "MACT"),
        new("CAAPARGDC", "Prevention of Accidental Release"),
        new("CAATIP", "Tribal Implementation Plan (TIP)"),
        new("CAATVP", "Title V"),
        new("CAAGACTM", "40 CFR Part 63 Area Sources"),
        new("CAANSPSM", "NSPS (Non-Major)"),
        new("CAARMP", "Risk Management Program"),
    ];

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
}
