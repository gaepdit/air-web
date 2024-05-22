using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace AirWeb.Domain.Entities.Facilities;

public partial record FacilityId
{
    [JsonIgnore]
    public string Id { get; }

    public FacilityId(string id) =>
        Id = IsValidFormat(id)
            ? id.Replace("-", "")
            : throw new ArgumentException($"{id} is not a valid Facility ID format.");

    public string FormattedId => $"{Id[..3]}-{Id[3..8]}";
    public string EpaFacilityIdentifier => $"GA00000013{Id}";

    public static implicit operator FacilityId(string id) => new(id);
    public override string ToString() => FormattedId;

    // Facility ID format

    private static bool IsValidFormat(string id) => FacilityIdRegex().IsMatch(id);

    // Test at https://regex101.com/r/2uYyHl/4
    // language:regex
    private const string FacilityIdPattern = @"^\d{3}-?\d{5}$";

    [GeneratedRegex(FacilityIdPattern)]
    private static partial Regex FacilityIdRegex();
}
