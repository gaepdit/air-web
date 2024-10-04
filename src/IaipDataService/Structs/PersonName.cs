using System.Text.Json.Serialization;

namespace IaipDataService.Structs;

public readonly record struct PersonName(
    string GivenName,
    string FamilyName,
    string? Prefix = null,
    string? Suffix = null)
{
    [JsonIgnore]
    public string DisplayName =>
        string.Join(" ", new[] { GivenName, FamilyName }.Where(s => !string.IsNullOrEmpty(s)));

    [JsonIgnore]
    public string SortableFullName =>
        string.Join(", ", new[] { FamilyName, GivenName }.Where(s => !string.IsNullOrEmpty(s)));

    public override string ToString() => DisplayName;
}
