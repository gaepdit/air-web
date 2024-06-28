using System.Text.RegularExpressions;

namespace AirWeb.Domain.ExternalEntities.Facilities;

public partial record FacilityId
{
    private readonly string _value;

    // Constructor


    public FacilityId(string id) =>
        _value = IsValidFormat(id)
            ? id.Replace("-", "")
            : throw new ArgumentException($"{id} is not a valid Facility ID format.");

    // Properties

    public string Id => $"{_value[..3]}-{_value[3..8]}";
    public string EpaFacilityIdentifier => $"GA00000013{_value}";

    // Operators

    public static implicit operator string(FacilityId id) => id.Id;
    public static explicit operator FacilityId(string id) => new(id);
    public override string ToString() => Id;

    // Format validation

    private static bool IsValidFormat(string id) => FacilityIdRegex().IsMatch(id);

    // FUTURE: Update regex to limit first three digits based on county list.
    // Test at https://regex101.com/r/2uYyHl/4
    // language:regex
    private const string FacilityIdPattern = @"^\d{3}-?\d{5}$";

    [GeneratedRegex(FacilityIdPattern)]
    private static partial Regex FacilityIdRegex();
}
