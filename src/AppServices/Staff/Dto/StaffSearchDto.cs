using AirWeb.AppServices.CommonSearch;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AirWeb.AppServices.Staff.Dto;

public record StaffSearchDto(
    // Sorting
    StaffSortBy Sort,

    // Search fields
    string? Name,
    [EmailAddress] string? Email,
    string? Role,
    Guid? Office,
    SearchStaffStatus? Status
) : ISearchDto
{
    // UI Routing
    public IDictionary<string, string?> AsRouteValues() => new Dictionary<string, string?>
    {
        { nameof(Sort), Sort.ToString() },
        { nameof(Name), Name },
        { nameof(Email), Email },
        { nameof(Role), Role },
        { nameof(Office), Office.ToString() },
        { nameof(Status), Status?.ToString() },
    };

    public StaffSearchDto TrimAll() => this with
    {
        Name = Name?.Trim(),
        Email = Email?.Trim(),
    };

    public string SortByName => Sort.ToString();
}

// Search enums
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SearchStaffStatus
{
    Active,
    Inactive,
    All,
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StaffSortBy
{
    [Description("FamilyName, GivenName")] NameAsc,

    [Description("FamilyName desc, GivenName desc")]
    NameDesc,

    [Description("Office.Name, FamilyName, GivenName")]
    OfficeAsc,

    [Description("Office.Name desc, FamilyName, GivenName")]
    OfficeDesc,
}
