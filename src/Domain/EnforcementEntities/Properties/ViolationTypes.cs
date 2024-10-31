namespace AirWeb.Domain.EnforcementEntities.Properties;

// Source of data from IAIP airbranch DB:
//
// ```sql
// select AIRVIOLATIONTYPECODE as Code,
//        VIOLATIONTYPEDESC    as Description,
//        SEVERITYCODE         as SeverityCode,
//        lower(DEPRECATED)    as Deprecated
// from dbo.LK_VIOLATION_TYPE
// where STATUS = 'A'
// order by SEVERITYCODE, DEPRECATED, VIOLATIONTYPEDESC
// ```

public static partial class Data
{
    public static List<ViolationType> ViolationTypes { get; } =
    [
        new(
            "FCIO",
            "Failure to construct, install or operate facility/equipment in accordance with permit or regulation",
            "FRV",
            false),
        new(
            "FMPR",
            "Failure to maintain records as required by permit or regulation",
            "FRV",
            false),
        new(
            "FOMP",
            "Failure to obtain or maintain a current permit",
            "FRV",
            false),
        new(
            "FRPR",
            "Failure to report as required by permit or regulation",
            "FRV",
            false),
        new(
            "FTCM",
            "Failure to test or conduct monitoring as required by permit or regulation",
            "FRV",
            false),
        new(
            "VDOA",
            "Violation of consent decree, court order, or administrative order",
            "FRV",
            false),
        new(
            "VLSP",
            "Violation of emission limit, emission standard, surrogate parameter",
            "FRV",
            false),
        new(
            "VWPS",
            "Violation of Work Practice Standard",
            "FRV",
            false),
        new(
            "VNS1",
            "Historic - In Violation No Schedule (1)",
            "FRV",
            true),
        new(
            "NMS",
            "Historic - In Violation Not Meeting Schedule (6)",
            "FRV",
            true),
        new(
            "URG",
            "Historic - In Violation Unknown with Regard to Schedule (7)",
            "FRV",
            true),
        new(
            "VEPC",
            "Historic - In Violation with Regard to Both Emissions and Procedural Compliance (B)",
            "FRV",
            true),
        new(
            "RPC",
            "Historic - In Violation with Regard to Procedural Compliance (W)",
            "FRV",
            true),
        new(
            "CRIT1",
            "Criteria 1 - Failure to obtain NSR permit and/or install BACT/LAER",
            "HPV",
            false),
        new(
            "CRIT2",
            "Criteria 2 - NSR/PSD/SIP, violation of emission limit, emission standard, or surrogate parameter",
            "HPV",
            false),
        new(
            "CRIT3",
            "Criteria 3 - NSPS, violation of emission limit, emission standard, surrogate parameter",
            "HPV",
            false),
        new(
            "CRIT4",
            "Criteria 4 - NESHAP, violation of emission limit, emission standard, surrogate parameter",
            "HPV",
            false),
        new(
            "CRIT5",
            "Criteria 5 - Violation of work practice standard, testing requirements, monitoring requirements, recordkeeping or reporting requirements",
            "HPV",
            false),
        new(
            "CRIT6",
            "Criteria 6 - Other HPV",
            "HPV",
            false),
        new(
            "M1A",
            "Historic - Any violation of emission limit detected via stack testing",
            "HPV",
            true),
        new(
            "M3F",
            "Historic - Any violation of non-opacity (>24 hours standard) via CEM",
            "HPV",
            true),
        new(
            "GC9",
            "Historic - Chronic or Recalcitrant Violation",
            "HPV",
            true),
        new(
            "DIS",
            "Historic - Discretionary HPV",
            "HPV",
            true),
        new(
            "GC8",
            "Historic - Emission Violation",
            "HPV",
            true),
        new(
            "GC4",
            "Historic - Enforcement Violation",
            "HPV",
            true),
        new(
            "GC1",
            "Historic - Fail to Obtain PSD or NSR Permit",
            "HPV",
            true),
        new(
            "G10",
            "Historic - Section 112(r) Violation",
            "HPV",
            true),
        new(
            "GC7",
            "Historic - Testing, Monitoring, Recordkeeping, or Reporting Violation",
            "HPV",
            true),
        new(
            "GC5",
            "Historic - Title V Certification Violation",
            "HPV",
            true),
        new(
            "GC6",
            "Historic - Title V Permit Application Violation",
            "HPV",
            true),
        new(
            "GC2",
            "Historic - Violation Of Air Toxics Requirements",
            "HPV",
            true),
        new(
            "M2A",
            "Historic - Violation of Direct Surrogate for >5% of limit for >3% of OT (operating time)",
            "HPV",
            true),
        new(
            "M2B",
            "Historic - Violation of Direct Surrogate for >50% of OT (operating time)",
            "HPV",
            true),
        new(
            "M2C",
            "Historic - Violation of Direct Surrogate of >25% for 2 reporting periods",
            "HPV",
            true),
        new(
            "M1B",
            "Historic - Violation of emission limits > 15% via sampling",
            "HPV",
            true),
        new(
            "M1C",
            "Historic - Violation of emission limits > the SST (Supplemental Significant Threshold)",
            "HPV",
            true),
        new(
            "M3B",
            "Historic - Violation of non- opacity standard via CEM of the SST(Supplemental Significant Threshold)",
            "HPV",
            true),
        new(
            "M3A",
            "Historic - Violation of non-opacity standard via CEM of >15% for >5% of operating time",
            "HPV",
            true),
        new(
            "M3C",
            "Historic - Violation of non-opacity standard via CEM of >15% for 2 reporting periods",
            "HPV",
            true),
        new(
            "M3E",
            "Historic - Violation of non-opacity standard via CEM of >25% during two consecutive reporting periods",
            "HPV",
            true),
        new(
            "M3D",
            "Historic - Violation of non-opacity standard via CEM of >50% of the operating time during the reporting period",
            "HPV",
            true),
        new(
            "M4C",
            "Historic - Violation of opacity standards (> 20%) via Continuous Opacity Monitoring (COM) for >5% of operating Time",
            "HPV",
            true),
        new(
            "M4D",
            "Historic - Violation of opacity standards (>20%) via Continuous Opacity Monitoring (COM) for 5% operating time",
            "HPV",
            true),
        new(
            "M4F",
            "Historic - Violation of opacity standards (>20%) via Method 9 VE Readings",
            "HPV",
            true),
        new(
            "M4A",
            "Historic - Violation of opacity standards (0-20%) via Continuous Opacity Monitoring (COM)",
            "HPV",
            true),
        new(
            "M4E",
            "Historic - Violation of opacity standards (0-20%) via Method 9 VE Readings",
            "HPV",
            true),
        new(
            "GC3",
            "Historic - Violation that Affects Synthetic Minor Status",
            "HPV",
            true),
        new(
            "M4B",
            "Historic - Violations of opacity standards >3% of operating time via Continuous Opacity Monitoring during two consecutive reporting periods",
            "HPV",
            true),
        new(
            "VFEV",
            "Violation of Federally-Enforceable Rule or Regulation that is not federally-reportable",
            "NFR",
            false),
        new(
            "VSTL",
            "Violation of State, Tribal or Local Agency Only Regulation",
            "OTH",
            false),
    ];
}
