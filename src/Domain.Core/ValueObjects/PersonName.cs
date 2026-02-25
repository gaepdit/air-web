using GaEpd.AppLibrary.Extensions;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.Core.ValueObjects;

public readonly record struct PersonName(
    string GivenName,
    string FamilyName,
    string? Prefix = null,
    string? Suffix = null
)
{
    [JsonIgnore]
    public string SortableFullName => new[] { FamilyName, GivenName }.ConcatWithSeparator(", ");
}
