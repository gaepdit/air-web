using IaipDataService.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IaipDataService.Facilities;

public record RegulatoryData
{
    public Facility Facility { get; init; } = null!;

    // Operating status

    [JsonIgnore]
    public FacilityOperatingStatus OperatingStatusCode { get; init; }

    [Display(Name = "Operating Status")]
    public string OperatingStatus => OperatingStatusCode.GetDisplayName();


    [Display(Name = "Startup Date")]
    public DateTime? StartupDate { get; init; }

    [Display(Name = "Permit Revocation Date")]
    public DateTime? PermitRevocationDate { get; init; }

    // Classifications

    [JsonIgnore]
    public FacilityClassification ClassificationCode { get; init; }

    [Display(Name = "Classification")]
    public string Classification => ClassificationCode.GetDisplayName();

    [JsonIgnore]
    public FacilityCmsClassification CmsClassificationCode { get; init; }

    [Display(Name = "CMS Classification")]
    public string CmsClassification => CmsClassificationCode.GetDisplayName();

    // Industry

    /// <summary>
    /// Facility ownership type.
    /// </summary>
    /// <remarks>
    /// Currently we only track federally-owned facilities, represented by the
    /// OwnershipTypeCode "FDF" and description "Federal Facility (U.S. Government)".
    /// Otherwise, the property is null.
    /// </remarks>
    [Display(Name = "Ownership Type")]
    public string? OwnershipType { get; init; }

    [Display(Name = "SIC")]
    [RegularExpression(SicCodePattern)]
    public string? Sic { get; init; }

    [Display(Name = "NAICS")]
    [RegularExpression(NaicsCodePattern)]
    public string? Naics { get; init; }

    [Display(Name = "RMP ID")]
    [RegularExpression(RmpIdPattern)]
    public string? RmpId { get; init; }

    // Nonattainment areas

    [Display(Name = "1-hr Ozone")]
    public OneHourOzoneNonattainmentStatus OneHourOzoneNonattainment { get; init; }

    [Display(Name = "8-hr Ozone")]
    public EightHourOzoneNonattainmentStatus EightHourOzoneNonattainment { get; init; }

    [Display(Name = "PM 2.5")]
    public PmFineNonattainmentStatus PmFineNonattainment { get; init; }

    // Regulations

    [Display(Name = "NSPS Fee Exempt")]
    public bool NspsFeeExempt { get; init; }

    // The following lists are returned by the Facility Service when requesting facility details, but not when
    // requesting a facility summary.

    /// <summary>
    /// List of air programs that apply to a facility.
    /// </summary>
    /// <remarks>
    /// Possible values:
    /// SIP, Federal SIP, Non-Federal SIP, CFC Tracking, PSD, NSR, NESHAP, NSPS, FESOP,
    /// Acid Precipitation, Native American, MACT, Title V, Risk Management Plan
    /// </remarks>
    [Display(Name = "Air Programs")]
    public List<AirProgram> AirPrograms { get; init; } = [];

    public IEnumerable<string> AirProgramsAsStrings =>
        AirPrograms.Select(program => program.GetDisplayName());

    /// <summary>
    /// List of pollutants that apply to a facility.
    /// </summary>
    public List<Pollutant> Pollutants { get; init; } = [];

    /// <summary>
    /// List of air program classifications that apply to a facility.
    /// </summary>
    /// <remarks>
    /// Possible values: NSR/PSD Major, HAPs Major
    /// </remarks>
    [Display(Name = "Program Classifications")]
    public List<AirProgramClassification> ProgramClassifications { get; init; } = [];

    public IEnumerable<string> ProgramClassificationsAsStrings =>
        ProgramClassifications.Select(program => program.GetDisplayName());

    #region Regex patterns

    // Valid SIC codes are one to four digits
    private const string SicCodePattern = @"^\d{1,4}$";

    // Valid NAICS codes are two to six digits
    private const string NaicsCodePattern = @"^\d{2,6}$";

    // Valid RMP IDs are in the form 0000-0000-0000 (with the dashes)
    private const string RmpIdPattern = @"^\d{4}-\d{4}-\d{4}$";

    #endregion
}
