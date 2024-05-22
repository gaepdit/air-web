using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace AirWeb.Domain.Entities.Facilities;

/// <summary>
///     The operational status of a facility.
/// </summary>
/// <remarks>Stored in the database as a single-character string.</remarks>
public enum FacilityOperatingStatus
{
    [Description("Unspecified")] U,
    [Description("Operational")] O,
    [Description("Planned")] P,
    [Description("Under Construction")] C,
    [Description("Temporarily Closed")] T,
    [Description("Closed/Dismantled")] X,
    [Description("Seasonal Operation")] I,
}

/// <summary>
///     The source classification of a facility (based on permit type).
/// </summary>
/// <remarks>Stored in the database as a two-character string.</remarks>
[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum FacilityClassification
{
    [Description("Unspecified")] Unspecified,
    [Description("Major source")] A,
    [Description("Minor source")] B,
    [Description("Synthetic minor")] SM,
    [Description("Permit by rule")] PR,
    [Description("Unclassified")] C,
}
