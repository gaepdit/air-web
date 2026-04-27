namespace AirWeb.Domain.Compliance.EnforcementEntities.ViolationTypes;

public static class ViolationTypeData
{
    public static ViolationType? GetViolationType(string? code) =>
        code is null ? null : ViolationTypes.Find(type => type.Code == code);

    public static IReadOnlyList<ViolationType> GetAll() =>
        ViolationTypes
            .OrderBy(v => v.Deprecated)
            .ThenBy(v => v.SeverityCode)
            .ThenBy(v => v.Description)
            .ToList();

    public static IReadOnlyList<ViolationType> GetCurrent() =>
        ViolationTypes
            .Where(v => !v.Deprecated)
            .OrderBy(v => v.SeverityCode)
            .ThenBy(v => v.Description)
            .ToList();

    internal static ViolationType GetRandomViolationType() =>
        ViolationTypes.Where(type => !type.Deprecated).OrderBy(_ => Guid.NewGuid()).First();

    // ReSharper disable StringLiteralTypo
    private static List<ViolationType> ViolationTypes { get; } =
    [
        new()
        {
            Code = "FCIO",
            Description =
                "Failure to construct, install or operate facility/equipment in accordance with permit or regulation",
            SeverityCode = ViolationSeverity.FRV,
            Deprecated = false,
        },
        new()
        {
            Code = "FMPR",
            Description = "Failure to maintain records as required by permit or regulation",
            SeverityCode = ViolationSeverity.FRV,
            Deprecated = false,
        },
        new()
        {
            Code = "FOMP",
            Description = "Failure to obtain or maintain a current permit",
            SeverityCode = ViolationSeverity.FRV,
            Deprecated = false,
        },
        new()
        {
            Code = "FRPR",
            Description = "Failure to report as required by permit or regulation",
            SeverityCode = ViolationSeverity.FRV,
            Deprecated = false,
        },
        new()
        {
            Code = "FTCM",
            Description = "Failure to test or conduct monitoring as required by permit or regulation",
            SeverityCode = ViolationSeverity.FRV,
            Deprecated = false,
        },
        new()
        {
            Code = "VDOA",
            Description = "Violation of consent decree, court order, or administrative order",
            SeverityCode = ViolationSeverity.FRV,
            Deprecated = false,
        },
        new()
        {
            Code = "VLSP",
            Description = "Violation of emission limit, emission standard, surrogate parameter",
            SeverityCode = ViolationSeverity.FRV,
            Deprecated = false,
        },
        new()
        {
            Code = "VWPS",
            Description = "Violation of Work Practice Standard",
            SeverityCode = ViolationSeverity.FRV,
            Deprecated = false,
        },
        new()
        {
            Code = "VNS1",
            Description = "Historic - In Violation No Schedule (1)",
            SeverityCode = ViolationSeverity.FRV,
            Deprecated = true,
        },
        new()
        {
            Code = "NMS",
            Description = "Historic - In Violation Not Meeting Schedule (6)",
            SeverityCode = ViolationSeverity.FRV,
            Deprecated = true,
        },
        new()
        {
            Code = "URG",
            Description = "Historic - In Violation Unknown with Regard to Schedule (7)",
            SeverityCode = ViolationSeverity.FRV,
            Deprecated = true,
        },
        new()
        {
            Code = "VEPC",
            Description = "Historic - In Violation with Regard to Both Emissions and Procedural Compliance (B)",
            SeverityCode = ViolationSeverity.FRV,
            Deprecated = true,
        },
        new()
        {
            Code = "RPC",
            Description = "Historic - In Violation with Regard to Procedural Compliance (W)",
            SeverityCode = ViolationSeverity.FRV,
            Deprecated = true,
        },
        new()
        {
            Code = "CRIT1",
            Description = "Criteria 1 - Failure to obtain NSR permit and/or install BACT/LAER",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = false,
        },
        new()
        {
            Code = "CRIT2",
            Description =
                "Criteria 2 - NSR/PSD/SIP, violation of emission limit, emission standard, or surrogate parameter",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = false,
        },
        new()
        {
            Code = "CRIT3",
            Description = "Criteria 3 - NSPS, violation of emission limit, emission standard, surrogate parameter",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = false,
        },
        new()
        {
            Code = "CRIT4",
            Description = "Criteria 4 - NESHAP, violation of emission limit, emission standard, surrogate parameter",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = false,
        },
        new()
        {
            Code = "CRIT5",
            Description =
                "Criteria 5 - Violation of work practice standard, testing requirements, monitoring requirements, recordkeeping or reporting requirements",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = false,
        },
        new()
        {
            Code = "CRIT6",
            Description = "Criteria 6 - Other HPV",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = false,
        },
        new()
        {
            Code = "M1A",
            Description = "Historic - Any violation of emission limit detected via stack testing",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = true,
        },
        new()
        {
            Code = "M3F",
            Description = "Historic - Any violation of non-opacity (>24 hours standard) via CEM",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = true,
        },
        new()
        {
            Code = "GC9",
            Description = "Historic - Chronic or Recalcitrant Violation",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = true,
        },
        new()
        {
            Code = "DIS",
            Description = "Historic - Discretionary HPV",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = true,
        },
        new()
        {
            Code = "GC8",
            Description = "Historic - Emission Violation",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = true,
        },
        new()
        {
            Code = "GC4",
            Description = "Historic - Enforcement Violation",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = true,
        },
        new()
        {
            Code = "GC1",
            Description = "Historic - Fail to Obtain PSD or NSR Permit",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = true,
        },
        new()
        {
            Code = "G10",
            Description = "Historic - Section 112(r) Violation",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = true,
        },
        new()
        {
            Code = "GC7",
            Description = "Historic - Testing, Monitoring, Recordkeeping, or Reporting Violation",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = true,
        },
        new()
        {
            Code = "GC5",
            Description = "Historic - Title V Certification Violation",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = true,
        },
        new()
        {
            Code = "GC6",
            Description = "Historic - Title V Permit Application Violation",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = true,
        },
        new()
        {
            Code = "GC2",
            Description = "Historic - Violation Of Air Toxics Requirements",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = true,
        },
        new()
        {
            Code = "M2A",
            Description = "Historic - Violation of Direct Surrogate for >5% of limit for >3% of OT (operating time)",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = true,
        },
        new()
        {
            Code = "M2B",
            Description = "Historic - Violation of Direct Surrogate for >50% of OT (operating time)",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = true,
        },
        new()
        {
            Code = "M2C",
            Description = "Historic - Violation of Direct Surrogate of >25% for 2 reporting periods",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = true,
        },
        new()
        {
            Code = "M1B",
            Description = "Historic - Violation of emission limits > 15% via sampling",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = true,
        },
        new()
        {
            Code = "M1C",
            Description = "Historic - Violation of emission limits > the SST (Supplemental Significant Threshold)",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = true,
        },
        new()
        {
            Code = "M3B",
            Description =
                "Historic - Violation of non- opacity standard via CEM of the SST(Supplemental Significant Threshold)",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = true,
        },
        new()
        {
            Code = "M3A",
            Description = "Historic - Violation of non-opacity standard via CEM of >15% for >5% of operating time",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = true,
        },
        new()
        {
            Code = "M3C",
            Description = "Historic - Violation of non-opacity standard via CEM of >15% for 2 reporting periods",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = true,
        },
        new()
        {
            Code = "M3E",
            Description =
                "Historic - Violation of non-opacity standard via CEM of >25% during two consecutive reporting periods",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = true,
        },
        new()
        {
            Code = "M3D",
            Description =
                "Historic - Violation of non-opacity standard via CEM of >50% of the operating time during the reporting period",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = true,
        },
        new()
        {
            Code = "M4C",
            Description =
                "Historic - Violation of opacity standards (> 20%) via Continuous Opacity Monitoring (COM) for >5% of operating Time",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = true,
        },
        new()
        {
            Code = "M4D",
            Description =
                "Historic - Violation of opacity standards (>20%) via Continuous Opacity Monitoring (COM) for 5% operating time",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = true,
        },
        new()
        {
            Code = "M4F",
            Description = "Historic - Violation of opacity standards (>20%) via Method 9 VE Readings",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = true,
        },
        new()
        {
            Code = "M4A",
            Description = "Historic - Violation of opacity standards (0-20%) via Continuous Opacity Monitoring (COM)",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = true,
        },
        new()
        {
            Code = "M4E",
            Description = "Historic - Violation of opacity standards (0-20%) via Method 9 VE Readings",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = true,
        },
        new()
        {
            Code = "GC3",
            Description = "Historic - Violation that Affects Synthetic Minor Status",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = true,
        },
        new()
        {
            Code = "M4B",
            Description =
                "Historic - Violations of opacity standards >3% of operating time via Continuous Opacity Monitoring during two consecutive reporting periods",
            SeverityCode = ViolationSeverity.HPV,
            Deprecated = true,
        },
        new()
        {
            Code = "VFEV",
            Description = "Violation of Federally-Enforceable Rule or Regulation that is not federally-reportable",
            SeverityCode = ViolationSeverity.NFR,
            Deprecated = false,
        },
        new()
        {
            Code = "VSTL",
            Description = "Violation of State, Tribal or Local Agency Only Regulation",
            SeverityCode = ViolationSeverity.OTH,
            Deprecated = false,
        },
    ];
}
