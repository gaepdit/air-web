using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace IaipDataService.SourceTests;

// Values from the "ISMPREPORTTYPE" table
[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum ReportType
{
    [Description("Monitor Certification")] MonitorCertification = 1,
    [Description("PEMS Development")] PemsDevelopment = 2,
    [Description("RATA/CEMS")] RataCems = 3,
    [Description("Source Test")] SourceTest = 4,
    [Description("Source Test")] NA = 5,
}

// Values from the "ISMPDOCUMENTTYPE" table
[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum DocumentType
{
    [Description("Unassigned")] Unassigned = 001,
    [Description("One Stack (Two Runs)")] OneStackTwoRuns = 002,
    [Description("One Stack (Three Runs)")] OneStackThreeRuns = 003,
    [Description("One Stack (Four Runs)")] OneStackFourRuns = 004,
    [Description("Two Stack (Standard)")] TwoStackStandard = 005,
    [Description("Two Stack (DRE)")] TwoStackDre = 006,
    [Description("Loading Rack")] LoadingRack = 007,
    [Description("Pond Treatment")] PondTreatment = 008, // (Pulping Process Condensate)
    [Description("Gas Concentration")] GasConcentration = 009,
    [Description("Flare")] Flare = 010,
    [Description("RATA")] Rata = 011,
    [Description("Memorandum (Standard)")] MemorandumStandard = 012,
    [Description("Memorandum (To File)")] MemorandumToFile = 013,
    [Description("Method 9 (Multi.)")] Method9Multi = 014,
    [Description("Method 22")] Method22 = 015,
    [Description("Method 9 (Single)")] Method9Single = 016,
    [Description("PTE (Permanent Total Enclosure)")] PTE = 018,
}
