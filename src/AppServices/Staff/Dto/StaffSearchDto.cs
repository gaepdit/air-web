using AirWeb.AppServices.CommonSearch;
using GaEpd.AppLibrary.Extensions;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AirWeb.AppServices.Staff.Dto;

public record StaffSearchDto : ISearchDto<StaffSearchDto>, ISearchDto
{
    public StaffSortBy Sort { get; init; } = StaffSortBy.NameAsc;
    public string SortByName => Sort.ToString();
    public string Sorting => Sort.GetDescription();

    public string? Name { get; init; }
    public string? Email { get; init; }
    public string? Role { get; init; }
    public Guid? Office { get; init; }
    public SearchStaffStatus? Status { get; init; }

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
    [Description("FamilyName, GivenName")]
    NameAsc,

    [Description("FamilyName desc, GivenName desc")]
    NameDesc,

    [Description("Office.Name, FamilyName, GivenName")]
    OfficeAsc,

    [Description("Office.Name desc, FamilyName, GivenName")]
    OfficeDesc,
}
