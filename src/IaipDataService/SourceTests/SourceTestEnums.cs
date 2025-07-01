using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace IaipDataService.SourceTests;

// Values from the "ISMPREPORTTYPE" table
[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum ReportType
{
    [Display(Name = "Monitor Certification")] MonitorCertification = 1,
    [Display(Name = "PEMS Development")] PemsDevelopment = 2,
    [Display(Name = "RATA/CEMS")] RataCems = 3,
    [Display(Name = "Source Test")] SourceTest = 4,
    [Display(Name = "Source Test")] NA = 5,
}

// Values from the "ISMPDOCUMENTTYPE" table
[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum DocumentType
{
    [Display(Name = "Unassigned")] Unassigned = 001,
    [Display(Name = "One Stack (Two Runs)")] OneStackTwoRuns = 002,
    [Display(Name = "One Stack (Three Runs)")] OneStackThreeRuns = 003,
    [Display(Name = "One Stack (Four Runs)")] OneStackFourRuns = 004,
    [Display(Name = "Two Stack (Standard)")] TwoStackStandard = 005,
    [Display(Name = "Two Stack (DRE)")] TwoStackDre = 006,
    [Display(Name = "Loading Rack")] LoadingRack = 007,
    [Display(Name = "Pond Treatment")] PondTreatment = 008, // (Pulping Process Condensate)
    [Display(Name = "Gas Concentration")] GasConcentration = 009,
    [Display(Name = "Flare")] Flare = 010,
    [Display(Name = "RATA")] Rata = 011,
    [Display(Name = "Memorandum (Standard)")] MemorandumStandard = 012,
    [Display(Name = "Memorandum (To File)")] MemorandumToFile = 013,
    [Display(Name = "Method 9 (Multi.)")] Method9Multi = 014,
    [Display(Name = "Method 22")] Method22 = 015,
    [Display(Name = "Method 9 (Single)")] Method9Single = 016,
    [Display(Name = "PTE (Permanent Total Enclosure)")] PTE = 018,
}
