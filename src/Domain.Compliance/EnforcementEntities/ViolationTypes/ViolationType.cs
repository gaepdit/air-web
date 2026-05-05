using GaEpd.AppLibrary.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace AirWeb.Domain.Compliance.EnforcementEntities.ViolationTypes;

public record ViolationType
{
    [Key]
    [StringLength(5)]
    public required string Code { get; init; }

    [StringLength(300)]
    public required string Description { get; init; }

    [StringLength(3)]
    public required ViolationSeverity SeverityCode { get; init; }

    public bool Deprecated { get; init; }

    public string Current => Deprecated ? "Historic" : "Current";
    public string Display => $"{SeverityCode}: {Description} ({Code})";
    public string SeverityCodeDisplay => SeverityCode.GetDisplayName();
}

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum ViolationSeverity
{
    [Display(Name = "High Priority Violation")]
    HPV,

    [Display(Name = "Federally Reportable Violation")]
    FRV,

    [Display(Name = "Federally Enforceable Violation but not Federally Reportable")]
    NFR,

    [Display(Name = "Other")]
    OTH,
}
