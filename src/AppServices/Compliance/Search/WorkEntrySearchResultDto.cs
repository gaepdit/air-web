using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.Identity;

namespace AirWeb.AppServices.Compliance.Search;

public record WorkEntrySearchResultDto : IFacilityInfo
{
    public int Id { get; init; }
    public RecordType RecordType { get; init; }
    public string FacilityId { get; init; } = string.Empty;
    public string FacilityName { get; set; } = string.Empty;
    public ApplicationUser? ResponsibleStaff { get; init; }
    public bool IsClosed { get; init; }
    public DateOnly EventDate { get; init; }
    public DateOnly? ClosedDate { get; init; }
    public bool IsDeleted { get; init; }
}
