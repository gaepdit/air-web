using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace IaipDataService.Facilities;

public partial record FacilityId : IComparable<FacilityId>
{
    private readonly string? _id;

    // Constructor

    [UsedImplicitly] // Used by ORM.
    private FacilityId() { }

    public FacilityId(string id) => Id = id;

    // Properties


    /// <summary>
    /// `Id` is the short form of the Facility ID without a hyphen, e.g. "00123456".
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
    /// `EpaFacilityIdentifier` is the ID used by EPA.
    /// </summary>
    public string EpaFacilityIdentifier => $"GA00000013{Id}";

    // Operators
    public static implicit operator string(FacilityId id) => id.FormattedId;
    public static explicit operator FacilityId(string id) => new(id);
    public override string ToString() => FormattedId;
    public virtual bool Equals(FacilityId? other) => !string.IsNullOrEmpty(other?._id) && other._id == _id;
    public int CompareTo(FacilityId? other) => string.Compare(_id, other?._id, StringComparison.Ordinal);
    public override int GetHashCode() => string.GetHashCode(_id, StringComparison.Ordinal);

    public static bool TryParse([NotNullWhen(true)] string? s, [NotNullWhen(true)] out FacilityId? result)
    {
        if (s is null)
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

    private static string Normalize(string input)
    {
        var value = input.Trim();

        if (!IsValidFormat(value)) throw new ArgumentException(FacilityIdFormatError);

        var dashIndex = value.IndexOf('-');
        if (dashIndex == -1)
        {
            return value.Length switch
            {
                8 => value,
                12 when value.StartsWith("0413") => value[4..],
                _ => throw new ArgumentException(FacilityIdFormatError),
            };
        }

        var countyPart = value[..dashIndex].PadLeft(3, '0');
        var restPart = value[(dashIndex + 1)..].PadLeft(5, '0');
        return countyPart + restPart;
    }

    // FUTURE: `CleanPartialFacilityId` is used to clean up search form entries.
    //       Instead of just replacing the entry, though, model validation should be 
    //       used to notify the user of invalid entries.
    public static string CleanPartialFacilityId(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;
        var cleanFacilityId = new string(input.Where(c => c == '-' || char.IsDigit(c)).ToArray());
        return ShortFormatRegex().IsMatch(cleanFacilityId)
            ? cleanFacilityId.Insert(3, "-")
            : cleanFacilityId;
    }

    // Regex
    [GeneratedRegex(FacilityIdEnclosedPattern)]
    private static partial Regex FacilityIdRegex();

    // Test at https://regex101.com/r/2uYyHl/9
    // language:regex
    private const string FacilityIdPattern =
        @"(?:^(?:0413)?(?:777|321|3[0-1][13579]|[0-2][0-9][13579])(?!00000)\d{5})$|(?:^(?:777|321|3[0-1][13579]|[0-2]?[0-9]?[13579])-(?!0{1,5}$)\d{1,5})";

    public const string FacilityIdEnclosedPattern = $"^{FacilityIdPattern}$";
    public static bool IsValidFormat(string id) => FacilityIdRegex().IsMatch(id);

    [GeneratedRegex(StandardFormat)]
    private static partial Regex StandardFormatRegex();

    // language:regex
    private const string StandardFormat = @"^\d{3}-\d{5}$";
    public static bool IsStandardFormat(string id) => StandardFormatRegex().IsMatch(id);

    [GeneratedRegex(ShortFormat)]
    private static partial Regex ShortFormatRegex();

    // language:regex
    private const string ShortFormat = @"^\d{8}$";

    // language:regex
    public const string SimplifiedFormat = @"\d{3}-?\d{5}";
}
