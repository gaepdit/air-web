using System.Text.RegularExpressions;

namespace AirWeb.Domain.ExternalEntities.Facilities;

public partial record FacilityId
{
    private readonly string _id = string.Empty;

    // Constructor

    [UsedImplicitly] // Used by ORM.
    private FacilityId() { }

    public FacilityId(string id) => Id = id;

    // Properties

    [Key]
    public string Id
    {
        get => !string.IsNullOrEmpty(_id)
            ? _id
            : throw new InvalidOperationException("Id not initialized.");

        private init => _id = IsValidFormat(value)
            ? value.Replace("-", "")
            : throw new ArgumentException($"The value '{value}' is not a valid Facility ID format.");
    }

    public string FormattedId => $"{_id[..3]}-{_id[3..8]}";
    public string EpaFacilityIdentifier => $"GA00000013{_id}";

    // Operators

    public static implicit operator string(FacilityId id) => id.FormattedId;
    public static explicit operator FacilityId(string id) => new(id);
    public override string ToString() => FormattedId;

    // Format validation
    private static bool IsValidFormat(string id) => FacilityIdRegex().IsMatch(id);

    // FUTURE: Update regex to limit first three digits based on county list.
    // Test at https://regex101.com/r/2uYyHl/4
    // language:regex
    private const string FacilityIdPattern = @"^\d{3}-?\d{5}$";

    [GeneratedRegex(FacilityIdPattern)]
    private static partial Regex FacilityIdRegex();
}
