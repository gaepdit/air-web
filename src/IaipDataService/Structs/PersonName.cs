using IaipDataService.Utilities;
using System.Text.Json.Serialization;

namespace IaipDataService.Structs;

public readonly record struct PersonName(
    string GivenName,
    string FamilyName,
    string? Prefix = null,
    string? Suffix = null)
{
    [JsonIgnore]
    public string FullName => new[] { GivenName, FamilyName }.ConcatWithSeparator();
}
