using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace IaipDataService.Facilities;

/// <summary>
/// The operational status of a facility.
/// </summary>
/// <remarks>Stored in the IAIP database as a single-character string.</remarks>
public enum FacilityOperatingStatus
{
    [Display(Name = "Unspecified")] U,
    [Display(Name = "Operational")] O,
    [Display(Name = "Planned")] P,
    [Display(Name = "Under Construction")] C,
    [Display(Name = "Temporarily Closed")] T,
    [Display(Name = "Closed/Dismantled")] X,
    [Display(Name = "Seasonal Operation")] I,
}

/// <summary>
/// The source classification of a facility (based on permit type).
/// </summary>
/// <remarks>Stored in the IAIP database as a one-character string.</remarks>
[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum FacilityClassification
{
    [Display(Name = "Unspecified")] Unspecified,
    [Display(Name = "Major source")] A,
    [Display(Name = "Minor source")] B,
    [Display(Name = "Synthetic minor")] SM,
    [Display(Name = "Permit by rule")] PR,
    [Display(Name = "Unclassified")] C,
}

/// <summary>
/// The CMS classification of a facility.
/// </summary>
/// <remarks>Stored in the IAIP database as a nullable one-character string.</remarks>
public enum FacilityCmsClassification
{
    [Display(Name = "Unspecified")] Unspecified,
    [Display(Name = "Major")] A,
    [Display(Name = "SM")] S,
    [Display(Name = "None")] X,
    [Display(Name = "Mega-site")] M,
}

/// <summary>
/// Specifies whether a facility is located within a one-hour ozone nonattainment area.
/// </summary>
/// <remarks>
/// The value of each enumeration member is significant because the members are stored
/// and retrieved from the IAIP database in a coded string (along with EightHourNonattainmentStatus
/// and PMFineNonattainmentStatus.)
/// </remarks>
public enum OneHourOzoneNonattainmentStatus
{
    No = 0,
    Yes = 1,
    Contribute = 2,
}

/// <summary>
/// Specifies whether a facility is located within an eight-hour ozone nonattainment area.
/// </summary>
/// <remarks>
/// The value of each enumeration member is significant because the members are stored
/// and retrieved from the IAIP database in a coded string (along with OneHourNonattainmentStatus
/// and PMFineNonattainmentStatus.)
/// </remarks>
public enum EightHourOzoneNonattainmentStatus
{
    None = 0,
    Atlanta = 1,
    Macon = 2,
}

/// <summary>
/// Specifies whether a facility is located within a PM Fine (PM 2.5) nonattainment area.
/// </summary>
/// <remarks>
/// The value of each enumeration member is significant because the members are stored
/// and retrieved from the IAIP database in a coded string (along with EightHourNonattainmentStatus
/// and OneHourNonattainmentStatus.)
/// </remarks>
public enum PmFineNonattainmentStatus
{
    None = 0,
    Atlanta = 1,
    Chattanooga = 2,
    Floyd = 3,
    Macon = 4,
}
