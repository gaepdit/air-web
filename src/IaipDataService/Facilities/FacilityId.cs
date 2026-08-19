using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace IaipDataService.Facilities;

public partial record FacilityId
{
    private readonly string? _id;

    // Constructor

    [UsedImplicitly] // Used by ORM.
    private FacilityId() { }

    public FacilityId(string id) => Id = id;

    // Properties

    /// <summary>
    /// The short form of the Facility ID without a hyphen, e.g. "00123456".
    /// </summary>
    [Key]
    public string Id
    {
        get => _id ?? throw new InvalidOperationException("Id not initialized.");
        private init => _id = Normalize(value);
    }

    /// <summary>
    /// `FormattedId` is the long form of the Facility ID with a hyphen, e.g. "001-23456".
    /// </summary>
    public string FormattedId => $"{Id[..3]}-{Id[3..8]}";

    /// <summary>
    /// The ID used by the EPA data exchange.
    /// </summary>
    public string EpaFacilityId => $"GA00000013{Id}";

    /// <summary>
    /// The ID used by the AIRBRANCH database.
    /// </summary>
    public string IaipDbId => $"{IaipDbFacilityPrefix}{Id}";

    private const string IaipDbFacilityPrefix = "0413";

    /// <summary>
    /// The 3-digit FIPS county code.
    /// </summary>
    public string CountyCode => Id[..3];

    public const string PortableSourceCountyCode = "777";

    // Operators
    public static implicit operator string(FacilityId id) => id.FormattedId;
    public static explicit operator FacilityId(string id) => new(id);
    public override string ToString() => FormattedId;
    public virtual bool Equals(FacilityId? other) => !string.IsNullOrEmpty(other?._id) && other._id == _id;

    // `GetHashCode()` is required by `Equals()`
    public override int GetHashCode() => string.GetHashCode(_id, StringComparison.Ordinal);

    public static bool TryParse([NotNullWhen(true)] string? s, [NotNullWhen(true)] out FacilityId? result)
    {
        if (string.IsNullOrEmpty(s))
        {
            result = null;
            return false;
        }

        try
        {
            result = new FacilityId(s);
            return true;
        }
        catch (Exception)
        {
            result = null;
            return false;
        }
    }

    // Format validation
    public const string FacilityIdFormatError = "The Facility ID entered is not valid.";

    public const string FacilityNotExistsError =
        "A Facility with that ID does not exist or has not been approved in the IAIP.";

    private static string Normalize(string input)
    {
        var value = input.Trim();

        if (IsValidEpaDxFormat(value)) return input[10..];
        if (!IsValidStandardFormat(value)) throw new ArgumentException(FacilityIdFormatError);

        var dashIndex = value.IndexOf('-');
        if (dashIndex == -1)
        {
            return value.Length switch
            {
                8 => value,
                12 => value[4..],
                _ => throw new ArgumentException(FacilityIdFormatError),
            };
        }

        return value[..dashIndex].PadLeft(3, '0') +
               value[(dashIndex + 1)..].PadLeft(5, '0');
    }

    public static bool IsValidFormat(string id) => IsValidStandardFormat(id) || IsValidEpaDxFormat(id);

    // Format as Facility ID if possible, otherwise return original input.
    public static string? TryFormat(string? input) =>
        TryParse(input, out var facilityId) ? facilityId.FormattedId : input;

    // --- Regex ---

    // == Standard Format
    [GeneratedRegex(FacilityIdPattern)]
    private static partial Regex FacilityIdRegex { get; }

    // Test at https://regex101.com/r/2uYyHl/10
    // language:regex
    private const string FacilityIdPattern =
        "^(?:^(?:0413)?(?:777|321|3[0-1][13579]|[0-2][0-9][13579])(?!00000)[0-9]{5})$|(?:^(?:777|321|3[0-1][13579]|[0-2]?[0-9]?[13579])-(?!0{1,5}$)[0-9]{1,5})$";

    private static bool IsValidStandardFormat(string id) => FacilityIdRegex.IsMatch(id);

    // == EPA Data Exchange Format
    [GeneratedRegex(EpaFacilityIdPattern)]
    private static partial Regex EpaFacilityIdRegex { get; }

    // Test at https://regex101.com/r/gZ9Go3/3
    // language:regex
    private const string EpaFacilityIdPattern =
        "^GA00000013(?:777|321|3[0-1][13579]|[0-2][0-9][13579])(?!00000)[0-9]{5}$";

    private static bool IsValidEpaDxFormat(string id) => EpaFacilityIdRegex.IsMatch(id);

    // === Simplified Formats
    // language:regex
    public const string DisplayIdFormat = "[0-9]{3}-?[0-9]{5}";

    // language:regex
    public const string LooseIdFormat = "[0-9]{1,3}-[0-9]{1,5}|[0-9]{8}";

    public const string LooseIdFormatError = "Invalid AIRS Number format.";
}
